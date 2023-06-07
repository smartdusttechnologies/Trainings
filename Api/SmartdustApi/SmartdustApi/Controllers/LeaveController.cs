using Microsoft.AspNetCore.Mvc;

namespace SmartdustApi.Controllers
{
    public class LeaveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
