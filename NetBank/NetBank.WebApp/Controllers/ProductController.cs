using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Product;
using NetBank.Core.Domain.Enums;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Dtos.Account;

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
            //trae la info del user logeado
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");

            //trae los productos del user logeado
            var allProducts = await _productService.GetProductsByUserId(user.Id); 

            return View(allProducts);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Product(string id,string? message=null, string? messageType =null)
        {
            ViewBag.Message=message;
            ViewBag.MessageType = messageType;    
            return View(await _productService.GetProductsByUserId(id));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create(string userId)
        {
            return View(new SaveProductViewModel{UserId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveProductViewModel productVm)
        {
            if(productVm.ProductType == 0)
            {
                return View(productVm);
            }

            var response = await _productService.CreateAsync(productVm);

            return RedirectToRoute(new { controller = "Product", action = "Product",id=response.UserId });
        }


        public async Task<IActionResult> Delete(int id,ProductType productType,string userId)
        {
           var response = await _productService.DeleteProduct(id,productType);

            if(response)
            {
                return RedirectToRoute(new { controller = "Product", action = "Product", id=userId , message = "Eliminacion satisfactoria", messageType = "alert-success" });
            }
            else
            {
                return RedirectToRoute(new { controller = "Product", action = "Product", id = userId, message="No puedes eliminar este producto ya que el dueño le debe.", messageType="alert-danger" });

            }

        }
    }
}
