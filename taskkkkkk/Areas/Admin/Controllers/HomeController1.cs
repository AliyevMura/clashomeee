using Microsoft.AspNetCore.Mvc;

namespace WebFrontToBack.Areas.Admin.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
