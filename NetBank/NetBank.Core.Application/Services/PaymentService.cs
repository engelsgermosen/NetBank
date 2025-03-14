using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Interfaces.Repositories;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Payment;
using NetBank.Core.Application.ViewModels.Transaction;
using NetBank.Core.Domain.Entities;
using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Application.Services
{
    public class PaymentService : GenericService<SavePaymentViewModel,PaymentViewModel,Payment>, IPaymentService
    {

        private readonly IPaymentRepository _paymentRepository;

        private readonly IMapper _mapper;

        private readonly IProductService _productService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AuthenticationResponse userInSession;

        public PaymentService(IPaymentRepository paymentRepository, IProductService productService, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(paymentRepository, mapper) 
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            userInSession = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }


        public async Task<PaymentCountViewModel> GetPaymentCount()
        {
            var response = await _paymentRepository.GetAllAsync();

            PaymentCountViewModel pay = new()
            {
                PaymentTotals = response.Count,
                TodayPayments = response.Where(x => x.PaymentDate.Date == DateTime.Now.Date).Count(),
            };

            return pay;
        }
        public async Task<SavePaymentViewModel> PaymentExpressAndBeneficiarie(SavePaymentViewModel payment)
        {
            SavePaymentViewModel response = new()
            {
                HasError = false,
            };

            if (payment.DestinationAccountNumber == payment.OriginAccountNumber)
            {
                response.HasError = true;
                response.Error = "No puedes pagarte a ti mismo";
                return response;
            }

            var cuentaDestino = await _productService.GetProductByAccountNumber(payment.DestinationAccountNumber);
            var cuentaOrigen = await _productService.GetProductByAccountNumber(payment.OriginAccountNumber);


            if(cuentaDestino == null)
            {
                response.HasError = true;
                response.Error = "Esa cuenta no existe";
                return response;
            }
            if(cuentaDestino.ProductType != ProductType.CuentaAhorro)
            {
                response.HasError = true;
                response.Error = "La cuenta que digitaste no es una cuenta de ahorro";
                return response;
            }

            if(payment.Amonut > cuentaOrigen.Balance)
            {
                response.HasError = true;
                response.Error = "La cuenta de origen no tiene fondos suficientes";
                return response;
            }

            return response;
        }

        public async Task<SavePaymentViewModel> ConfirmPaymentExpressAndBeneficiarie(SavePaymentViewModel payment)
        {
            Payment pago = _mapper.Map<Payment>(payment);
            pago = await _paymentRepository.AddAsync(pago);

            var cuentaOrigen = await _productService.GetByIdSaveViewModel(payment.OriginAccountNumber);
            cuentaOrigen.Balance -= payment.Amonut;
            await _productService.UpdateAsync(cuentaOrigen, cuentaOrigen.AccountNumber);

            var cuentaDestino = await _productService.GetByIdSaveViewModel(payment.DestinationAccountNumber);
            cuentaDestino.Balance += payment.Amonut;
            await _productService.UpdateAsync(cuentaDestino, cuentaDestino.AccountNumber);



            return _mapper.Map<SavePaymentViewModel>(pago);
        }



        //Pago a Trajeta de Credito
        public async Task<SavePaymentViewModel> PaymentCreditCard(SavePaymentViewModel payment)
        {
            payment.PaymentType = PaymentType.PaymentCreditCard;
            payment.UserId = userInSession.Id;
            SavePaymentViewModel response = new()
            {
                HasError = false,
            };

            var tarjetaDestino = await _productService.GetProductByAccountNumber(payment.DestinationAccountNumber);
            var cuentaOrigen = await _productService.GetProductByAccountNumber(payment.OriginAccountNumber);

            if (cuentaOrigen == null)
            {
                response.HasError = true;
                response.Error = "Por favor seleccione una cuenta de origen";
                return response;
            }
            if (tarjetaDestino == null)
            {
                response.HasError = true;
                response.Error = "Seleccione la tarjeta de credito que desea pagar";
                return response;
            }

            if (payment.Amonut > cuentaOrigen.Balance)
            {
                response.HasError = true;
                response.Error = "La cuenta de origen no tiene fondos suficientes";
                return response;
            }

            if (tarjetaDestino.AmountOwed == 0)
            {
                response.HasError = true;
                response.Error = "No puedes pagar esta targeta porque no le debes dinero";
                return response;
            }

            if (payment.Amonut > tarjetaDestino.AmountOwed)
            {
                payment.Amonut = tarjetaDestino.AmountOwed.Value;
            }

            Payment pago = _mapper.Map<Payment>(payment);
            pago = await _paymentRepository.AddAsync(pago);

            var cuentaToGetMoney = await _productService.GetByIdSaveViewModel(payment.OriginAccountNumber);
            cuentaToGetMoney.Balance -= payment.Amonut;
            await _productService.UpdateAsync(cuentaToGetMoney, cuentaToGetMoney.AccountNumber);

            var tarjetaParaDepositar = await _productService.GetByIdSaveViewModel(payment.DestinationAccountNumber);
            tarjetaParaDepositar.AmountOwed -= payment.Amonut;
            await _productService.UpdateAsync(tarjetaParaDepositar, tarjetaParaDepositar.AccountNumber);

            response = _mapper.Map<SavePaymentViewModel>(pago);

            if(tarjetaParaDepositar.AmountOwed == 0)
            {
                response.IsSaldo = true;
            }

            return response;
        }


        //Pago a prestamo
        public async Task<SavePaymentViewModel> PaymentLoan(SavePaymentViewModel payment)
        {
            payment.PaymentType = PaymentType.PaymentLoan;
            payment.UserId = userInSession.Id;
            SavePaymentViewModel response = new()
            {
                HasError = false,
            };

            var LoanDestino = await _productService.GetProductByAccountNumber(payment.DestinationAccountNumber);
            var cuentaOrigen = await _productService.GetProductByAccountNumber(payment.OriginAccountNumber);

            if (cuentaOrigen == null)
            {
                response.HasError = true;
                response.Error = "Por favor seleccione una cuenta de origen";
                return response;
            }
            if (LoanDestino == null)
            {
                response.HasError = true;
                response.Error = "Seleccione el prestamo que desea pagar";
                return response;
            }

            if (payment.Amonut > cuentaOrigen.Balance)
            {
                response.HasError = true;
                response.Error = "La cuenta de origen no tiene fondos suficientes";
                return response;
            }

            if (LoanDestino.AmountOwed == 0)
            {
                response.HasError = true;
                response.Error = "No puedes pagar este Prestamo porque no le debes dinero";
                return response;
            }

            if (payment.Amonut > LoanDestino.AmountOwed)
            {
                payment.Amonut = LoanDestino.AmountOwed.Value;
            }

            Payment pago = _mapper.Map<Payment>(payment);
            pago = await _paymentRepository.AddAsync(pago);

            var cuentaToGetMoney = await _productService.GetByIdSaveViewModel(payment.OriginAccountNumber);
            cuentaToGetMoney.Balance -= payment.Amonut;
            await _productService.UpdateAsync(cuentaToGetMoney, cuentaToGetMoney.AccountNumber);

            var loanParaDepositar = await _productService.GetByIdSaveViewModel(payment.DestinationAccountNumber);
            loanParaDepositar.AmountOwed -= payment.Amonut;
            await _productService.UpdateAsync(loanParaDepositar, loanParaDepositar.AccountNumber);

            response = _mapper.Map<SavePaymentViewModel>(pago);

            if(loanParaDepositar.AmountOwed == 0)
            {
                await _productService.DeleteProduct(loanParaDepositar.AccountNumber,ProductType.Prestamo,loanParaDepositar.UserId);
                response.IsSaldo = true;
            }
            return response;
        }
    }
}
