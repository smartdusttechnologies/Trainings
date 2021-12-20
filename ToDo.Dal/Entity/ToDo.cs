using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Dal.Entity
{
    public class ToDo
    {
        public int ID { get; set; }
        public string Task { get; set; }
        public DateTime DueDate { get; set; }

        public string TStatus { get; set; }

    }
}
