using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Dal.Entity
{
    public class ToDo
    {
        public int id { get; set; }
        public string Task { get; set; }
        public DateTime DueDate { get; set; }

    }
}
