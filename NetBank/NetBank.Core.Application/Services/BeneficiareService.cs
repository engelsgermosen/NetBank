using AutoMapper;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.Services.Repositories;
using NetBank.Core.Application.ViewModels.Beneficiare;
using NetBank.Core.Domain.Entities;

namespace NetBank.Core.Application.Services
{
    public class BeneficiareService : GenericService<SaveBeneficiareViewModel, BeneficiareViewModel, Beneficiarie>, IBeneficiareService
    {
        private readonly IBeneficiareRepository _repository;

        private readonly IMapper _mapper;

        public BeneficiareService(IBeneficiareRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
    }
}
