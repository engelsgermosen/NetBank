using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBank.Core.Application.Dtos.Account;
using NetBank.Core.Application.Enums;
using NetBank.Core.Application.Helpers;
using NetBank.Core.Application.Interfaces.Services;
using NetBank.Core.Application.ViewModels.User;
using NetBank.WebApp.Middlewares;

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

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
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

                switch(userVm.Rol)
                {
                    case (int)Roles.Admin:
                        return RedirectToRoute(new { controller = "Admin", action = "Index" });

                    case (int)Roles.Client:
                        return RedirectToRoute(new { controller = "Home", action = "Index" });
                    default:
                        return null;

                }

            }
            else
            {
                loginVm.HasError = userVm.HasError;
                loginVm.Error = userVm.Error;
                return View(loginVm);
            }
           
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register()
        {
            ViewBag.Roles = await _rolService.GetAllrolesAsync();
            return View(new SaveUserViewModel());
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]

        public async Task<IActionResult> Register(SaveUserViewModel userVm)
        {
            ViewBag.Roles = await _rolService.GetAllrolesAsync();

            if (!ModelState.IsValid)
            {
                return View(userVm);
            }

            RegisterResponse response  = await _userService.RegisterAsync(userVm);

            if (response != null && !response.HasError)
            {
                return RedirectToRoute(new { controller = "Admin", action = "Main" });
            }
            else
            {
                userVm.HasError = response.HasError;
                userVm.Error = response.Error;
                return View(userVm);
            }

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            return View(await _userService.GetByIdViewModel(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserViewModel userVm)
        {
            ModelState.Remove(nameof(userVm.Password));
            ModelState.Remove(nameof(userVm.ConfirmPassword));

            if (!ModelState.IsValid)
            {
                return View(userVm);
            }

            UpdateUserResponse response = await _userService.UpdateUserViewModel(userVm);

            if (response != null && !response.HasError)
            {
                return RedirectToRoute(new { controller = "Admin", action = "Main" });
            }
            else
            {
                userVm.HasError = response.HasError;
                userVm.Error = response.Error;
                return View(userVm);
            }
        }


        public IActionResult AccesDenied()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _userService.LogOutAsync();
            _httpContextAccessor.HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        
    }
}
