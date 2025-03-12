using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Product;
using NetBank.Core.Domain.Enums;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Dtos.Account;

namespace NetBank.WebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AuthenticationResponse userInSession;

        public ProductController(IProductService productService, IHttpContextAccessor httpContextAccessor)
        {
            _productService = productService;
            _httpContextAccessor = httpContextAccessor;
            userInSession = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Index()
        {
            var allProducts = await _productService.GetProductsByUserId(userInSession.Id); 

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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id,ProductType productType,string userId)
        {
           var response = await _productService.DeleteProduct(id,productType,userId);

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
