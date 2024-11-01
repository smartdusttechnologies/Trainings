using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MultiDatabase.Data;
using MultiDatabase.Repository.Interface;

namespace MultiDatabase.Repository
{
    public class DbContextFactory :IDbContextFactory
    {
        public ApplicationDbContext GetDbContext(string connectionstring, string provider)
        {
            var optionbuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            switch (provider.ToLower())
            {
                case "sqlserver":
                    optionbuilder.UseSqlServer(connectionstring);
                    break;
                case "inmemory":
                    optionbuilder.UseInMemoryDatabase("InMemoryDb");
                    break;
                default:
                    throw new NotSupportedException($"The provider {provider} is not supported.");
            }

            return new ApplicationDbContext(optionbuilder.Options);
        }
    }
}
