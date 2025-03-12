using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Beneficiare;
using Microsoft.AspNetCore.Authorization;

namespace NetBank.WebApp.Controllers
{
    [Authorize(Roles="Client")]
    public class BeneficiarieController : Controller
    {
        readonly IBeneficiareService _beneficiareService;
        readonly IProductService _productService;
        readonly IUserService _userService;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly AuthenticationResponse userInSession;

        public BeneficiarieController(IBeneficiareService beneficiareService, IProductService productService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _beneficiareService = beneficiareService;
            _productService = productService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            userInSession = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> Index(string? message =null, bool? HasError=false)
        {
            //var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            //if (user == null)
            //{
            //    return RedirectToAction("Index", "User");
            //}

            ViewBag.Message = message;
            ViewBag.HasError = HasError;

            var listaBeneficiarios = await _beneficiareService.GetBeneficiariosByUserId(userInSession.Id)
                                      ?? new List<BeneficiareViewModel>();

            if (listaBeneficiarios == null)
            {
                TempData["ErrorBeneficiario"] = "No hay beneficiarios disponibles";
            }

            var viewModel = new BeneficiariosCompositeViewModel
            {
                Beneficiarios = listaBeneficiarios,
                NewBeneficiarie = new SaveBeneficiareViewModel
                {
                    Name = string.Empty,
                    LastName = string.Empty,
                    AccountNumber = 0
                }
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task <IActionResult> Create([Bind(Prefix = "NewBeneficiarie")] BeneficiareViewModel newBeneficiarie)
        {

            if(!ModelState.IsValid)
            {
                return RedirectToRoute(new { controller = "Beneficiarie", action = "Index", message = "El numero de cuenta digitado no es valido, el formato correcto es" +
                    "(780xxxxxx)", HasError = true });
            }
            
            var productCuenta = await _productService.GetProductByAccountNumber(newBeneficiarie.AccountNumber);

            //y traeme al usuario relacionado con la cuenta
            var userBeneficiario = await _userService.GetByIdViewModel(productCuenta.UserId);


            if(productCuenta != null)
            {
                var saveViewModel = new SaveBeneficiareViewModel
                {
                    AccountNumber = newBeneficiarie.AccountNumber,
                    UserId = userInSession.Id, //id del usuario en seccion
                    Name = userBeneficiario.Name,
                    LastName = userBeneficiario.LastName
                };
               _beneficiareService.CreateAsync(saveViewModel);
                return RedirectToAction("Index");
            }
            
             TempData["ErrorBeneficiario"] = "El numero de cuenta insertado no existe";
             return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BeneficiareViewModel beneficiare)
        {
            await _beneficiareService.DeleteAsync(beneficiare.Id);
            return RedirectToAction("Index");
        }
    }
}
