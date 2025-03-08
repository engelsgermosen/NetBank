using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Interfaces.Services;

namespace NetBank.WebApp.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            //obtener info de usuario logueado para solo traer los productos de ese usuario
            //mientras tanto traelos todos
            var allProducts = await _productService.GetAllAsync(); 

            return View(allProducts);
        }
    }
}
