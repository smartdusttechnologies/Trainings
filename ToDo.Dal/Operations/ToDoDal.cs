﻿using System;
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
        private string sqlConnectionString = @"Data Source=DESKTOP-O9DO8UK;Initial Catalog=ToDoList;User ID=sa;Password=Welcome@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public List<Entity.ToDo> get()
        {
            List<Entity.ToDo> toDo = new List<Entity.ToDo>();
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                toDo = connection.Query<Entity.ToDo>("Select ID, Task, DueDate , TStatus, AssignedTo, StoryPoints, Description from tblToDo ORDER BY DueDate").ToList();           
                connection.Close();
            }
            return toDo;
        }
        //This method Add the Data to database
        public int InsertEToDo(Entity.ToDo obj)
        {
            obj.TStatus = "new";
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Insert into tblToDo (Task,DueDate,TStatus,AssignedTo,StoryPoints, Description) values (@Task, @DueDate,@TStatus,@AssignedTo,@StoryPoints, @Description)", new { Task = obj.Task, DueDate = obj.DueDate , TStatus = obj.TStatus, AssignedTo=obj.AssignedTo, StoryPoints=obj.StoryPoints, Description=obj.Description,});
                connection.Close();
                return affectedRows;
            }
        }

        public object InsertEToDo()
        {
            throw new NotImplementedException();
        }
        //This method update the Task in database
        public int UpdateEToDo(Entity.ToDo obj)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Update tblToDo set TStatus= @TStatus Where ID = @ID", new { ID = obj.ID, TStatus = obj.TStatus });
                connection.Close();
                return affectedRows;
            }
        }
        //This method updated a record in database
        public int UpdateTaskToDo(Entity.ToDo obj)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Update tblToDo set Task=@Task,Description=@Description,AssignedTo=@AssignedTo, StoryPoints=@StoryPoints, DueDate=@DueDate Where ID = @ID", new { ID = obj.ID, Description=obj.Description, AssignedTo=obj.AssignedTo, StoryPoints=obj.StoryPoints, DueDate=obj.DueDate , Task=obj.Task });
                connection.Close();
                return affectedRows;
            }
        }
        //This method deletes a record from database    
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
        public object UpdateEToDo()
        {
            throw new NotImplementedException();
        }
    }
}
