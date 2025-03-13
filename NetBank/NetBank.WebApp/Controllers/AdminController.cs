using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Interfaces.Services;

namespace NetBank.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ITransactionService _transactionService;

        private readonly IUserService _userService;

        private readonly IProductService _productService;

        private readonly IPaymentService _paymentService;

        public AdminController(ITransactionService transactionService, IUserService userService, IProductService productService, IPaymentService paymentService)
        {
            _transactionService = transactionService;
            _userService = userService;
            _productService = productService;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Transacciones = await _transactionService.GetTransaccionCount();
            ViewBag.Clients = await _userService.GetAllUsersViewModel();
            ViewBag.Products = await _productService.GetAllAsync();
            ViewBag.Payments = await _paymentService.GetPaymentCount();
            return View();
        }

        public async Task<IActionResult> Main(string? message=null,bool hasError = false)
        {
            ViewBag.Error = message;
            ViewBag.HasError = hasError;
            return View(await _userService.GetAllUsersViewModel());
        }

        public async Task<IActionResult> Inactivate(string id)
        {

            var response = await _userService.UserInactiveViewModelAsync(id);

            return RedirectToRoute(new { controller = "Admin", action = "Main", message = response.Error, hasError = response.HasError });
        }
    }
}
