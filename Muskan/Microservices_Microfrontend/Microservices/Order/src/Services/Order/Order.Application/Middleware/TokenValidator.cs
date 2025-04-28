using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Ordering.Application.Middleware
{
     public class TokenValidator(RequestDelegate next, HttpClient client, IConfiguration configuration, ILogger<TokenValidator> logger)
     {
          public async Task InvokeAsync(HttpContext context)
          {
               // Validate token before proceeding to the next middleware
               if (!await ValidateToken(context))
               {
                    return; // If validation fails, the request stops here
               }

               await next(context); // Call the next middleware in the pipeline
          }

          private async Task<bool> ValidateToken(HttpContext context)
          {
               try
               {
                    logger.LogInformation("Validating token...");
                    var token = context.GetBearerToken();

                    if (string.IsNullOrWhiteSpace(token))
                    {
                         logger.LogWarning("No token provided in Authorization header.");
                         var problem = Results.Problem(
                             title: "Unauthorized",
                             detail: "No token provided in Authorization header.",
                             statusCode: StatusCodes.Status401Unauthorized
                         );
                         await problem.ExecuteAsync(context);
                         return false;
                    }

                    logger.LogInformation("Token provided: {token}", token);
                    var baseUrl = configuration["CommonService:Auth_Url"];

                    var response = await client.PostAsJsonAsync($"{baseUrl}/Security/validate-token", new { Token = token });
                    logger.LogInformation("Response from the Security service: {response}", response);

                    if (!response.IsSuccessStatusCode)
                    {
                         logger.LogWarning("Token validation failed with status code: {statusCode}", response.StatusCode);
                         var problem = Results.Problem(
                             title: "Unauthorized",
                             detail: "Invalid token provided.",
                             statusCode: StatusCodes.Status401Unauthorized
                         );
                         await problem.ExecuteAsync(context);
                         return false; Z
                    }

                    logger.LogInformation("Token validation succeeded.");
                    return true;
               }
               catch (HttpRequestException httpEx)
               {
                    // Log HTTP-specific errors
                    logger.LogError(httpEx, "Error occurred while calling the authentication service.", httpEx.Message);
                    var problem = Results.Problem(
                        title: "Service Unavailable",
                        detail: $"Failed to validate token due to an issue with the authentication service. {httpEx.Message}",
                        statusCode: StatusCodes.Status503ServiceUnavailable
                    );
                    await problem.ExecuteAsync(context);
                    return false;
               }
               catch (Exception ex)
               {
                    // Log any unexpected errors
                    logger.LogError(ex, "An unexpected error occurred during token validation.");
                    var problem = Results.Problem(
                        title: "Internal Server Error",
                        detail: "An unexpected error occurred while validating the token.",
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                    await problem.ExecuteAsync(context);
                    return false;
               }
          }
     }
}
