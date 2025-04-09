using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Shopping.Web.Pages.Account
{
     public class LogoutModel(IConfiguration configuration) : PageModel
     {
          public async Task OnGet()
          {
                var returnToUrl = configuration["Auth0:App_Url"] ?? "https://localhost:6065";
               var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                  .WithRedirectUri(returnToUrl)
                   .Build();

               await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
               await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
          }
     }
}
