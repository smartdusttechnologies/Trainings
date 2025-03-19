using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Discount.Grpc.Data
{
    public static class Extensions
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();
            context.Database.MigrateAsync();
            return app;
        }
    }
}
