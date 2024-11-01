using Microsoft.EntityFrameworkCore;
using MultiDatabase.Models.Entities;

namespace MultiDatabase.Data
{
    public class Application2DbContext : DbContext
    {
        public Application2DbContext(DbContextOptions<Application2DbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
