using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.Services.Repositories;
using NetBank.Core.Application.ViewModels.Transaction;
using NetBank.Core.Domain.Entities;
using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Application.Services
{
    public class TransactionService : GenericService<SaveTransactionViewModel,TransactionViewModel,Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _repository;

        private readonly IMapper _mapper;

        private readonly IProductService _productService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AuthenticationResponse userInSession;


        public TransactionService(ITransactionRepository repository, IMapper mapper, IProductService productService, IHttpContextAccessor httpContextAccessor) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            userInSession = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<TransaccionCountViewModel> GetTransaccionCount()
        {
            var response = await _repository.GetAllAsync();

            TransaccionCountViewModel transaccionCountViewModel = new ()
            {
                TransactionTotals = response.Count,
                TodayTransaction = response.Where(x => x.TransactionDate.Date == DateTime.Now.Date).Count(),
            };

            return transaccionCountViewModel;
        }

        public async Task<SaveTransactionViewModel> CashAdvance(SaveTransactionViewModel saveTransactionViewModel)
        {
            saveTransactionViewModel.TransactionType = TransactionType.CashAdvance;
            saveTransactionViewModel.UserId = userInSession.Id;

            SaveTransactionViewModel response = new()
            {
                HasError = false,
            };

            var cuentaOrigen = await _productService.GetByIdSaveViewModel(saveTransactionViewModel.OriginProductId ?? 0);
            var cuentaDestino = await _productService.GetByIdSaveViewModel(saveTransactionViewModel.DestinationProductId ?? 0);

            if((cuentaOrigen.CreditLimit - cuentaOrigen.AmountOwed) < saveTransactionViewModel.Amount)
            {
                response.HasError = true;
                response.Error = "El monto excede el limite disponible de esta tarjeta";
                return response;
            }

            Transaction transaction = _mapper.Map<Transaction>(saveTransactionViewModel);
            transaction = await _repository.AddAsync(transaction);


            cuentaOrigen.AmountOwed += transaction.Amount * (decimal)1.0625;
            await _productService.UpdateAsync(cuentaOrigen, cuentaOrigen.AccountNumber);

            cuentaDestino.Balance += transaction.Amount;
            await _productService.UpdateAsync(cuentaDestino, cuentaDestino.AccountNumber);

           
            response.Id = transaction.Id;
            response.Amount = transaction.Amount;
            response.OriginProductId = transaction.OriginProductId;
            response.DestinationProductId = transaction.DestinationProductId;
            return response;
        }

        public async Task<SaveTransactionViewModel> TransferCash(SaveTransactionViewModel saveTransactionViewModel)
        {
            saveTransactionViewModel.TransactionType = TransactionType.Transfers;
            saveTransactionViewModel.UserId = userInSession.Id;

            SaveTransactionViewModel response = new()
            {
                HasError = false,
            };

            if(saveTransactionViewModel.OriginProductId == saveTransactionViewModel.DestinationProductId)
            {
                response.HasError = true;
                response.Error = "Selecciona cuentas diferentes";
                return response;
            }

            var cuentaOrigen = await _productService.GetByIdSaveViewModel(saveTransactionViewModel.OriginProductId ?? 0);
            var cuentaDestino = await _productService.GetByIdSaveViewModel(saveTransactionViewModel.DestinationProductId ?? 0);

            if (cuentaOrigen.Balance  < saveTransactionViewModel.Amount)
            {
                response.HasError = true;
                response.Error = "El monto excede el limite del balance de la cuenta de origen";
                return response;
            }

            Transaction transaction = _mapper.Map<Transaction>(saveTransactionViewModel);
            transaction = await _repository.AddAsync(transaction);


            cuentaOrigen.Balance -= transaction.Amount;
            await _productService.UpdateAsync(cuentaOrigen, cuentaOrigen.AccountNumber);

            cuentaDestino.Balance += transaction.Amount;
            await _productService.UpdateAsync(cuentaDestino, cuentaDestino.AccountNumber);


            response.Id = transaction.Id;
            response.Amount = transaction.Amount;
            response.OriginProductId = transaction.OriginProductId;
            response.DestinationProductId = transaction.DestinationProductId;
            return response;
        }
    }
}
