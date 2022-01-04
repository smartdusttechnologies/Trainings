using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ToDo_Assignment.Models
{
    public class ToDoModel
    {
            public int ID { get; set; }
            public string Task { get; set; }
            public DateTime DueDate { get; set; }
            public string TStatus { get; set; }
        
            public string AssignedTo { get; set; }
            [Required]
            public string Description { get; set; }


        }
   }
