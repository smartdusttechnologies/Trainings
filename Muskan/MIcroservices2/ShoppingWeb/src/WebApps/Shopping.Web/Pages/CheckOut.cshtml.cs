namespace Shopping.Web.Pages
{
     public class CheckOutModel(IBasketService basketService, ILogger<CheckOutModel> logger, IHttpContextAccessor httpContextAccessor)
         : PageModel
     {
          [BindProperty]
          public BasketCheckOutModel Order { get; set; } = default!;
          public ShoppingCartModel Cart { get; set; } = default!;

          public async Task<IActionResult> OnGetAsync()
          {
try{
               //var username = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
               var username = httpContextAccessor.HttpContext?.User.Identity.Name;
               logger.LogInformation("Checkout Model is visited");
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
          public async Task<IActionResult> OnPostCheckOutAsync()
          {
               try
               {

                    //var username = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
                    var username = httpContextAccessor.HttpContext?.User.Identity.Name;
                    Cart = await basketService.LoadUserBasket(username);
                    if (!ModelState.IsValid)
                    {
                         return Page();
                    }
                    // assumption customerId is passed in from the UI authenticated user swn        
                    Order.CustomerId = new Guid("58c49479-ec65-4de2-86e7-033c546291aa");
                    Order.Username = Cart.UserName;
                    Order.TotalPrice = Cart.TotalPrice;

                    await basketService.CheckoutBasket(new CheckoutBasketRequest(Order));
                    return RedirectToPage("Confirmation", "OrderSubmitted");
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