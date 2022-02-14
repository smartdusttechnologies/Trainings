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
            var tasks = listofnote.get();
            List<ToDoModel> toDos = new List<ToDoModel>();
            foreach (var item in tasks)
            {
                var task = new ToDoModel();
                task.DueDate = item.DueDate;
                task.Task = item.Task;
                task.ID = item.ID;
                task.TStatus = item.TStatus;
                task.AssignedTo = item.AssignedTo;
                task.StoryPoints = item.StoryPoints;
                task.Description = item.Description;
                toDos.Add(task);
            }
            return View(toDos);
        }
        [HttpPost]
        public IActionResult updateData(int id, string status)
        {
            string newStatus = "";
            if (status == "new") {
                newStatus = "InProgress";
            }
            else if (status == "InProgress") {
                newStatus = "Completed";
            }
            else if (status == "Completed") {
                newStatus = "new";
            }
            var toDoDal = new ToDo.Dal.Entity.ToDo();
            toDoDal.ID = id;
            toDoDal.TStatus = newStatus;
            new ToDoDal().UpdateEToDo(toDoDal);
            return RedirectToAction("index");
        }

        //For insertion
        [HttpPost]
        public ActionResult InsertEToDo(string task, DateTime dueDate, string tStatus, string assignedTo,int storyPoints, string description)
        {
            var toDoDal = new ToDo.Dal.Entity.ToDo();
            toDoDal.Task = task;
            toDoDal.DueDate = dueDate;
            toDoDal.AssignedTo = assignedTo;
            toDoDal.StoryPoints = storyPoints;
            toDoDal.Description = description;
            new ToDoDal().InsertEToDo(toDoDal);
            return RedirectToAction("index");
        }

        //For updating the data
        [HttpPost]
        public ActionResult UpdateToDo(int id, string task, DateTime dueDate, string assignedTo, int storyPoints, string description)
        {
            var toDoDal = new ToDo.Dal.Entity.ToDo();
            toDoDal.ID = id;
            toDoDal.Task = task;
            toDoDal.DueDate = dueDate;
            toDoDal.AssignedTo = assignedTo;
            toDoDal.StoryPoints = storyPoints;
            toDoDal.Description = description;
            new ToDoDal().UpdateTaskToDo(toDoDal);
            return RedirectToAction("index");
        }

        [HttpGet]
        public ActionResult DeleteEToDo(int id )
        {
            var toDoDal = new ToDo.Dal.Entity.ToDo();
            toDoDal.ID = id;
            new ToDoDal().DeleteEToDo(toDoDal);
            return RedirectToAction("Index");
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

        public IActionResult GridView()
        {
            ToDoDal listofnote = new ToDoDal();
            var tasks = listofnote.get();
            List<ToDoModel> toDos = new List<ToDoModel>();
            foreach (var item in tasks)
            {
                var task = new ToDoModel();
                task.DueDate = item.DueDate;
                task.Task = item.Task;
                task.ID = item.ID;
                task.TStatus = item.TStatus;
                task.AssignedTo = item.AssignedTo;
                task.StoryPoints = item.StoryPoints;
                task.Description = item.Description;
                toDos.Add(task);
            }
            return View(tasks);

        /// <summary>
        /// this controller used for showing search results for your query task
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTaskDetail(int detail)
        {

            var toDoDal = new ToDoDal();
            var tasks = toDoDal.get();
            ViewData["id"] = detail;
            var abc = tasks.Where(x => x.ID.Equals(detail)).FirstOrDefault();
            return Json(abc);
        }

        /// <summary>
        /// This controller used for Autocomplete feature that gives options of search results.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSearchResults(string term)
        {
            var toDoDal = new ToDoDal();
            var tasks = toDoDal.get();

            var searchResults = tasks.Where(x => x.Task.Contains(term)).Select(x => new
            {
                label = x.Task,
                val = x.ID
            }).ToList();
            return Json(searchResults);
        }
    }
}
