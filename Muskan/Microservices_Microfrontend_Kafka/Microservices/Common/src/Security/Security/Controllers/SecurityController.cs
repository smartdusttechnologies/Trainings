using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Security.Controllers
{
     [ApiController]
     [Route("[controller]")]
     public class SecurityController(IConfiguration config, ILogger<SecurityController> logger) : Controller
     {
          [HttpGet("profile")]
          [Authorize]
          public IActionResult GetUserProfile()
          {
               var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
               var email = User.FindFirst(ClaimTypes.Email)?.Value;
               var name = User.FindFirst(ClaimTypes.Name)?.Value;

               return Ok(new
               {
                    Message = "User profile fetched",
                    UserId = userId,
                    Email = email,
                    Name = name
               });
          }
          /// <summary>
          /// Validates a JWT token using the JWKS endpoint from Auth0 or other identity providers.
          /// This checks signature, expiration, audience, issuer, etc.
          /// </summary>
          [HttpPost("validate-token")]
          public async Task<IActionResult> ValidateToken([FromBody] TokenRequest request)
          {
               logger.LogInformation("Validating token: {Token}", request.Token);
               var rawToken = request.Token?.Trim().Replace("Bearer ", "").Replace("\"", "").Replace("[Authorization] ", "").Replace(",", "");
               logger.LogInformation("Validating token after the extracting: {rawToken}", rawToken);
               if (string.IsNullOrWhiteSpace(rawToken))
               {
                    logger.LogWarning("Token is missing or empty.");
                    return BadRequest(new { message = "Token is missing or empty." });
               }
               // TokenHandler is used to validate the token    ,
               var tokenHandler = new JwtSecurityTokenHandler();

               if (!tokenHandler.CanReadToken(rawToken))
               {
                    logger.LogWarning("Cannot read token: Token is not well formed.");
                    return BadRequest(new { message = "JWT is not well formed" });
               }
               logger.LogInformation("Validating token: {Token}", request.Token);
               // Used to decode and validate the JWT
               //var tokenHandler = new JwtSecurityTokenHandler();
               // Get the JWKS (JSON Web Key Set) URL from configuration
               var jwksUrl = $"https://{config["Auth0:Domain"]}/.well-known/jwks.json";
               // Use HttpClient to fetch the JWKS from the identity provider
               using var http = new HttpClient();
               // Download the JSON containing public keys
               var jwksJson = await http.GetStringAsync(jwksUrl);

               // Parse the JSON into a JWKS object
               var jwks = new JsonWebKeySet(jwksJson);
               // Setup validation parameters
               // Define parameters for validating the JWT token
               var validationParameters = new TokenValidationParameters
               {// Ensure the token was issued by the correct issuer
                    ValidateIssuer = true,
                    // Set the expected issuer URL
                    ValidIssuer = $"https://{config["Auth0:Domain"]}/",
                    // Check if token was intended for this API
                    ValidateAudience = true,
                    // Expected audience value
                    ValidAudience = config["Auth0:Audience"],
                    // Check if token is not expired
                    ValidateLifetime = true,
                    // Verify signature using the correct key
                    ValidateIssuerSigningKey = true,
                    // A method to return signing keys based on the token's 'kid' value
                    IssuerSigningKeyResolver = (rawToken, securityToken, kid, validationParameters) =>
                    {// Match key ID

                         return jwks.Keys.Where(k => k.Kid == kid);
                    },
                    ClockSkew = TimeSpan.FromMinutes(2)
               };

               try
               {
                    logger.LogInformation("Validating token with parameters: {Parameters}", validationParameters);
                    // Try to validate the token using the provided parameters
                    var principal = tokenHandler.ValidateToken(rawToken, validationParameters, out var validatedToken);
                    if (validatedToken is JwtSecurityToken jwtToken && jwtToken.ValidTo < DateTime.UtcNow)
                    {
                         logger.LogInformation("Token is expired");
                         return Unauthorized(new { message = "Token is expired" });
                    }
                    logger.LogInformation("Token is valid");
                    return Ok(new
                    {

                         message = "Token is valid",
                         subject = principal.Identity?.Name,
                         claims = principal.Claims.Select(c => new { c.Type, c.Value })
                    });

               }
               // This catches the error when token has expired
               catch (SecurityTokenExpiredException ex)
               {
                    logger.LogInformation("Token has expired: {Message}", ex.Message);
                    return Unauthorized(new { message = "Token has expired", error = ex.Message });
               }
               // This handles any other kind of validation failure
               catch (SecurityTokenException ex)
               {
                    logger.LogInformation("Token validation failed: {Message}", ex.Message);
                    return Unauthorized(new { message = "Invalid Token", error = ex.Message });
               }
               catch (Exception ex)
               {
                    logger.LogInformation("Unhandled exception during token validation: {Message}", ex.Message);
                    return Unauthorized(new { message = "Unhandled Exception in  Token Validation", error = ex.Message });
               }
          }

          public class TokenRequest
          {
               public string Token
               {
                    get; set;
               }
          }
     }
}
