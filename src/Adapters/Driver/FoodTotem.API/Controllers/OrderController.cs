using Demand.Application.ViewModels;
using Demand.Domain.Models;
using Demand.Domain.Services;
using FluentValidation;
using Identity.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IValidator<Order> _orderValidator;

        public OrderController(ILogger<OrderController> logger,
            IOrderService orderService,
            IValidator<Order> orderValidator)
        {
            _logger = logger;
            _orderService = orderService;
            _orderValidator = orderValidator;
        }

        #region GET Endpoints
        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>Returns all orders</returns>
        /// <response code="204">No orders found.</response>
        [HttpGet(Name = "Get Orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetOrders();
            if (orders.Count() == 0) {

                return NoContent();
            }
            return Ok(orders);
        }

        /// <summary>
        /// Get order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the order with the specified id</returns>
        /// <response code="204">No order with the specified id was found.</response>
        [HttpGet("{id:Guid}", Name = "Get Order By Id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _orderService.GetOrder(id));
        }

        /// <summary>
        /// Get all orders on the kitchen queue
        /// </summary>
        /// <returns>Returns all orders with status Preparing</returns>
        /// <response code="204">No orders found on the kitchen queue.</response>
        [HttpGet("queued", Name = "Get queued orders")]
        public async Task<IActionResult> GetQueuedOrders()
        {
            return Ok(await _orderService.GetQueuedOrders());
        }
        #endregion

        #region POST Endpoints
        /// <summary>
        /// Add an order
        /// </summary>
        /// <param name="orderViewModel">Represents the order details to be added</param>
        /// <returns>Returns 200 if the order was succesfully added.</returns>
        /// <response code="400">Order in invalid format. Model validations errors should be prompted when necessary.</response>
        /// <response code="500">Something wrong happened when adding order. Could be internet connection or database error.</response>
        [HttpPost(Name = "Add new order")]
        public async Task<IActionResult> AddOrder(OrderViewModel orderViewModel)
        {
            var order = new Order(orderViewModel.Customer);
            var combo = new List<OrderFood>();
            foreach (var food in orderViewModel.Combo)
            {
                combo.Add(new OrderFood(food.FoodId, food.Quantity));
            }
            order.SetCombo(combo);
            var validationResult = _orderValidator.Validate(order);
            if (validationResult.IsValid)
            {
                var successful = await _orderService.AddOrder(order);
                if (successful) return Ok("Order added successfully.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding order.");
            }
            return BadRequest(validationResult.ToString());
        }
        #endregion

        #region DELETE Endpoints
        /// <summary>
        /// Delete an order with the specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns 200 when successful</returns>
        /// <response code="404">No order with the specified id was found.</response>
        /// <response code="500">Something wrong happened when deleting order. Could be internet connection or database error.</response>
        [HttpDelete("{id:Guid}", Name = "Delete a order")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _orderService.GetOrder(id);
            if (order is null) return NotFound("Could not found order with the specified id");
            var successful = await _orderService.DeleteOrder(order);
            if (successful) return Ok("Order deleted successfully");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting order.");
        }
        #endregion
    }
}