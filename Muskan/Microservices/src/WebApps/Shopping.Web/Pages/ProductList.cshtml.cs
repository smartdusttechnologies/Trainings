namespace Shopping.Web.Pages
{
    public class ProductListModel(ICatalogServices catalogService, IBasketService basketService, ILogger<ProductListModel> logger)
         : PageModel
    {
        public IEnumerable<string> CategoryList { get; set; } = [];
        public IEnumerable<ProductModel> ProductList { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            logger.LogInformation("Product Model is visited");
            var response = await catalogService.GetProducts();
            CategoryList = response.Products.SelectMany(p => p.Category).Distinct();
            if (!string.IsNullOrEmpty(categoryName)) {
                ProductList = response.Products.Where(p => p.Category.Contains(categoryName));
                SelectedCategory = categoryName;
            }else
            {
                ProductList = response.Products;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            var productRepose = await catalogService.GetProduct(productId);

            var basket = await basketService.LoadUserBasket();

            basket.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = productRepose.Products.Name,
                Price = productRepose.Products.Price,
                Quantity =1 ,
                Color = "Black"
            });
            await basketService.StoreBasket(new StoreBasketRequest(basket));
            return RedirectToPage("Cart");
        }
    }
}
