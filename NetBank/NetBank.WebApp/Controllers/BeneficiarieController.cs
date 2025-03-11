using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.Beneficiare;

namespace NetBank.WebApp.Controllers
{
    public class BeneficiarieController : Controller
    {
        IBeneficiareService _beneficiareService;
        IProductService _productService;
        IUserService _userService;

        public BeneficiarieController(IBeneficiareService beneficiareService, IProductService productService, IUserService userService)
        {
            _beneficiareService = beneficiareService;
            _productService = productService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            if (user == null)
            {
                return RedirectToAction("Index", "User");
            }

            var listaBeneficiarios = await _beneficiareService.GetBeneficiariosByUserId(user.Id)
                                      ?? new List<BeneficiareViewModel>();

            if (listaBeneficiarios == null)
            {
                TempData["ErrorBeneficiario"] = "No hay beneficiarios disponibles";
            }

            var viewModel = new BeneficiariosCompositeViewModel
            {
                Beneficiarios = listaBeneficiarios,
                NewBeneficiarie = new BeneficiareViewModel
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
            var user = HttpContext.Session.Get<AuthenticationResponse>("user");
            
            //traeme la cuenta con este numero
            var productCuenta = await _productService.GetProductByAccountNumber(newBeneficiarie.AccountNumber);

            //y traeme al usuario relacionado con la cuenta
            var userBeneficiario = await _userService.GetByIdViewModel(productCuenta.UserId);


            if(productCuenta != null)
            {
                var saveViewModel = new SaveBeneficiareViewModel
                {
                    AccountNumber = newBeneficiarie.AccountNumber,
                    UserId = user.Id, //id del usuario en seccion
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
