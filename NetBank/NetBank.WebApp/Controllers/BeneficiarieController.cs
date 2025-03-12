using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Beneficiare;
using Microsoft.AspNetCore.Authorization;
using NetBank.Core.Domain.Enums;

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
                    AccountNumber = 0
                }
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task <IActionResult> Create([Bind(Prefix = "NewBeneficiarie")] SaveBeneficiareViewModel newBeneficiarie)
        {

            if(!ModelState.IsValid)
            {
                TempData["ErrorBeneficiario"] = "El numero de cuenta digitado no es valido, el formato correcto es: (780xxxxxx)";
                return RedirectToAction("Index");
            }
            
            var productCuenta = await _productService.GetProductByAccountNumber(newBeneficiarie.AccountNumber);


            if(productCuenta != null )
            {
                var userBene = await _userService.GetByIdViewModel(productCuenta.UserId);

                if (await _beneficiareService.AlreadyHave(productCuenta.AccountNumber, userInSession.Id) == false)
                {
                    if (productCuenta.ProductType == ProductType.CuentaAhorro)
                    {
                        newBeneficiarie.UserId = userInSession.Id;
                        newBeneficiarie.Name = userBene.Name;
                        newBeneficiarie.LastName = userBene.LastName;
                        await _beneficiareService.CreateAsync(newBeneficiarie);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorBeneficiario"] = "El numero de cuenta insertado no es una cuenta de ahorro";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["ErrorBeneficiario"] = "Ya tienes esa cuenta agregada como beneficiario";
                    return RedirectToAction("Index");
                }
                
            }
            else
            {
                TempData["ErrorBeneficiario"] = "El numero de cuenta insertado no existe";
                return RedirectToAction("Index");
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BeneficiareViewModel beneficiare)
        {
            await _beneficiareService.DeleteAsync(beneficiare.Id);
            return RedirectToAction("Index");
        }
    }
}
