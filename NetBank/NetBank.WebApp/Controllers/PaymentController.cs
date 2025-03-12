using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Payment;
using NetBank.Core.Domain.Enums;
using Newtonsoft.Json;

namespace NetBank.WebApp.Controllers
{
    [Authorize(Roles="Client")]
    public class PaymentController : Controller
    {
        private readonly IProductService _productService;

        private readonly IPaymentService _paymentService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IAccountService _accountService;

        private readonly AuthenticationResponse userInSession;


        public PaymentController(IProductService productService,IAccountService accountService, IPaymentService paymentService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
            _paymentService = paymentService;
            _accountService = accountService;
            userInSession = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }
        public async Task<IActionResult> Express()
        {
            return View(new SavePaymentViewModel
            {
                Accounts = await _productService.GetProductsByUserId(userInSession.Id)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Express(SavePaymentViewModel payment)
        {

            if(!ModelState.IsValid)
            {
                return View(new SavePaymentViewModel
                {
                    Accounts = await _productService.GetProductsByUserId(userInSession.Id)
                });
            }

            var response = await _paymentService.PaymentExpress(payment);

            if(response.HasError)
            {
                return View(new SavePaymentViewModel
                {
                    Amonut = payment.Amonut,
                    DestinationAccountNumber = payment.DestinationAccountNumber,
                    OriginAccountNumber = payment.OriginAccountNumber,
                    Accounts = await _productService.GetProductsByUserId(userInSession.Id),
                    HasError = response.HasError,
                   Error = response.Error
                });
            }
            else
            {
                var confirmData = new ConfirmExpressPaymentViewModel
                {
                    PagoConfirmacion = payment,
                    Usuario = await _accountService.GetUserByAccountNumber((payment.DestinationAccountNumber))
                };

                TempData["PagoConfirmacion"] = JsonConvert.SerializeObject(confirmData);

                return RedirectToAction("ExpressConfirm");
            }
        }

        public IActionResult ExpressConfirm()
        {
            var pago = JsonConvert.DeserializeObject<ConfirmExpressPaymentViewModel>(TempData["PagoConfirmacion"].ToString());
            return View(pago);
        }

        [HttpPost]
        public async Task<IActionResult> ExpressConfirm([Bind(Prefix = "PagoConfirmacion")] SavePaymentViewModel payment)
        {
            try
            {
                payment.UserId = userInSession.Id;
                payment.PaymentType = PaymentType.PaymentExpress;
                await _paymentService.ConfirmPaymentExpress(payment);
                return RedirectToRoute(new { controller = "Product", action = "Index" });
            }
            catch (Exception)
            {

                return View();
            } 
        }
    }
}
