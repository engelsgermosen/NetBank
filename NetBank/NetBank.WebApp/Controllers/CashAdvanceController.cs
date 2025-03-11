using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Transaction;

namespace NetBank.WebApp.Controllers
{
    [Authorize(Roles="Client")]
    public class CashAdvanceController : Controller
    {
        private readonly IProductService _productService;

        private readonly ITransactionService _transactionService;

        public CashAdvanceController(IProductService productService, ITransactionService transactionService)
        {
            _productService = productService;
            _transactionService = transactionService;
        }
        public async Task<IActionResult> Index()
        {
            var account = await _productService.GetAccountForCashAdvance();
            ViewBag.Cards = account.tarjetas;
            ViewBag.Accounts = account.cuentas;
            return View(new SaveTransactionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(SaveTransactionViewModel vm)
        {

            var account = await _productService.GetAccountForCashAdvance();
            ViewBag.Cards = account.tarjetas;
            ViewBag.Accounts = account.cuentas;

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

           var response = await _transactionService.CashAdvance(vm);

            if(response.HasError)
            {
                vm.Error = response.Error; 
                vm.HasError = response.HasError;
                return View(response);
            }

            return RedirectToAction("Index");
        }

    }
}
