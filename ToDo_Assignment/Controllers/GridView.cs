using Microsoft.AspNetCore.Mvc;

namespace ToDo_Assignment.Controllers
{
    public class GridView : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
