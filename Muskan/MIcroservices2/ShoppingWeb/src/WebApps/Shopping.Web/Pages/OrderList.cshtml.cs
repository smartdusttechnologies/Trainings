namespace Shopping.Web.Pages
{
     public class OrderListModel(IOrderingService orderingService, ILogger<OrderListModel> logger, IHttpContextAccessor httpContextAccessor)
         : PageModel
     {
          public IEnumerable<OrderModel> Orders { get; set; } = default!;

          public async Task<IActionResult> OnGetAsync()
          {
               try
               {

                    logger.LogInformation("OrderList Model is visited");
                    var customerId = new Guid("58c49479-ec65-4de2-86e7-033c546291aa");  
                    var response = await orderingService.GetOrdersByCustomer(customerId);
                    Orders = response.Orders;

                    return Page();
               }
           catch (ApiException apiEx)
            {
                logger.LogError(apiEx, "API error while Getting the Order list.");
                TempData["ErrorMessage"] = $"Error from API: {apiEx.Message}. Please try again later.";
                return RedirectToPage("/Error");
            }
                 catch (HttpRequestException httpEx)
            {
                logger.LogError(httpEx, "HTTP error occurred while fetching Orders.");
                TempData["ErrorMessage"] = "Network issue: Unable to load products. Please check your connection and try again.";
                return RedirectToPage("/Error");
            }
            catch (TimeoutException timeoutEx)
            {
                logger.LogError(timeoutEx, "Timeout while fetching Order list.");
                TempData["ErrorMessage"] = "The request took too long. Please try again later.";
                return RedirectToPage("/Error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while fetching Order list.");
                TempData["ErrorMessage"] = $"Something went wrong: {ex.Message}. Please try again later.";
                return RedirectToPage("/Error");
            }
          }
     }
}
