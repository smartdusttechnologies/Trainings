using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDo.Dal.Operations;
using ToDo_Assignment.Models;

namespace ToDo_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ToDoDal listofnote = new ToDoDal();
            var tasks=  listofnote.get();
            List<ToDoModel> toDos = new List<ToDoModel>();
            foreach (var item in tasks)
            {
                var task = new ToDoModel();
                task.DueDate = item.DueDate;
                task.Task = item.Task;
                toDos.Add(task);
               
            }
            return View(toDos);
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
