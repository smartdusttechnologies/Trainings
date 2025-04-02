

using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    /// <summary>
    /// CachedBasketRepository class
    /// IBasketRepository repository: The actual database repository responsible for storing and retrieving basket data.
    /// IDistributedCache cache: The caching layer(usually Redis) used for improving performance.
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="cache"></param>
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {
        /// <summary>
        /// GetBasket method
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            //f the basket is in the cache, it deserializes and returns it (cache hit).
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            // Check if the basket is in the cache and return it if it is (cache hit).
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                // If the basket is in the cache, it deserializes and returns it (cache hit).
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);
            }
            // If the basket is not in the cache, it retrieves it from the database and stores it in the cache (cache miss).
            var basket = await repository.GetBasket(userName, cancellationToken);
            // Set the basket in the cache . 
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
            // return the baskets
            return basket;
        }
        /// <summary>
        /// Store Basket Method
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            // Stores the basket in the database
            await repository.StoreBasket(basket, cancellationToken);
            // Store the basket in thr Cache 
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
            // return the basket
            return basket;
        }
        /// <summary>
        /// Delete Basket Method
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            // Deletes the basket from the database
            await repository.DeleteBasket(userName, cancellationToken);
            // Deletes the basket from the cache
            await cache.RemoveAsync(userName, cancellationToken);
            // return true
            return true;
        }
    }
}
