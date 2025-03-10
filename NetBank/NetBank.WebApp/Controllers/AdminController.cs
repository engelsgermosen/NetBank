using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.WebApp.Middlewares;

namespace NetBank.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ITransactionService _transactionService;

        private readonly IUserService _userService;

        public AdminController(ITransactionService transactionService, IUserService userService)
        {
            _transactionService = transactionService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Transacciones = await _transactionService.GetTransaccionCount();
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
