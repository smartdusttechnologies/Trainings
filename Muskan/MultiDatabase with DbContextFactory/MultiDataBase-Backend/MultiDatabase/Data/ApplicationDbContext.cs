using Microsoft.EntityFrameworkCore;
using MultiDatabase.Models.Entities;


namespace MultiDatabase.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<Employee> Employees{ get; set; }
    }

}

   

