using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ToDo.Dal.Operations
{
     public class ToDoDal
    {
        private string sqlConnectionString = @"Data Source=DESKTOP-4UBR12G;Initial Catalog=ToDoList;User ID=sa;Password=admin;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<Entity.ToDo> get()
        {
            List<Entity.ToDo> toDo = new List<Entity.ToDo>();
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                toDo = connection.Query<Entity.ToDo>("Select ID, Task, DueDate , TStatus from tblToDo").ToList();
                connection.Close();
            }
            return toDo;
        }




        public int InsertEToDo(Entity.ToDo obj)
        {
            obj.TStatus = "new";
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Insert into tblToDo (Task,DueDate,TStatus) values (@Task, @DueDate,@TStatus)", new { ID = obj.Task, Task = obj.DueDate.Date , TStatus = obj.TStatus });
                connection.Close();
                return affectedRows;
            }
        }

        public object InsertEToDo()
        {
            throw new NotImplementedException();
        }

        public int UpdateEToDo(Entity.ToDo obj)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Update ToDo set ID= @Id, Task = @Id Where Id = @Id", new { Id = obj.ID, Task = obj.Task });
                connection.Close();
                return affectedRows;
            }
        }

        //This method deletes a student record from database    
        public int DeleteEToDo(Entity.ToDo obj)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Delete from tblToDO Where ID = @Id", new { Id = obj.ID });
                connection.Close();
                return affectedRows;
            }
        }



    }
}
