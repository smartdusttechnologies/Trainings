using Catalog.API.Midddleware;

namespace Catalog.API.Extensions
{
     public static class EndpointConventionBuilderExtensions
     {
          /// <summary>
          /// This is an extension method for RouteHandlerBuilder to require custom authentication on endpoints.
          /// </summary>
          /// <param name="builder"></param>
          /// <returns></returns>
          public static RouteHandlerBuilder RequireCustomAuth(this RouteHandlerBuilder builder)
          {

               // RequireCustomAuth is an extension method that adds a custom authentication filter to the endpoint.
               // It uses the TokenValidator service to validate the token in the request header.

               // The method takes a RouteHandlerBuilder instance and returns it after adding the filter.
               // This allows you to chain other methods on the builder if needed.

               // The method is generic and can be used with any endpoint type.
               // It uses dependency injection to resolve the TokenValidator service from the request's service provider.

               // The filter checks if the token is valid and either continues to the next middleware or returns a 401 Unauthorized response.
               // This is useful for securing endpoints that require authentication.
               {

                    // AddEndpointFilter attaches a filter (middleware-like function) to this endpoint.
                    // It runs before the actual endpoint handler and can control access or modify behavior.

                    /// AddEndpointFilter
                    /// Adds a filter to an endpoint that executes custom logic **before or after** the request handler.
                    /// Authentication, validation, logging, etc., for specific endpoints..
                    /// To run token validation before executing the main logic of the endpoint.

                    return builder.AddEndpointFilter(async (context, next) =>
                    {
                         // Access the current HTTP request context
                         var httpContext = context.HttpContext;

                         // Create a logger manually using ILoggerFactory since static types can't be used directly
                         var loggerFactory = httpContext.RequestServices.GetRequiredService<ILoggerFactory>();
                         var logger = loggerFactory.CreateLogger("CustomAuth");

                         logger.LogInformation("Executing custom authentication filter.");

                         //  Keyword: RequestServices
                         //  Definition: Provides access to registered services (via dependency injection) within the current request scope.
                         // Use Case: Resolving services like database, validators, loggers, etc.
                         // Resolve the custom TokenValidator service from DI container
                         var validator = httpContext.RequestServices.GetRequiredService<TokenValidator>();
                         //  Method: ValidateToken
                         //  Purpose: Validates the token (e.g., JWT) included in the request to check if the user is authorized.

                         // Validate the token
                         // If token is invalid, return 401 Unauthorized response immediately
                         var isValid = await validator.ValidateToken(httpContext);
                         if (!isValid)
                         {
                              logger.LogWarning("Token validation failed. Unauthorized access.");
                              return Results.Unauthorized();
                         }

                         // If valid, continue to the next filter or the endpoint handler
                         logger.LogInformation("Token validation succeeded. Proceeding to the next middleware.");
                         return await next(context);
                    });
               }
          }

     }
}