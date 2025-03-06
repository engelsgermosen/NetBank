using AutoMapper;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.Services.Repositories;
using NetBank.Core.Application.ViewModels.Transaction;
using NetBank.Core.Domain.Entities;

namespace NetBank.Core.Application.Services
{
    internal class TransactionService : GenericService<SaveTransactionViewModel,TransactionViewModel,Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _repository;

        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
