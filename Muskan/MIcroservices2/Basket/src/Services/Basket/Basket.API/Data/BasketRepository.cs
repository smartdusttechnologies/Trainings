

using Microsoft.EntityFrameworkCore;

namespace Basket.API.Data
{
    public class BasketRepository(ApplicationDbContext context ,ILoggingService<BasketRepository> logger) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken)
        {
             await logger.LogInformationAsync($"Fetching basket for user: {userName}");

            var basket = await context.ShoppingCarts
                                      .Include(sc => sc.Items)  // Include related items
                                      .FirstOrDefaultAsync(sc => sc.UserName == userName, cancellationToken);
    if (basket is null)
            {
                await logger.LogWarningAsync($"Basket not found for user: {userName}");
                throw new BasketNotFoundException(userName);
            }
               await logger.LogInformationAsync($"Basket retrieved for user: {userName}, Items count: {basket.Items.Count}");
            return basket ;
        }


        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken)
        {
            try
            {
                if (basket.Id == Guid.Empty)
                {
                    basket.Id = Guid.NewGuid();
                }
                context.ShoppingCarts.Add(basket);
                foreach (var item in basket.Items)
                {
                    if (item.Id == Guid.Empty)
                    {
                        item.Id = Guid.NewGuid(); // Assign unique ID
                    }
                    item.ShoppingCartId = basket.Id;  // Link item to the basket
                    context.ShoppingCartItems.Add(item);  // Add each item
                }

                // Save changes to the database
                await context.SaveChangesAsync(cancellationToken);
                  await logger.LogInformationAsync($"Stored basket for user: {basket.UserName} with {basket.Items.Count} items.");
                return basket;

            }
          
            catch (DbUpdateException ex)
{
                
                  await logger.LogErrorAsync("Error storing basket in database.", ex);
                throw;
            }
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
        {await logger.LogInformationAsync($"Attempting to delete basket for user: {userName}");

            var basket = await context.ShoppingCarts
                                        .FirstOrDefaultAsync(sc => sc.UserName == userName, cancellationToken);

            if (basket != null)
            {
                context.ShoppingCarts.Remove(basket);
                await context.SaveChangesAsync(cancellationToken);
                  await logger.LogInformationAsync($"Deleted basket for user: {userName}");
                return true;
            }

       await logger.LogWarningAsync($"No basket found to delete for user: {userName}");
            return false;
        }
        public async Task<bool> UpdateBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            var existingCart = await context.ShoppingCarts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserName == cart.UserName);
            if (existingCart != null)
            {
                context.ShoppingCarts.Remove(existingCart);
            }

            await context.ShoppingCarts.AddAsync(cart);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
