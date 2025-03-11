using AutoMapper;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Product;
using NetBank.Core.Application.ViewModels.User;
using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        public UserService(IAccountService accountService,IProductService productService, IMapper mapper)
        {
            _accountService = accountService;
            _productService = productService; 
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

            if(vm.InitialAmount != null)
            {
                await _productService.CreateAsync(new SaveProductViewModel
                {
                    Balance = vm.InitialAmount,
                    ProductType = ProductType.CuentaAhorro,
                    IsMain = true,
                    UserId = userResponse.Id,
                });
            }
            return userResponse;
        }

        public async Task LogOutAsync()
        {
            await _accountService.SignOutAsync();
        }

        public async Task<List<UserViewModel>> GetAllUsersViewModel()
        {
            var response = await _accountService.GetAllUsersAsync();
            return _mapper.Map<List<UserViewModel>>(response);
        }

        public async Task<UserInactiveViewModel> UserInactiveViewModelAsync(string id)
        {
            var response = await _accountService.UserInactivateAsync(id);
            return _mapper.Map<UserInactiveViewModel>(response);
        }

        public async Task<UpdateUserResponse> UpdateUserViewModel(UpdateUserViewModel vm)
        {
            UpdateUserRequest request = _mapper.Map<UpdateUserRequest>(vm);
            UpdateUserResponse response = await _accountService.UpdateUserAsync(request);

            if(vm.AditionalAmount != null && !response.HasError)
            {
                var cuentaMain = await _productService.GetProductMainByUserId(vm.Id);
                cuentaMain.Balance += vm.AditionalAmount;
                await _productService.UpdateAsync(cuentaMain, cuentaMain.AccountNumber);
            }

            return response;
        }

        public async Task<UpdateUserViewModel> GetByIdViewModel(string id)
        {
            var response = await _accountService.GetUserByIdAsync(id);
            UpdateUserViewModel userVm = new()
            { 
                Id = id,
                Name= response.Name,
                LastName= response.LastName,
                Email= response.Email,
                IsAdmin = response.Rol == 1 ? true : false,
                Identification = response.Identification,
                UserName = response.UserName,     
            };
            return userVm;
        }
    }
}
