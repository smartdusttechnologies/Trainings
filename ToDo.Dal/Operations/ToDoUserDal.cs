using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace ToDo.Dal.Operations
{
    public class ToDoUserDal
    {
        private string sqlConnectionString = @"Data Source=DEEPAK-MAMTA110\MSSQLSERVER01;Initial Catalog=ToDo;User ID=sa;Password=admin;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public List<Entity.ToDoUser> UserGet()
        {
            List<Entity.ToDoUser> toDoUser = new List<Entity.ToDoUser>();
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                toDoUser = connection.Query<Entity.ToDoUser>("Select Id, Name, TeamName from tblToDo").ToList();
                connection.Close();
            }
            return toDoUser;
        }


    }
}
