using Microsoft.AspNetCore.Mvc;

namespace BettingApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
