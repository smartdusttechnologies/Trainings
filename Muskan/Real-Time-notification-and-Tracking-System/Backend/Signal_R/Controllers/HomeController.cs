using Microsoft.AspNetCore.Mvc;

namespace Signal_R.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
