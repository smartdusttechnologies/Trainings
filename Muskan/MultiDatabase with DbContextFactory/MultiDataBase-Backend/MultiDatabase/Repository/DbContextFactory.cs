using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MultiDatabase.Data;
using MultiDatabase.Repository.Interface;

namespace MultiDatabase.Repository
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public DbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ApplicationDbContext CreateApplicationDbContext()
        {
            return CreateDbContext<ApplicationDbContext>("ApplicationDbContext");
        }

        public Application2DbContext CreateApplication2DbContext()
        {
            return CreateDbContext<Application2DbContext>("Application2DbContext");
        }

        private TContext CreateDbContext<TContext>(string dbContextKey) where TContext : DbContext
        {
            var dbType = _configuration[$"DbContextSettings:{dbContextKey}:Type"];
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            switch (dbType)
            {
                case "InMemory":
                    optionsBuilder.UseInMemoryDatabase(dbContextKey == "ApplicationDbContext" ? "InMemoryDb" : "InMemoryDb2");
                    break;
                case "SqlServer":
                    optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServerUserPortal"));
                    break;
                case "MySql":
                    optionsBuilder.UseMySql(_configuration.GetConnectionString("EmployeePortal"),
                        new MySqlServerVersion(new Version(8, 0, 21)));
                    break;
                default:
                    throw new NotSupportedException($"Database type '{dbType}' is not supported.");
            }

            return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options);
        }
    }
}
