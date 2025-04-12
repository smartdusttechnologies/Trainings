using Microsoft.AspNetCore.Http.HttpResults;
using System.Numerics;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ordering.Infrastructure.Data.Interceptors
{
    /// <summary>
    /// This interceptor is used to automatically update auditing fields before saving changes in the database.
    /// SaveChangesInterceptor 
    /// provides hooks into the save changes operations of DbContext.
    /// This feature is incredibly beneficial for implementing auditing functionalities, 
    /// where you need to track changes to entities, such as who created or modified an entity and when.
    /// </summary>
    public class AuditableInterceptor : SaveChangesInterceptor
    {
        /// <summary>
        /// This method intercepts the saving process and updates audit fields for entities before committing to the database.
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context); // Update audit fields asynchronously        
            return base.SavingChanges(eventData, result);
        }
        /// <summary>
        /// Asynchronous version of the above method for handling database operations in a non-blocking manner.
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="result"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context); // Update audit fields asynchronously
            return base.SavingChangesAsync(eventData, result, cancellationToken);      
        }
        /// <summary>
        /// This method updates entities by setting 'CreatedAt', 'UpdatedAt', 'CreatedBy', and 'UpdatedBy' fields automatically.
        /// </summary>
        /// <param name="context"></param>
        public void UpdateEntities(DbContext? context)
        {
            if (context is null) return; //Ensure the context is not null
            foreach (var entry in context.ChangeTracker.Entries<IEntity>())
            { 
                // If the entity is newly added, set the CreatedAt and CreatedBy fields.
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "SysAdmin"; // System-defined user
                }
                // If the entity is added, modified, or has owned entities changed, update the UpdatedAt and UpdatedBy fields
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = "SysAdmin";
                }
            }
        }
    }
 
}
/// <summary>
/// 
/// </summary>
public static class Extensions
{
    /// <summary>
    /// This method checks if any owned entities within an entry have changed (Added or Modified).
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null && // Ensure there is a referenced entity
            r.TargetEntry.Metadata.IsOwned() && // Check if the target entity is owned
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
//When?
//When you need to track changes in owned entities, such as embedded or nested objects.
//Why?
//Ensures updates in related entities are tracked properly.
//Prevents missing updates on complex objects.
}