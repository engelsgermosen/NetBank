using NetBank.Core.Application.ViewModels.Beneficiare;
using NetBank.Core.Domain.Entities;

namespace NetBank.Core.Application.Interfaces.Services
{
    public interface IBeneficiareService : IGenericService<SaveBeneficiareViewModel, BeneficiareViewModel, Beneficiarie>
    {
        Task<List<BeneficiareViewModel>> GetBeneficiariosByUserId(string id);
    }
}
