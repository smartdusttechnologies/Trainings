namespace Shopping.Web.Pages
{
     public class ProductListModel(ICatalogServices catalogService, IBasketService basketService, ILogger<ProductListModel> logger, IHttpContextAccessor httpContextAccessor)
          : PageModel
     {
          public IEnumerable<string> CategoryList { get; set; } = [];
          public IEnumerable<ProductModel> ProductList { get; set; } = [];

          [BindProperty(SupportsGet = true)]
          public string SelectedCategory { get; set; } = default!;

          public async Task<IActionResult> OnGetAsync(string categoryName)
          {
               try
               {

                    logger.LogInformation("Product Model is visited");
                    var response = await catalogService.GetProducts();
                    CategoryList = response.Products.SelectMany(p => p.Category).Distinct();
                    if (!string.IsNullOrEmpty(categoryName))
                    {
                         ProductList = response.Products.Where(p => p.Category.Contains(categoryName));
                         SelectedCategory = categoryName;
                    }
                    else
                    {
                         ProductList = response.Products;
                    }
                    return Page();
               }
            catch (ApiException apiEx)
            {
                logger.LogError(apiEx, "API error while adding product to cart.");
                TempData["ErrorMessage"] = $"Error from API: {apiEx.Message}. Please try again later.";
                return RedirectToPage("/Error");
            }
                 catch (HttpRequestException httpEx)
            {
                logger.LogError(httpEx, "HTTP error occurred while fetching products.");
                TempData["ErrorMessage"] = "Network issue: Unable to load products. Please check your connection and try again.";
                return RedirectToPage("/Error");
            }
            catch (TimeoutException timeoutEx)
            {
                logger.LogError(timeoutEx, "Timeout while fetching product list.");
                TempData["ErrorMessage"] = "The request took too long. Please try again later.";
                return RedirectToPage("/Error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while fetching product list.");
                TempData["ErrorMessage"] = $"Something went wrong: {ex.Message}. Please try again later.";
                return RedirectToPage("/Error");
            }
          }
          public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
          {
               try
               {

                    //var username = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                    var username = httpContextAccessor.HttpContext?.User.Identity.Name;
                    var productRepose = await catalogService.GetProduct(productId);

                    var basket = await basketService.LoadUserBasket(username);

                    basket.Items.Add(new ShoppingCartItemModel
                    {
                         ProductId = productId,
                         ProductName = productRepose.Products.Name,
                         Price = productRepose.Products.Price,
                         Quantity = 1,
                         Color = "Black"
                    });
                    await basketService.StoreBasket(new StoreBasketRequest(basket));
                    return RedirectToPage("Cart");
               }
                    catch (ApiException apiEx)
            {
                logger.LogError(apiEx, "API error while adding product to cart.");
                TempData["ErrorMessage"] = $"Error from API: {apiEx.Message}. Please try again later.";
                return RedirectToPage("/Error");
            }
                 catch (HttpRequestException httpEx)
            {
                logger.LogError(httpEx, "HTTP error occurred while fetching products.");
                TempData["ErrorMessage"] = "Network issue: Unable to load products. Please check your connection and try again.";
                return RedirectToPage("/Error");
            }
            catch (TimeoutException timeoutEx)
            {
                logger.LogError(timeoutEx, "Timeout while fetching product list.");
                TempData["ErrorMessage"] = "The request took too long. Please try again later.";
                return RedirectToPage("/Error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while fetching product list.");
                TempData["ErrorMessage"] = $"Something went wrong: {ex.Message}. Please try again later.";
                return RedirectToPage("/Error");
            }
               
          }
     }
}
