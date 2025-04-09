namespace Shopping.Web.Pages
{
     public class ProductDetailModel(ICatalogServices catalogService, IBasketService basketService, ILogger<ProductListModel> logger, IHttpContextAccessor httpContextAccessor) : PageModel
     {
          public ProductModel Product { get; set; } = default!;

          [BindProperty]
          public string Color { get; set; } = default!;

          [BindProperty]
          public int Quantity { get; set; } = default!;

          public async Task<IActionResult> OnGetAsync(Guid productId)
          {
               try{
               var response = await catalogService.GetProduct(productId);
               Product = response.Products;

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

                    logger.LogInformation("Add to cart button clicked");
                    var productResponse = await catalogService.GetProduct(productId);
                    //var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                    //var username = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                    var username = httpContextAccessor.HttpContext?.User.Identity.Name;
                    var basket = await basketService.LoadUserBasket(username);

                    basket.Items.Add(new ShoppingCartItemModel
                    {
                         ProductId = productId,
                         ProductName = productResponse.Products.Name,
                         Price = productResponse.Products.Price,
                         Quantity = Quantity,
                         Color = Color
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
