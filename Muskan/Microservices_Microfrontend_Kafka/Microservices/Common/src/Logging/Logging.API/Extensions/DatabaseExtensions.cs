using Logging.API.Data;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Logging.API.Extensions
{
     public static class DatabaseExtensions
     {
          private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

          public static async Task<IApplicationBuilder> UseMigration(this IApplicationBuilder app)
          {
               if (app == null)
               {
                    Logger.Error("Application builder is null in UseMigration.");
                    throw new ArgumentNullException(nameof(app));
               }

               using var scope = app.ApplicationServices.CreateScope();

               try
               {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                    if (pendingMigrations.Any())
                    {
                         Logger.Info("Database migration started.");
                         await context.Database.MigrateAsync();
                         Logger.Info("Database migration completed.");
                    }
                    else
                    {
                         Logger.Info("No pending migrations found.");
                    }
               }
               catch (DbUpdateException dbEx)
               {
                    Logger.Error(dbEx, "Database update exception during migration. Check DB connection or schema.");
                    LogInnerExceptions(dbEx);
               }
               catch (InvalidOperationException invalidOpEx)
               {
                    Logger.Error(invalidOpEx, "Invalid operation during database migration.");
                    LogInnerExceptions(invalidOpEx);
               }
               catch (Exception ex)
               {
                    Logger.Error(ex, "Unexpected error occurred during database migration.");
                    LogInnerExceptions(ex);
               }

               return app;
          }

          private static void LogInnerExceptions(Exception ex)
          {
               var current = ex.InnerException;
               while (current != null)
               {
                    Logger.Error(current, "Inner exception: {Message}", current.Message);
                    current = current.InnerException;
               }
          }
     }
}
