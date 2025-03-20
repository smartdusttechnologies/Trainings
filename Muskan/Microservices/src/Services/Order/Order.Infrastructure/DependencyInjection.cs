using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;
using Ordering.Infrastructure.Data.Interceptors;
namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServerDb");
         service.AddScoped<ISaveChangesInterceptor, AuditableInterceptor>();
         service.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptors>();
            service.AddDbContext<ApplicationDbContext>((sp , options) =>
            {
                options.AddInterceptors(sp.GetService<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
            });
            service.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            return service;
        }
    }
}
