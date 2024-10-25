using Microsoft.EntityFrameworkCore;
using MultiDatabase.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiDatabase.Data
{
    public class UserTestDbContext : DbContext
    {
        public UserTestDbContext(DbContextOptions<UserTestDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
    //public class User
    //{
    //    public int Id { get; set; }
    //    public string? Name { get; set; }
    //    public string? Address { get; set; }
    //    public string? Phone { get; set; }
    //    public string? Email { get; set; }
    //    [NotMapped]
    //    public IFormFile File { get; set; }
    //    public byte[]? FileData { get; set; }
    //    public string? FileName { get; set; }
    //}
}
