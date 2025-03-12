using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetBank.WebApp.Controllers
{
    [Authorize(Roles="Client")]
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
