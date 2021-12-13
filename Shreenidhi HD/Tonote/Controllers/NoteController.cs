using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tonote.Models;
using Dapper;
using System.Data.SqlClient;

namespace Tonote.Controllers
{
    public class NoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Note()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Note(NoteModule Note)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID",Note.ID);
            param.Add("@Name",Note.Name);
            Notedb.ExecuteWithoutReturn("Note", param);
            return RedirectToAction("Index");
        }
    }
}
