namespace Basket.API.Extensions
{
     public static class HttpContextExtensions
     {
          public static string? GetBearerToken(this HttpContext context)
          {
               var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                                                   .CreateLogger("HttpContextExtensions");

               logger.LogInformation("Extracting Bearer token from Authorization header.");
               var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
               logger.LogInformation($"Authorization Header {authHeader}");
               if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
               {
                    var token = authHeader["Bearer ".Length..].Trim();
                    // Remove anything after token (e.g., comma, {Authorization}, etc.)
                    var stopChars = new[] { ',', ' ' };
                    var index = token.IndexOfAny(stopChars);
                    if (index > 0)
                         token = token[..index].Trim();
                    // Fix: Remove anything after comma or invalid characters
                    if (token.Contains(','))
                    {
                         token = token.Split(',')[0].Trim();
                    }

                    logger.LogInformation("Bearer token cleaned and extracted.");
                    logger.LogInformation($"token cleaned and extracted : {token} .");
                    return token;
               }

               logger.LogWarning("Bearer token missing.");
               return null;
          }
     }
}
