﻿namespace Catalog.API.Products.CreateProduct
{
     /// <summary>
     /// Represents a request to create a new product in the catalog.
     /// </summary>
     /// <param name="Name"></param>
     /// <param name="Category"></param>
     /// <param name="Description"></param>
     /// <param name="Price"></param>
     /// <param name="Image"></param>
     public record CreateProductRequest(string Name, List<string> Category, string Description, decimal Price, string Image) : ICommand<CreateProductResult>;
     /// <summary>
     /// This represents the response returned when a product is created.
     /// </summary>
     /// <param name="Id"></param>
     public record CreateProductResponse(Guid Id);
     /// <summary>
     /// Represents the endpoint for creating a new product in the catalog.
     /// This class implements ICarterModule, which is used to define API endpoints in Carter,
     /// a minimalistic framework for handling routes in ASP.NET Core.
     /// </summary>
     public class CreateProductEndpoint : ICarterModule
     {
          /// <summary>
          /// Adds the routes for creating a new product in the catalog.
          /// </summary>
          /// <param name="app"></param>
          /// <exception cref="Exception"></exception>
          public void AddRoutes(IEndpointRouteBuilder app)
          {
               //Defines a POST route /products to create a new product.
               //            CreateProductRequest request(the incoming request DTO).
               //ISender sender(MediatR's sender to dispatch commands).
               app.MapPost("/products", async (CreateProductRequest request, ISender sender, ILoggingService<CreateProductEndpoint> loggingService) =>
               {
                    var correlationId = Guid.NewGuid().ToString(); // Generate a new correlation ID
                    await loggingService.LogInformationAsync("Creating a new product...");
                    try
                    {
                         if (request == null)
                         {
                              await loggingService.LogErrorAsync("Received a null request!", new ArgumentNullException(nameof(request)));
                              Console.WriteLine("Received a null request!");
                              return Results.BadRequest("Invalid request");
                         }
                         // Convert request DTO to CreateProductCommand
                         var command = request.Adapt<CreateProductCommand>();
                         // Send the command to MediatR
                         var result = await sender.Send(command);
                         // Convert the result to response DTO
                         var response = result.Adapt<CreateProductResponse>();
                         // Return HTTP 201 Created with response
                         return Results.Created($"/products/{response.Id}", response);
                    }
                    catch (Exception ex)
                    {
                         await loggingService.LogErrorAsync("An error occurred while creating a product.", ex);
                         Console.WriteLine(ex.Message);
                         throw new Exception(ex.Message);
                    }

               })
                    .RequireCustomAuth()
                    .WithName("CreateProduct")
               .Produces<CreateProductResponse>(StatusCodes.Status201Created).
               ProducesProblem(StatusCodes.Status400BadRequest)
               .WithSummary("Create Product")
               .WithDescription("Creates a new product in the catalog.");
          }
     }
}
