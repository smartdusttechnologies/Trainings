using Basket.API.Baskets.CheckOutBasket;
using Basket.API.Baskets.DeleteBasket;
using Basket.API.Baskets.GetBasket;
using Basket.API.Baskets.StoreBasket;
using Basket.API.Baskets.UpdateBasket;
using Basket.API.DTOs.CheckoutBasketDtos;
using Basket.API.DTOs.DeleteBasketDtos;
using Basket.API.DTOs.GetBasketDtos;
using Basket.API.DTOs.StoreBasketDtos;
using Basket.API.DTOs.UpdateBasketDtos;
using  Basket.API.Midddleware;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
     [ApiController]
     [Route("basket")]
     public class BasketController(IMapper mapper, ISender sender, ILoggingService<BasketController> logger , TokenValidator tokenValidator) : Controller
     {
          [HttpPost]
          [ProducesResponseType(typeof(StoreBasketResponse), StatusCodes.Status201Created)]
          [ProducesResponseType(StatusCodes.Status500InternalServerError)]
          public async Task<IActionResult> StoreBasket([FromBody] StoreBasketRequest request)
          {
               try
               {
                          await logger.LogInformationAsync("Validating the token of the header...");
               var isTokenValid = await tokenValidator.ValidateToken(HttpContext);
               if (!isTokenValid)
               {
                    await logger.LogErrorAsync("You are not authorized to access the endpoint", new UnauthorizedAccessException());
                    return Unauthorized("Invalid token.");
               }
                    await logger.LogInformationAsync("Storing basket for user: " + request.Cart.UserName);

                    var command = mapper.Map<StoreBasketCommand>(request);
                    var result = await sender.Send(command);
                    var response = mapper.Map<StoreBasketResponse>(result);

                    return StatusCode(StatusCodes.Status201Created, response);
               }
               catch (Exception ex)
               {
                    await logger.LogErrorAsync("Error storing basket", ex);
                    return Problem("An error occurred while storing the basket.", statusCode: StatusCodes.Status500InternalServerError);
               }
          }
          [HttpGet("{userName}")]
          [ProducesResponseType(typeof(GetBasketResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status500InternalServerError)]
          public async Task<IActionResult> GetBasket(string userName)
          {
               try
               {
                                   await logger.LogInformationAsync("Validating the token of the header...");
               var isTokenValid = await tokenValidator.ValidateToken(HttpContext);
               if (!isTokenValid)
               {
                    await logger.LogErrorAsync("You are not authorized to access the endpoint", new UnauthorizedAccessException());
                    return Unauthorized("Invalid token.");
               }
                    await logger.LogInformationAsync("Getting basket for user: " + userName);

                    var result = await sender.Send(new GetBasketQuery(userName));
                    var response = mapper.Map<GetBasketResponse>(result);

                    return Ok(response);
               }
               catch (BasketNotFoundException ex)
               {
                    await logger.LogErrorAsync("Basket not found for user : " + userName, ex);
                    return Problem($"{ex.Message}", statusCode: StatusCodes.Status404NotFound);
               }
               catch (Exception ex)
               {
                    await logger.LogErrorAsync("Error getting basket for user: " + userName, ex);
                    return Problem("An error occurred while getting the basket.", statusCode: StatusCodes.Status500InternalServerError);
               }
          }

          [HttpPut]
          [ProducesResponseType(typeof(UpdateBasketResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          [ProducesResponseType(StatusCodes.Status401Unauthorized)]
          public async Task<IActionResult> UpdateBasket([FromBody] UpdateBasketRequest request)
          {
               try
               {
                                   await logger.LogInformationAsync("Validating the token of the header...");
               var isTokenValid = await tokenValidator.ValidateToken(HttpContext);
               if (!isTokenValid)
               {
                    await logger.LogErrorAsync("You are not authorized to access the endpoint", new UnauthorizedAccessException());
                    return Unauthorized("Invalid token.");
               }
                    var command = mapper.Map<UpdateBasketCommand>(request);
                    var result = await sender.Send(command);
                    var response = mapper.Map<UpdateBasketResponse>(result);

                    return Ok(response);
               }
               catch (Exception ex)
               {
                    await logger.LogErrorAsync("Error updating basket", ex);
                    return Problem("An error occurred while updating the basket.", statusCode: StatusCodes.Status500InternalServerError);
               }
          }

          [HttpDelete("{userName}")]
          [ProducesResponseType(typeof(DeleteBasketResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          [ProducesResponseType(StatusCodes.Status500InternalServerError)]
          public async Task<IActionResult> DeleteBasket(string userName)
          {
               try
               {
                                   await logger.LogInformationAsync("Validating the token of the header...");
               var isTokenValid = await tokenValidator.ValidateToken(HttpContext);
               if (!isTokenValid)
               {
                    await logger.LogErrorAsync("You are not authorized to access the endpoint", new UnauthorizedAccessException());
                    return Unauthorized("Invalid token.");
               }
                    await logger.LogInformationAsync("Deleting basket for user: " + userName);
                    var result = await sender.Send(new DeleteBasketCommand(userName));
                    var response = mapper.Map<DeleteBasketResponse>(result);

                    return Ok(response);
               }
               catch (Exception ex)
               {
                    await logger.LogErrorAsync("Error deleting basket for user: " + userName, ex);
                    return Problem("An error occurred while deleting the basket.", statusCode: StatusCodes.Status500InternalServerError);
               }
          }

          [HttpPost("checkout")]
          [ProducesResponseType(typeof(CheckOutBasketResponse), StatusCodes.Status201Created)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          [ProducesDefaultResponseType]
          //[Route("checkout")]
          public async Task<IActionResult> CheckoutAsync([FromBody] CheckOutBasketRequest request)
          {
                              await logger.LogInformationAsync("Validating the token of the header...");
               var isTokenValid = await tokenValidator.ValidateToken(HttpContext);
               if (!isTokenValid)
               {
                    await logger.LogErrorAsync("You are not authorized to access the endpoint", new UnauthorizedAccessException());
                    return Unauthorized("Invalid token.");
               }
               var command = mapper.Map<CheckOutBasketCommand>(request);
               var result = await sender.Send(command);
               var response = mapper.Map<CheckOutBasketResponse>(result);

               return CreatedAtAction(nameof(CheckoutAsync), response);
          }
     }
}

