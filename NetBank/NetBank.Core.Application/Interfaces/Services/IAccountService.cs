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
    }
}