using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace UserDal.Operations
{
    public class UserDal
    {
        private string sqlConnectionString = @"Data Source=DESKTOP-IFTP34V;Initial Catalog=UserTable;User ID=sa;Password=Vishal@#&099;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<User> get()
        {
            List<User> user = new List<User>();
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                user = connection.Query<User>("Select ID,  UserName, TeamName from UserTable ").ToList();
                connection.Close();
            }
            return user;
        }



        //This method inserts a record in database
        public int InsertUser(User obj)
        {
            //obj.Team = "new";

            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Insert into UserTable (TeamName,UserName) values (@TeamName,@UserName)", new { TeamName=obj.TeamName, UserName=obj.UserName });
                connection.Close();
                return affectedRows;
            }
        }
        

        //This method updates a record in database  
        public int UpdateUser(User obj)
        {
            using (var connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Update UserTable set TeamName=@TeamName,UserName=@UserName Where ID = @ID", new { ID = obj.ID, UserName=obj.UserName,TeamName=obj.TeamName  });
                connection.Close();
                return affectedRows;
            }
        }


        //This method deletes a record from database    
        public int DeleteUser(User obj)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                var affectedRows = connection.Execute("Delete from UserTable Where ID = @Id", new { Id = obj.ID });
                connection.Close();
                return affectedRows;
            }
        }
    }
}

