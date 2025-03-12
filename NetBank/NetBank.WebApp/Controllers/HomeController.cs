using Microsoft.AspNetCore.Mvc;

namespace NetBank.WebApp.Controllers
{
    public class HomeController : Controller
    {


        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Natanael()
        {
            return View();
        }


    }
}
