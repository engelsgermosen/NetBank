using NetBank.Core.Application.ViewModels.Product;
using NetBank.Core.Domain.Entities;
using NetBank.Core.Domain.Enums;

namespace NetBank.Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel, Product>
    {
        Task<SaveProductViewModel> GetProductMainByUserId(string id);

        Task<List<ProductViewModel>> GetProductsByUserId(string id);

<<<<<<< HEAD
        Task<bool> DeleteProduct(int id, ProductType type, string userId);

        Task<(List<ProductViewModel> tarjetas, List<ProductViewModel> cuentas)> GetAccountForCashAdvance();
=======
        Task<bool> DeleteProduct(int id, ProductType type);

        Task<ProductViewModel> GetProductByAccountNumber(int accountNumber);
>>>>>>> 51af201a87c22365f51fe662274b59bc37dbf245
    }
}
