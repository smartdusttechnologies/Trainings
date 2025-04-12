using Microsoft.AspNetCore.Mvc;

namespace Shopping.Web.Pages
{
    public class CartModel(IBasketService basketService , ILogger<CartModel> logger) : PageModel
    {
        public ShoppingCartModel Cart { get; set; } = new ShoppingCartModel();
        public async Task<IActionResult> OnGetAsync()
        {
            logger.LogInformation("Cart Model is visited");
            Cart = await basketService.LoadUserBasket();
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveToCartAsync(Guid productId)
        {
            Cart = await basketService.LoadUserBasket();
            Cart.Items.RemoveAll(x => x.ProductId == productId);
            await basketService.StoreBasket(new StoreBasketRequest(Cart));
            return RedirectToPage();
        }
    }
}
