using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tonote.Models;

namespace Tonote.Controllers
{
    public class NoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Note()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Note(NoteModule obj)
        {
            ViewBag.Msg = "The Note Name" + obj.Name + "added";
            return View();
        }
    }
}
