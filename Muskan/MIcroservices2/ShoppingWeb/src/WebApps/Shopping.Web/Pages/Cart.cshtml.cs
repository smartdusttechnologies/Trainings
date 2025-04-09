

namespace Shopping.Web.Pages
{
     public class CartModel(IBasketService basketService, ILogger<CartModel> logger, IHttpContextAccessor httpContextAccessor) : PageModel
     {
          public ShoppingCartModel Cart { get; set; } = new ShoppingCartModel();
          public async Task<IActionResult> OnGetAsync()
          {
try{
               //var username = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
               var username = httpContextAccessor.HttpContext?.User.Identity.Name;


               logger.LogInformation("Cart Model is visited");
               Cart = await basketService.LoadUserBasket(username);
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
          public async Task<IActionResult> OnPostRemoveToCartAsync(Guid productId)
          {
               try
               {

                    //var username = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                    var username = httpContextAccessor.HttpContext?.User.Identity.Name;
                    Cart = await basketService.LoadUserBasket(username);
                    Cart.Items.RemoveAll(x => x.ProductId == productId);
                    await basketService.StoreBasket(new StoreBasketRequest(Cart));
                    return RedirectToPage();
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
}
