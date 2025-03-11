using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.ViewModels.User;

namespace NetBank.Core.Application.Interfaces.Services
{
    public interface IUserService 
    {
        Task<AuthenticationResponse> LoginAsync(LoginViewModel request);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm);
        Task LogOutAsync();

        Task<List<UserViewModel>> GetAllUsersViewModel();
        Task<UserInactiveViewModel> UserInactiveViewModelAsync(string id);

        Task<UpdateUserResponse> UpdateUserViewModel(UpdateUserViewModel vm);

        Task<UpdateUserViewModel> GetByIdViewModel(string id);

    }
}
