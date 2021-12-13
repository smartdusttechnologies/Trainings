using System;
using System.ComponentModel.DataAnnotations;
using Dapper;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Tonote.Models
{
    public class NoteModule
    {
        [Key]
        // [Required(ErrorMessage ="")]
        public String ID { get; set; }
        [Required(ErrorMessage = "Note Name is required")]
        public String Name { get; set; }
        
    }
    public static class Notedb
    {
        private static string connectionString = "Data Source=DESKTOP-4UBR12G;User ID=sa;Password=admin;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static string ConnectionString { get => connectionString; set => connectionString = value; }

        public static void ExecuteWithoutReturn(string procedureName, DynamicParameters param)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
