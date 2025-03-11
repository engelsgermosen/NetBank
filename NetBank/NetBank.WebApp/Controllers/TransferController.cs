using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Transaction;

namespace NetBank.WebApp.Controllers
{
    [Authorize(Roles="Client")]
    public class TransferController : Controller
    {
        private readonly IProductService _productService;

        private readonly ITransactionService _transactionService;

        public TransferController(IProductService productService, ITransactionService transactionService)
        {
            _productService = productService;
            _transactionService = transactionService;
        }
        public async Task<IActionResult> Index(string? message= null, string? messageType=null)
        {
            var account = await _productService.GetAccountForCashAdvance();
            ViewBag.Accounts = account.cuentas;
            ViewBag.Message = message;
            ViewBag.MessageType = messageType;
            return View(new SaveTransactionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(SaveTransactionViewModel vm)
        {

            var account = await _productService.GetAccountForCashAdvance();
            ViewBag.Accounts = account.cuentas;

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

           var response = await _transactionService.TransferCash(vm);

            if(response.HasError)
            {
                vm.Error = response.Error; 
                vm.HasError = response.HasError;
                return View(response);
            }

            return RedirectToRoute(new {controller="Transfer",action="Index",message ="Tranferencia hecha satisfactoriamente", messageType="alert alert-success" });
        }

    }
}
