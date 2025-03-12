using Microsoft.AspNetCore.Mvc;

namespace NetBank.WebApp.Controllers
{
    public class HomeController : Controller
    {


        public async Task<IActionResult> Index()
        {
            return View();
        }

<<<<<<< HEAD
        public async Task<IActionResult> Natanael()
=======
        public async Task<IActionResult> Engels()
>>>>>>> 8ac32d22a9d443f816ce1a2d95cc00c19b6b87d0
        {
            return View();
        }


    }
}
