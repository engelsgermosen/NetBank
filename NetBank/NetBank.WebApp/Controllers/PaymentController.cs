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

        private readonly IBeneficiareService _beneficiareService;

        private readonly AuthenticationResponse userInSession;


        public PaymentController(IProductService productService,IBeneficiareService beneficiareService,IAccountService accountService, IPaymentService paymentService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
            _paymentService = paymentService;
            _accountService = accountService;
            _beneficiareService = beneficiareService;
            userInSession = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }
        public async Task<IActionResult> Express()
        {
            return View(new SavePaymentViewModel
            {
                PaymentType = PaymentType.PaymentExpress,
                Accounts = await _productService.GetCuentasAhorrosByUserId(userInSession.Id)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Express(SavePaymentViewModel payment)
        {

            if (!ModelState.IsValid)
            {
                return View(new SavePaymentViewModel
                {
                    PaymentType = payment.PaymentType,
                    Accounts = await _productService.GetCuentasAhorrosByUserId(userInSession.Id)
                });
            }

            var response = await _paymentService.PaymentExpressAndBeneficiarie(payment);

            if (response.HasError)
            {
                if(payment.PaymentType == PaymentType.PaymentExpress)
                {
                    return View(new SavePaymentViewModel
                    {
                        PaymentType = payment.PaymentType,
                        Amonut = payment.Amonut,
                        DestinationAccountNumber = payment.DestinationAccountNumber,
                        OriginAccountNumber = payment.OriginAccountNumber,
                        Accounts = await _productService.GetCuentasAhorrosByUserId(userInSession.Id),
                        HasError = response.HasError,
                        Error = response.Error
                    });
                }
                else
                {
                    return RedirectToRoute(new {controller="Payment",action= "Beneficiarie",HasError = true,Error=response.Error });
                }
            }
            else
            {
                var confirmData = new ConfirmExpressAngBeneficiariePaymentViewModel
                {
                    PagoConfirmacion = payment,
                    Usuario = await _accountService.GetUserByAccountNumber((payment.DestinationAccountNumber))
                };

                TempData["PagoConfirmacion"] = JsonConvert.SerializeObject(confirmData);

                return RedirectToAction("ExpressAndBeneficiarieConfirm");
            }
        }

        public IActionResult ExpressAndBeneficiarieConfirm()
        {
            var jsonPago = TempData.Peek("PagoConfirmacion") as string;

                if(!string.IsNullOrEmpty(jsonPago))
                {
                    var pago = JsonConvert.DeserializeObject<ConfirmExpressAngBeneficiariePaymentViewModel>(jsonPago);
                    return View(pago);
                }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ExpressAndBeneficiarieConfirm([Bind(Prefix = "PagoConfirmacion")] SavePaymentViewModel payment)
        {
            try
            {
                payment.UserId = userInSession.Id;
                await _paymentService.ConfirmPaymentExpressAndBeneficiarie(payment);
                return RedirectToRoute(new { controller = "Product", action = "Index" });
            }
            catch (Exception)
            {

                return View();
            } 
        }

        //Pago a beneficiarios

        public async Task<IActionResult> Beneficiarie(bool HasError = false,string? Error = null)
        {
            ViewBag.Beneficiarios = await _beneficiareService.GetBeneficiariosByUserId(userInSession.Id);
            return View(new SavePaymentViewModel
            {
                HasError = HasError,
                Error = Error,
                PaymentType = PaymentType.PaymentBeneficiarie,
                Accounts = await _productService.GetCuentasAhorrosByUserId(userInSession.Id)
            });
        }
    }
}
