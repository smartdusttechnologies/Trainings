using Catalog.API.Midddleware;
using Catalog.API.Models.DTO.CreateDto;
using Catalog.API.Models.DTO.DeleteDto;
using Catalog.API.Models.DTO.GetBYCategory;
using Catalog.API.Models.DTO.GetByIdDto;
using Catalog.API.Models.DTO.GetDto;
using Catalog.API.Models.DTO.UpdateDto;
using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.DeleteProduct;
using Catalog.API.Products.GetProduct;
using Catalog.API.Products.GetProductByCategory;
using Catalog.API.Products.GetProductById;
using Catalog.API.Products.UpdateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
     [ApiController]
     [Route("[controller]")]
     public class ProductsController(ISender sender, IMapper mapper, ILoggingService<ProductsController> loggingService, TokenValidator tokenValidator) : Controller
     {
          [HttpPost]
          [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status201Created)]
          public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
          {
               // Validate the token before proceeding
               await loggingService.LogInformationAsync("Validating the token of the header...");
               var isTokenValid = await tokenValidator.ValidateToken(HttpContext);
               if (!isTokenValid)
               {
                    await loggingService.LogErrorAsync("You are not authorized to access the endpoint", new UnauthorizedAccessException());
                    return Unauthorized("Invalid token.");
               }
               await loggingService.LogInformationAsync("Creating a new product...");

               try
               {
                    if (request == null)
                    {
                         await loggingService.LogErrorAsync("Received a null request!", new ArgumentNullException(nameof(request)));
                         return BadRequest("Invalid request");
                    }
                    await loggingService.LogInformationAsync("Creating product request sent to handler ..");
                    var command = mapper.Map<CreateProductCommand>(request);
                    var result = await sender.Send(command);
                    var response = mapper.Map<CreateProductResponse>(result);

                    return StatusCode(StatusCodes.Status201Created, response);
               }
               catch (Exception ex)
               {
                    await loggingService.LogErrorAsync("An error occurred while creating a product.", ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the product.");
               }
          }
          /// <summary>
          /// Gets a list of products in the catalog with pagination support.
          /// </summary>
          /// <param name="pageNumber">The page number to retrieve.</param>
          /// <param name="pageSize">The number of products per page.</param>
          /// <returns>A list of products in the catalog.</returns>
          [HttpGet]
          [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 10)
          {
               try
               {
                    await loggingService.LogInformationAsync($"Getting products (Page: {pageNumber}, Size: {pageSize})...");

                    // Creating the query object to fetch the products
                    var query = new GetProductQuery(pageNumber, pageSize);
                    var result = await sender.Send(query);
                    // Using AutoMapper to map the result into a response
                    var response = mapper.Map<GetProductResponse>(result);
                    return Ok(response);  // Return the products as a response
               }
               catch (Exception ex)
               {
                    await loggingService.LogErrorAsync("An error occurred while fetching products.", ex);
                    return BadRequest($"An error occurred while fetching the products. {ex}");
               }
          }
          /// <summary>
          /// Gets a specific product by ID.
          /// </summary>
          /// <param name="id">The ID of the product to retrieve.</param>
          /// <returns>A specific product in the catalog.</returns>
          [HttpGet("{id}")]
          [ProducesResponseType(typeof(GetProductByIdResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          public async Task<IActionResult> GetProductById(Guid id)
          {
               try
               {
                    await loggingService.LogInformationAsync($"Getting product with ID: {id}...");

                    // Creating the query object to fetch the product by ID
                    var query = new GetProductByIdQuery(id);
                    var result = await sender.Send(query);

                    if (result.Products == null)
                    {
                         return NotFound("Product not found.");
                    }

                    // Using AutoMapper to map the result into a response
                    var response = mapper.Map<GetProductByIdResponse>(result);

                    return Ok(response);  // Return the product as a response
               }
               catch (Exception ex)
               {
                    await loggingService.LogErrorAsync("An error occurred while fetching the product by ID.", ex);
                    return BadRequest("An error occurred while fetching the product.");
               }
          }
          // Get products by category
          [HttpGet("category/{category}")]
          public async Task<IActionResult> GetProductByCategory(string category)
          {
               try
               {
                    // Logging the request
                    await loggingService.LogInformationAsync("Getting products by category...");

                    // Send query to get products by category
                    var result = await sender.Send(new GetProductByCategoryQuery(category));

                    // Handle case where no products are found
                    if (result == null)
                    {
                         await loggingService.LogWarningAsync($"No products found for category: {category}");
                         return NotFound("Products not found in this category");
                    }
                    // Log successful query result
                    await loggingService.LogInformationAsync($"Successfully retrieved {result.Products.Count()} products for category: {category}");
                    // Map the result to the response DTO
                    var response = mapper.Map<GetProductByCategoryResponse>(result);
                    await loggingService.LogInformationAsync($"Returning {response.Products.Count()} products for category: {category}");
                    // Return success with the response
                    return Ok(response);
               }
               catch (Exception ex)
               {
                    // Log and return the error
                    await loggingService.LogErrorAsync("Error occurred in the GetProductByCategory endpoint", ex);
                    return BadRequest("An error occurred while retrieving the product by category.");
               }
          }

          [HttpPut]
          [ProducesResponseType(typeof(UpdateProductResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          public async Task<IActionResult> UpdateProduct(UpdateProductRequest request)
          {
               await loggingService.LogInformationAsync("Validating the token of the header...");
               var isTokenValid = await tokenValidator.ValidateToken(HttpContext);
               if (!isTokenValid) 
               {
                    await loggingService.LogErrorAsync("You are not authorized to access the endpoint", new UnauthorizedAccessException());
                    return Unauthorized("Invalid token.");
               }
               try
               {
                    await loggingService.LogInformationAsync($"Received update request for Product Id: {request.Id}");

                    // Mapping the request to command using AutoMapper
                    var command = mapper.Map<UpdateProductCommand>(request);

                    UpdateProductResult result;

                    try
                    {
                         // Sending the command to the handler via Mediator
                         result = await sender.Send(command);
                    }
                    catch (ProductNotFoundException ex)
                    {
                         await loggingService.LogErrorAsync($"Product not found for Id: {command.Id}", ex);
                         return NotFound($"Product with ID {command.Id} was not found.");
                    }
                    catch (Exception ex)
                    {
                         await loggingService.LogErrorAsync("Unexpected error occurred while updating product", ex);
                         return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while updating the product.");
                    }

                    if (!result.IsSuccess)
                    {
                         await loggingService.LogWarningAsync($"Update failed for Product Id: {command.Id}");
                         return BadRequest($"Failed to update Product with ID {command.Id}");
                    }

                    await loggingService.LogInformationAsync($"Successfully updated Product Id: {command.Id}");

                    // Mapping the result to response using AutoMapper
                    var response = mapper.Map<UpdateProductResponse>(result);

                    return Ok(response);
               }
               catch (Exception ex)
               {
                    await loggingService.LogErrorAsync("An error occurred while processing the update request.", ex);
                    return BadRequest("An error occurred while processing the update request.");
               }
          }
          /// <summary>
          /// Deletes a product from the catalog by its ID.
          /// </summary>
          /// <param name="id">The product ID to be deleted.</param>
          /// <returns>A response indicating if the deletion was successful.</returns>
          [HttpDelete("{id}")]
          [ProducesResponseType(typeof(DeleteProductResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          [ProducesResponseType(StatusCodes.Status500InternalServerError)]
          public async Task<IActionResult> DeleteProduct(Guid id)
          {
               await loggingService.LogInformationAsync("Validating the token of the header...");
               var isTokenValid = await tokenValidator.ValidateToken(HttpContext);
               if (!isTokenValid)
               {
                    await loggingService.LogErrorAsync("You are not authorized to access the endpoint", new UnauthorizedAccessException());
                    return Unauthorized("Invalid token.");
               }
               try
               {
                    await loggingService.LogInformationAsync($"Deleting product with ID: {id}...");

                    // Sending command to delete the product
                    var command = new DeleteProductCommand(id);
                    var result = await sender.Send(command);

                    // Using AutoMapper to map the result into a response
                    var response = mapper.Map<DeleteProductResponse>(result);

                    if (result.isSuccess)
                    {
                         return Ok(response);  // Return success response
                    }
                    else
                    {
                         return BadRequest("Product could not be deleted.");  // Return failure response
                    }
               }
               catch (Exception ex)
               {
                    await loggingService.LogErrorAsync("An error occurred while deleting the product.", ex);
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
               }
          }
     }
}
