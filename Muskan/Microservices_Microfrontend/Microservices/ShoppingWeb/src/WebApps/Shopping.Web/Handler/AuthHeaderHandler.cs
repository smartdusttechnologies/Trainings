using System.Net.Http.Headers;

namespace Shopping.Web.Handler
{

     public class AuthHeaderHandler : DelegatingHandler
     {
          private readonly IHttpContextAccessor _httpContextAccessor;

          public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
          {
               _httpContextAccessor = httpContextAccessor;
          }
          protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
          {
               var authResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();

               if (authResult.Succeeded && authResult.Principal != null)
               {
                    var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token"); // Change "access_token" to "id_token"

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    else
                    {
                         Console.WriteLine("⚠️ Warning: Authorization token is missing in `AuthHeaderHandler`");
                    }
               }
               else
               {
                    Console.WriteLine("⚠️ Warning: User is not authenticated.");
               }

               return await base.SendAsync(request, cancellationToken);
          }
     }
     //protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
     //{
     //     var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

     //     if (!string.IsNullOrEmpty(accessToken))
     //     {
     //          request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
     //     }

     //     return await base.SendAsync(request, cancellationToken);
     //}
}

