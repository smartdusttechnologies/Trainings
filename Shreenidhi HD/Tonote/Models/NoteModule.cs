using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Tonote.Controllers;

namespace Tonote.Models
{
    public class NoteModule
    {
        // [Required(ErrorMessage ="")]
        public String NoteId { get; set; }
        [Required(ErrorMessage = "Note Name is required")]
        public String Name { get; set; }
        
    }
}
