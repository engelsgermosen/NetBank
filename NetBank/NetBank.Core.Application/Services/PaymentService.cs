﻿using AutoMapper;
using NetBank.Core.Application.Interfaces.Repositories;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Payment;
using NetBank.Core.Domain.Entities;
using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Application.Services
{
    public class PaymentService : GenericService<SavePaymentViewModel,PaymentViewModel,Payment>, IPaymentService
    {

        private readonly IPaymentRepository _paymentRepository;

        private readonly IMapper _mapper;

        private readonly IProductService _productService;

        public PaymentService(IPaymentRepository paymentRepository, IProductService productService, IMapper mapper) : base(paymentRepository, mapper) 
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _productService = productService;
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

<<<<<<< HEAD
        //Pago beneficiario

        //public async Task<SavePaymentViewModel> PaymentBeneficiarie(SavePaymentViewModel payment)
        //{
        //    SavePaymentViewModel response = new()
        //    {
        //        HasError = false,
        //    };

        //    if (payment.DestinationAccountNumber == payment.OriginAccountNumber)
        //    {
        //        response.HasError = true;
        //        response.Error = "No puedes pagarte a ti mismo";
        //        return response;
        //    }

        //    var cuentaDestino = await _productService.GetProductByAccountNumber(payment.DestinationAccountNumber);
        //    var cuentaOrigen = await _productService.GetProductByAccountNumber(payment.OriginAccountNumber);


        //    if (cuentaDestino == null)
        //    {
        //        response.HasError = true;
        //        response.Error = "Esa cuenta no existe";
        //        return response;
        //    }
        //    if (cuentaDestino.ProductType != ProductType.CuentaAhorro)
        //    {
        //        response.HasError = true;
        //        response.Error = "La cuenta que digitaste no es una cuenta de ahorro";
        //        return response;
        //    }

        //    if (payment.Amonut > cuentaOrigen.Balance)
        //    {
        //        response.HasError = true;
        //        response.Error = "La cuenta de origen no tiene fondos suficientes";
        //        return response;
        //    }

        //    return response;
        //}
=======


        //Pago a Trajeta de Credito
        public async Task<SavePaymentViewModel> PaymentCreditCard(SavePaymentViewModel payment)
        {
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

            return _mapper.Map<SavePaymentViewModel>(pago);
        }


        //Pago a Trajeta de Credito
        public async Task<SavePaymentViewModel> PaymentLoan(SavePaymentViewModel payment)
        {
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

            return _mapper.Map<SavePaymentViewModel>(pago);
        }


>>>>>>> natanael/app

    }
}
