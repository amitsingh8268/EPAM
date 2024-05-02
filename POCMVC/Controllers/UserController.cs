using Microsoft.AspNetCore.Mvc;

namespace POCMVC.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
