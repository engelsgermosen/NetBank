using AutoMapper;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Role;

namespace NetBank.Core.Application.Services
{
    public class RolService : IRolService
    {
        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;

        public RolService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<List<RolViewModel>> GetAllrolesAsync()
        {
            var response = await _accountService.GetRolesAsync();
            return _mapper.Map<List<RolViewModel>>(response);
        }
    }
}
