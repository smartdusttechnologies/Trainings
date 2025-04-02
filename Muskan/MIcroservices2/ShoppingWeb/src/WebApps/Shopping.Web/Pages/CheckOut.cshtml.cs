namespace Shopping.Web.Pages
{
    public class CheckOutModel(IBasketService basketService, ILogger<CheckOutModel> logger)
        : PageModel
    {
        [BindProperty]
        public BasketCheckOutModel Order { get; set; } = default!;
        public ShoppingCartModel Cart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            logger.LogInformation("Checkout Model is visited");
            Cart = await basketService.LoadUserBasket();

            return Page();
        }
        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            Cart = await basketService.LoadUserBasket();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // assumption customerId is passed in from the UI authenticated user swn        
            Order.CustomerId = new Guid("58c49479-ec65-4de2-86e7-033c546291aa");
            Order.Username = Cart.UserName;
            Order.TotalPrice = Cart.TotalPrice;

            await basketService.CheckoutBasket(new CheckoutBasketRequest(Order));
            return RedirectToPage("Confirmation" , "OrderSubmitted");
    
        }
    }
}