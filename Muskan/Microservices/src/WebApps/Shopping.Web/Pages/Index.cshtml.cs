namespace Shopping.Web.Pages;
/// <summary>
/// Razor Page dor Index 
/// </summary>
/// <param name="_logger"></param>
/// <param name="_catalogServices"></param>
/// <param name="_basketServices"></param>
public class IndexModel(ILogger<IndexModel> _logger, ICatalogServices _catalogServices, IBasketService _basketServices) : PageModel
{
    /// <summary>
    ///  A collection to store the list of products retrieved from the catalog service.
    /// </summary>
    public IEnumerable<ProductModel> ProductList { get; set; } = new List<ProductModel>();
    /// <summary>
    /// Handles HTTP GET requests to load products when the page is visited.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            _logger.LogInformation("Index Page Visited");
            //load the product list from the catatlog service with api gateway
            var result = await _catalogServices.GetProducts();
            ProductList = result.Products;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product list.");
            // Storing an error message in TempData to display on the UI.
            TempData["ErrorMessage"] = $"Something Went wrong  :  {ex.Message} \n Please try again later.";
            return RedirectToPage("/Error");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
    {
        try
        {
            _logger.LogInformation("Add to Cart is Clicked Visited");
            var productResponse = await _catalogServices.GetProduct(productId);
            var basket = await _basketServices.LoadUserBasket();

            basket.Items.Add(
                new ShoppingCartItemModel
                {
                    ProductId = productId,
                    ProductName = productResponse.Products.Name,
                    Price = productResponse.Products.Price,
                    Quantity = 1,
                    Color = "Black"
                });
            await _basketServices.StoreBasket(new StoreBasketRequest(basket));

            return RedirectToPage("Cart");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding product to cart.");
            TempData["ErrorMessage"] = $"Something Went wrong  :  {ex.Message} \n Please try again later.";
            return RedirectToPage("/Error");
        }
    }
}