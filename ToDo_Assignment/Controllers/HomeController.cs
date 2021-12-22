using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDo.Dal.Operations;
using ToDo_Assignment.Models;
using ToDo.Dal.Entity;
using System.Globalization;

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
                task.TStatus = item.TStatus;
                toDos.Add(task);
               
            }
            return View(toDos);
        }
        [HttpPost]
        public ActionResult InsertEToDo(string task ,DateTime dueDate, string tStatus)
        {
           
            var toDoDal = new ToDo.Dal.Entity.ToDo();
            toDoDal.Task = task;
            toDoDal.DueDate = dueDate;
            
            
            new ToDoDal().InsertEToDo(toDoDal);
            return RedirectToAction("index");
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
