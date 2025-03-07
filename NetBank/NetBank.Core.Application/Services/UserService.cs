using AutoMapper;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.User;

namespace NetBank.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;

        public UserService(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel request)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(request);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);
            return userResponse;
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm)
        {
            RegisterRequest userCreate = _mapper.Map<RegisterRequest>(vm);
            RegisterResponse userResponse = await _accountService.RegisterAsync(userCreate);
            return userResponse;
        }

        public async Task LogOutAsync()
        {
            await _accountService.SignOutAsync();
        }
    }
}
