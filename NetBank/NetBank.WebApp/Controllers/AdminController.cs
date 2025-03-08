using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Interfaces.Services;

namespace NetBank.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ITransactionService _transactionService;

        public AdminController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Transacciones = await _transactionService.GetTransaccionCount();
            return View();
        }
    }
}
