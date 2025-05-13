namespace Basket.API.Midddleware
{
    public class TokenValidator
    {
   
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenValidator> _logger;

        public TokenValidator( HttpClient client, IConfiguration configuration, ILogger<TokenValidator> logger)
        {
   
            _client = client;
            _configuration = configuration;
            _logger = logger;
        }
        //   public async Task InvokeAsync(HttpContext context)
        //   {
        //        // Validate token before proceeding to the next middleware
        //        if (!await ValidateToken(context))
        //        {
        //             return; // If validation fails, the request stops here
        //        }

        //        await _next(context); // Call the next middleware in the pipeline
        //   }

        public async Task<bool> ValidateToken(HttpContext context)
        {
            try
            {
                 _logger.LogInformation("Validating token...");
               var token = context.GetBearerToken();
               //var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
               if (string.IsNullOrWhiteSpace(token))
               {
                    _logger.LogWarning("No token provided in Authorization header.");
                    var problem = Results.Problem(
                     title: "Unauthorized",
                     detail: "No token provided in Authorization header.",
                     statusCode: StatusCodes.Status401Unauthorized
                 );
                    await problem.ExecuteAsync(context);
                    _logger.LogInformation("return false ");
                    return false;
               }
               _logger.LogInformation("Token provided: {token}", token);
                var baseUrl = _configuration["CommonService:Auth_Url"];
                
                var response = await _client.PostAsJsonAsync($"{baseUrl}/Security/validate-token", new { Token = token });
                _logger.LogInformation("Response from the Security service: {response}", response);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Token validation failed with status code: {statusCode}", response.StatusCode);
                    var problem = Results.Problem(
                        title: "Unauthorized",
                        detail: "Invalid token provided.",
                        statusCode: StatusCodes.Status401Unauthorized
                    );
                    await problem.ExecuteAsync(context);
                    return false;
                }

                _logger.LogInformation("Token validation succeeded.");
                return true;
            }
            catch (HttpRequestException httpEx)
            {
                // Log HTTP-specific errors
                _logger.LogError(httpEx, "Error occurred while calling the authentication service." ,httpEx.Message);
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
                _logger.LogError(ex, "An unexpected error occurred during token validation.");
                var problem = Results.Problem(
                    title: "Internal Server Error",
                    detail: $"An unexpected error occurred while validating the token. {ex.Message}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
                await problem.ExecuteAsync(context);
                return false;
            }
        }
    }
}