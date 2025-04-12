using Auth0.AspNetCore.Authentication;

namespace Shopping.Web.Pages.Account
{
     public class LoginModel(IConfiguration configuration) : PageModel
     {
          public async Task OnGet(string? returnUrl = null)
          {
               // Set the base URL for the application
               var baseUrl = configuration["Auth0:App_Url"] ?? "https://localhost:6065";
               returnUrl ??= $"{baseUrl}/Account/Profile";

               // Create authentication properties for the login request
               var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                           .WithRedirectUri(returnUrl)
                           .Build();

               // Challenge the user to log in using Auth0
               await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
          }          
     }
}
