namespace Shopping.Web.Pages;
/// <summary>
/// Razor Page dor Index 
/// </summary>
/// <param name="_logger"></param>
/// <param name="_catalogServices"></param>
/// <param name="_basketServices"></param>
public class IndexModel(ILogger<IndexModel> logger, ICatalogServices _catalogServices, IBasketService _basketServices, IHttpContextAccessor httpContextAccessor) : PageModel
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
               logger.LogInformation("Index Page Visited");
               //load the product list from the catatlog service with api gateway
               var result = await _catalogServices.GetProducts();
               ProductList = result.Products;
               return Page();
          }
        catch (ApiException apiEx)
            {
                logger.LogError(apiEx, "API error Occur.");
                TempData["ErrorMessage"] = $"Error from API: {apiEx.Message}. Please try again later.";
                return RedirectToPage("/Error");
            }
                 catch (HttpRequestException httpEx)
            {
                logger.LogError(httpEx, "HTTP error occurred.");
                TempData["ErrorMessage"] = "Network issue: Unable to load products. Please check your connection and try again.";
                return RedirectToPage("/Error");
            }
            catch (TimeoutException timeoutEx)
            {
                logger.LogError(timeoutEx, "Timeout Error Occured");
                TempData["ErrorMessage"] = "The request took too long. Please try again later.";
                return RedirectToPage("/Error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occured.");
                TempData["ErrorMessage"] = $"Something went wrong: {ex.Message}. Please try again later.";
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
               var username = httpContextAccessor.HttpContext?.User.Identity.Name;
               logger.LogInformation("Add to Cart is Clicked Visited");
               var productResponse = await _catalogServices.GetProduct(productId);
               var basket = await _basketServices.LoadUserBasket(username);

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
         catch (ApiException apiEx)
            {
                logger.LogError(apiEx, "API error Occur.");
                TempData["ErrorMessage"] = $"Error from API: {apiEx.Message}. Please try again later.";
                return RedirectToPage("/Error");
            }
                 catch (HttpRequestException httpEx)
            {
                logger.LogError(httpEx, "HTTP error occurred.");
                TempData["ErrorMessage"] = "Network issue: Unable to load products. Please check your connection and try again.";
                return RedirectToPage("/Error");
            }
            catch (TimeoutException timeoutEx)
            {
                logger.LogError(timeoutEx, "Timeout Error Occured");
                TempData["ErrorMessage"] = "The request took too long. Please try again later.";
                return RedirectToPage("/Error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error occured.");
                TempData["ErrorMessage"] = $"Something went wrong: {ex.Message}. Please try again later.";
                return RedirectToPage("/Error");
            }
     }
}