using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        private readonly IUserService _userService;

        public BeneficiareService(IBeneficiareRepository repository, IMapper mapper, IUserService userService) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
        }
        
       public async Task<List<BeneficiareViewModel>> GetBeneficiariosByUserId(string id)
       {
           var query = await _repository.GetQuery().Where(x => x.UserId == id).ToListAsync();
           return _mapper.Map<List<BeneficiareViewModel>>(query);
       }

        public async Task<bool> AlreadyHave(int accountNumber,string userId)
        {
            var query = await _repository.GetQuery().Where(x => x.UserId == userId && x.AccountNumber == accountNumber).FirstOrDefaultAsync();
            return query != null;
        }

        
    }
}
