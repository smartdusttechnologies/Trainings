using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;

namespace ToDoo.Dal.Entity
{
    class ToDo
    {
        public int id { get; set; }
        public string Task { get; set; }
        public DateTime DueDate {get;set;}


    }
}
