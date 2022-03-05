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
        private string sqlConnectionString = @"Data Source=DESKTOP-O9DO8UK;Initial Catalog=ToDoList;User ID=sa;Password=Welcome@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public List<Entity.ToDoUser> UserGet()
        {
            List<Entity.ToDoUser> toDoUser = new List<Entity.ToDoUser>();
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                toDoUser = connection.Query<Entity.ToDoUser>("Select Id, Name, TeamName from tblToDoUser").ToList();
                connection.Close();
            }
            return toDoUser;
        }


    }
}
