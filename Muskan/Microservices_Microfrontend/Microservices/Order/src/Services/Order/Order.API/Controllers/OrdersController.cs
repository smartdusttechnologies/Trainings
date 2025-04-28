using Microsoft.AspNetCore.Mvc;
using Ordering.Application.DTOs.Create;
using Ordering.Application.DTOs.Delete;
using Ordering.Application.DTOs.Get;
using Ordering.Application.DTOs.Update;
using Ordering.Application.Order.Commands.CreateOrder;
using Ordering.Application.Order.Commands.DeleteOrder;
using Ordering.Application.Order.Commands.UpdateOrder;
using Ordering.Application.Order.Queries.GetOrderByCustomer;
using Ordering.Application.Order.Queries.GetOrderByName;
using Ordering.Application.Order.Queries.GetOrders;
using Ordering.Application.Services;

namespace Ordering.API.Controllers
{
     [ApiController]
     [Route("orders")]
     public class OrdersController : ControllerBase
     {
          private readonly ISender _sender;
          private readonly IMapper _mapper;
          private readonly ILoggingService<OrdersController> _logger;

          public OrdersController(ISender sender, IMapper mapper, ILoggingService<OrdersController> logger)
          {
               _sender = sender;
               _mapper = mapper;
               _logger = logger;
          }

          /// <summary>
          /// Create Order
          /// </summary>
          /// <param name="request">CreateOrderRequest</param>
          /// <returns>201 Created with Order Id</returns>
          [HttpPost]
          [ProducesResponseType(typeof(CreateOrderResponse), StatusCodes.Status201Created)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
          {
               await _logger.LogInformationAsync($"Create Order Endpoint hit: {request}");

               var command = _mapper.Map<CreateOrderCommand>(request);
               var result = await _sender.Send(command);
               var response = new CreateOrderResponse(result.Id);               // assuming `result` is Guid

               return StatusCode(StatusCodes.Status201Created, response);
          }
          /// <summary>
          /// Get paginated list of orders
          /// </summary>
          /// <param name="request">Pagination and filter parameters</param>
          /// <returns>200 OK with paginated list of orders</returns>
          [HttpGet]
          //[Authorize] // Replace with [RequireCustomAuth] if you have a custom attribute
          [ProducesResponseType(typeof(GetOrderResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          public async Task<IActionResult> GetOrders([FromQuery] PaginateRequest request)
          {
               await _logger.LogInformationAsync($"Get Order endpoint hit : {request}");

               var result = await _sender.Send(new GetOrdersQuery(request));

               var response = new GetOrderResponse(result.PaginateRequest);

               return Ok(response);
          }
          /// <summary>
          /// Gets all orders placed by a specific customer
          /// </summary>
          /// <param name="customerID">The ID of the customer</param>
          /// <returns>List of Orders</returns>
          [HttpGet("customer/{customerID}")]
          //[Authorize] // Replace with [RequireCustomAuth] if using a custom policy
          [ProducesResponseType(typeof(GetOrderByCustomerResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          public async Task<IActionResult> GetOrdersByCustomer(Guid customerID)
          {
               await _logger.LogInformationAsync($"Get Order by Customer ID called: {customerID}");

               var result = await _sender.Send(new GetOrderByCustomerQuery(customerID));

               var response = _mapper.Map<GetOrderByCustomerResponse>(result);

               return Ok(response);
          }

          /// <summary>
          /// Get orders by order name.
          /// </summary>
          [HttpGet("name/{orderName}")]
          [ProducesResponseType(typeof(GetOrderByNameResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          public async Task<IActionResult> GetOrderByName(string orderName)
          {
               await _logger.LogInformationAsync($"GetOrderByName is called: {orderName}");

               var result = await _sender.Send(new GetOrderByNameQuery(orderName));

               if (result.Orders == null || !result.Orders.Any())
                    return NotFound();

               var mappedOrders = _mapper.Map<IEnumerable<OrdersDTO>>(result.Orders);
               var response = new GetOrderByNameResponse(mappedOrders);

               return Ok(response);
          }
          /// <summary>
          /// Update an order.
          /// </summary>
          [HttpPut]
          [ProducesResponseType(typeof(UpdateOrderResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
          {
               await _logger.LogInformationAsync($"Update order request received for ID: {request.Order.Id}");

               var command = _mapper.Map<UpdateOrderCommand>(request);
               var result = await _sender.Send(command);

               var response = _mapper.Map<UpdateOrderResponse>(result);
               return Ok(response);
          }
          /// <summary>
          /// Delete Order
          /// </summary>
          /// <param name="id">Order Id</param>
          /// <returns>200 OK with success status</returns>
          [HttpDelete("{id}")]
          //[Authorize] // or your custom [RequireCustomAuth]
          [ProducesResponseType(typeof(DeleteOrderResponse), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          public async Task<IActionResult> DeleteOrder(Guid id)
          {
               await _logger.LogInformationAsync($"Delete Order Endpoint hit : {id}");

               var result = await _sender.Send(new DeleteOrderCommand(id));

               var response = new DeleteOrderResponse(result.isSuccess);

               return Ok(response);
          }
     }
}


