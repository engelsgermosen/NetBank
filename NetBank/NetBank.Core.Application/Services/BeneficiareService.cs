using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.Services.Repositories;
using NetBank.Core.Application.ViewModels.Beneficiare;
using NetBank.Core.Application.ViewModels.Product;
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
        
       public async Task<List<BeneficiareViewModel>> GetBeneficiariosByUserId(string id)
       {
           var query = await _repository.GetQuery().Where(x => x.UserId == id).ToListAsync();
           return _mapper.Map<List<BeneficiareViewModel>>(query);
       }
       
    }
}
