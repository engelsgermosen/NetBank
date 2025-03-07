using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.User;

namespace NetBank.WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRolService _rolService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor, IRolService rolService)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _rolService = rolService;
        }
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]

        public async Task<IActionResult> Index(LoginViewModel loginVm)
        {

            if(!ModelState.IsValid)
            {
                return View(loginVm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(loginVm);

            if (userVm != null &&  !userVm.HasError)
            {
                _httpContextAccessor.HttpContext.Session.Set<AuthenticationResponse>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                loginVm.HasError = userVm.HasError;
                loginVm.Error = userVm.Error;
                return View(loginVm);
            }
           
        }

        public async Task<IActionResult> Register()
        {
            ViewBag.Roles = await _rolService.GetAllrolesAsync();
            return View(new SaveUserViewModel());
        }

        [HttpPost]

        public async Task<IActionResult> Register(SaveUserViewModel userVm)
        {

            if (!ModelState.IsValid)
            {
                return View(userVm);
            }

            RegisterResponse response  = await _userService.RegisterAsync(userVm);

            if (userVm != null && !userVm.HasError)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                userVm.HasError = response.HasError;
                userVm.Error = response.Error;
                return View(userVm);
            }

        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.LogOutAsync();
            _httpContextAccessor.HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
    }
}
