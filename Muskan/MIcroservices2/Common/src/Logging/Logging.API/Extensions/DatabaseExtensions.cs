using Logging.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Logging.API.Extensions
{

     public static class DatabaseExtensions
     {
          public static async Task<IApplicationBuilder> UseMigration(this IApplicationBuilder app)
          {
               using var scope = app.ApplicationServices.CreateScope();
               try
               {
                    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                    if (pendingMigrations.Any())
                    {
                         Console.WriteLine("Data base migration started");
                         await context.Database.MigrateAsync();
                         Console.WriteLine("Data base migration ended");
                    }
               }
               catch (Exception ex)
               {
                    Console.WriteLine(ex.InnerException);

               }
               return app;
          }
     }
}
