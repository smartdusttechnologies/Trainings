

using Microsoft.EntityFrameworkCore;

namespace Basket.API.Data
{
    public class BasketRepository(ApplicationDbContext context) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken)
        {
            var basket = await context.ShoppingCarts
                                      .Include(sc => sc.Items)  // Include related items
                                      .FirstOrDefaultAsync(sc => sc.UserName == userName, cancellationToken);

            return basket is null ? throw new BasketNotFoundException(userName) : basket;
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
                return basket;

            }
          
            catch (DbUpdateException ex)
{
                
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
        {
            var basket = await context.ShoppingCarts
                                        .FirstOrDefaultAsync(sc => sc.UserName == userName, cancellationToken);

            if (basket != null)
            {
                context.ShoppingCarts.Remove(basket);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            }

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
