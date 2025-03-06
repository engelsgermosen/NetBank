
using Microsoft.AspNetCore.Mvc;


namespace NetBank.WebApp.Controllers
{
    public class HomeController : Controller
    {


        public HomeController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }


    }
}
