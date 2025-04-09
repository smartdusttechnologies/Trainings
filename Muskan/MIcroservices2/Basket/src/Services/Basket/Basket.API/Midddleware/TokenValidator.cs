namespace Basket.API.Midddleware
{
     public class TokenValidator(HttpClient client, IConfiguration configuration, ILogger<TokenValidator> logger)
     {
          public async Task<bool> ValidateToken(HttpContext context)
          {
               logger.LogInformation("Validating token...");
               var token = context.GetBearerToken();
               //var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
               if (string.IsNullOrWhiteSpace(token))
               {
                    logger.LogWarning("No token provided in Authorization header.");
                    var problem = Results.Problem(
                     title: "Unauthorized",
                     detail: "No token provided in Authorization header.",
                     statusCode: StatusCodes.Status401Unauthorized
                 );
                    await problem.ExecuteAsync(context);
                    logger.LogInformation("return false ");
                    return false;
               }
               logger.LogInformation("Token provided: {token}", token);
               var baseUrl = configuration["CommonService:Auth_Url"];
               var response = await client.PostAsJsonAsync($"{baseUrl}/Security/validate-token", new { Token = token });
               if (!response.IsSuccessStatusCode)
               {
                    logger.LogWarning("Token validation failed with status code: {statusCode}", response.StatusCode);
                    // Log the response content for debugging
                    var problem = Results.Problem(
                               title: "Unauthorized",
                               detail: "Invalid token provided.",
                               statusCode: StatusCodes.Status401Unauthorized
                                     );
                    await problem.ExecuteAsync(context);
                    return false;
               }
               logger.LogInformation("Token validation succeeded.");
               return true;
          }

     }
}
