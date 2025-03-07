using NetBank.Core.Application.ViewModels.Role;

namespace NetBank.Core.Application.Interfaces.Services
{
    public interface IRolService
    {
        Task<List<RolViewModel>> GetAllrolesAsync();
    }
}
