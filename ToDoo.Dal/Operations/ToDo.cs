using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ToDoo.Dal.Entity;
using Dapper;


namespace ToDoo.Dal.Operations
{
    class ToDo
    {

        private string sqlConnectionString = @"Data Source=DESKTOP-1GFB995;Initial";

        private List<Entity.ToDo> get()
        {
            List<ToDo> ToDo = new List<ToDo>();
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                ToDo = connection.Query<ToDo>("Select Id, Task, DueDate from ToDo.tbl").ToList();
                connection.Close();
            }
            return ToDo;
        }




        private int InsertEToDo(Entity.ToDo obj)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Insert into ToDo (ID, Task) values (@ID, @Task)", new { ID = obj.id, Task = obj.Task });
                connection.Close();
                return affectedRows;
            }
        }

        private int UpdateEToDo(Entity.ToDo obj)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Update ToDo set Name = @Id, Task = @Id Where Id = @Id", new { Id = obj.id, Task = obj.Task });
                connection.Close();
                return affectedRows;
            }
        }

        //This method deletes a student record from database    
        private int DeleteEToDo(Entity.ToDo obj)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Delete from ToDO Where Id = @Id", new { Id = obj.id });
                connection.Close();
                return affectedRows;
            }
        }



    }
}