using BuildingBlock.Exceptions.Handler;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAPIService(this IServiceCollection service , IConfiguration configuration)
        {
            service.AddCarter();
            service.AddExceptionHandler<CustomExceptionHandler>();
            service.AddHealthChecks()
                 .AddSqlServer(configuration.GetConnectionString("SqlServerDb"));


            return service;
        }
        public static WebApplication UseAPIService (this WebApplication app)
        {
            app.MapCarter();
            app.UseExceptionHandler(options => { });
            app.UseHealthChecks("/healths",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            return app;
        }
    }
}
