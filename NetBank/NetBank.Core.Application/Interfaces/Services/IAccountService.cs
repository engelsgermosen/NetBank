using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Dtos.Rol;

namespace NetBank.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task SignOutAsync();

        Task<List<RolList>> GetRolesAsync();

        Task<List<AuthenticationResponse>> GetAllUsersAsync();

        Task<UserInactivate> UserInactivateAsync(string id);
        Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);

        Task<AuthenticationResponse> GetUserByIdAsync(string id);

        Task<AuthenticationResponse> GetUserByAccountNumber(int accountNumber);

    }
}