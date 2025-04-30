using Microsoft.AspNetCore.Authentication;

namespace Shopping.Web.Pages.Auth
{
     public class CallbackModel : PageModel
     {
          public async Task<IActionResult> OnGetAsync()
          {
               // Handle OAuth Callback
               var authenticateResult = await HttpContext.AuthenticateAsync();

               if (!authenticateResult.Succeeded)
               {
                    return RedirectToPage("/Error"); // Redirect if authentication failed
               }

               // Retrieve user information (if needed)
               var claims = authenticateResult.Principal?.Claims;

               // Redirect to home or dashboard after successful login
               return RedirectToPage("/Index");
          }
     }
}