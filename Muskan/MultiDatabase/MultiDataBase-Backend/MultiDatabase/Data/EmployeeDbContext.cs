using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using MultiDatabase.Models.Entities;

namespace TestProject.DbContexts
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
    }

    //public class Employee
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string HomeAddress { get; set; }
    //    public string Designation { get; set; }
    //    [NotMapped]
    //    public IFormFile File { get; set; }
    //    public byte[]? FileData { get; set; }
    //    public string? FileName { get; set; }
    //    public DateTime DateOfJoin { get; set; } = DateTime.Now;
    //    public string EmployeeSurname { get; set; }
    //}
}
