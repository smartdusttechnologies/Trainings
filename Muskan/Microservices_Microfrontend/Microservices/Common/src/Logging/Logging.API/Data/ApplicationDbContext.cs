using Logging.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Logging.API.Data
{
     public class ApplicationDbContext : DbContext
     {
          public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
          {
          }
          public DbSet<LogEntry> LogEntries { get; set; }
          public DbSet<Log> Logs { get; set; }

     }
}
