using Demand.Application.Ports;
using Demand.Application.ViewModels;
using Demand.Domain.Models;
using Domain.Core;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderAppService _orderAppService;

        public OrderController(ILogger<OrderController> logger,
            IOrderAppService orderAppService)
        {
            _logger = logger;
            _orderAppService = orderAppService;
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
            var orders = await _orderAppService.GetOrders();
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
            var order = await _orderAppService.GetOrder(id);
            if (order is null) return NotFound();
            return Ok(order);
        }

        /// <summary>
        /// Get all orders on the kitchen queue
        /// </summary>
        /// <returns>Returns all orders with status Preparing</returns>
        /// <response code="204">No orders found on the kitchen queue.</response>
        [HttpGet("queued", Name = "Get queued orders")]
        public async Task<IActionResult> GetQueuedOrders()
        {
            var queuedOrders = await _orderAppService.GetQueuedOrders();
            if (!queuedOrders.Any())
            {
                return NoContent();
            }
            return Ok(queuedOrders);
        }

        /// <summary>
        /// Get all ongoing orders ranked by date and status.
        /// </summary>
        /// <returns>Return all orders in course.</returns>
        /// <response code="204">No orders found for the specified id.</response>
        [HttpGet("ongoing", Name = "Get orders in progress")]
        public async Task<IActionResult> GetOngoingOrders()
        {
            var ongoingOrders = await _orderAppService.GetOngoingOrders();
            if (!ongoingOrders.Any())
            {
                return NoContent();
            }
            return Ok(ongoingOrders);
        }
        #endregion

        #region POST Endpoints
        /// <summary>
        /// Checkout an order
        /// </summary>
        /// <param name="orderViewModel">Represents the order details to be added</param>
        /// <returns>Returns 200 if the order was succesfully added and show details of the order.</returns>
        /// <response code="400">Order in invalid format. Model validations errors should be prompted when necessary.</response>
        /// <response code="500">Something wrong happened when adding order. Could be internet connection or database error.</response>
        [HttpPost(Name = "Checkout order")]
        public async Task<IActionResult> CheckoutOrder(OrderViewModel orderViewModel)
        {
            try
            {
                return Ok(await _orderAppService.CheckoutOrder(orderViewModel));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding order.");
            }
        }
        #endregion

        #region PUT Endpoints
        /// <summary>
        /// Update an order status
        /// </summary>
        /// <param name="newOrderStatus">Represents the order status be setted</param>
        /// <response code="400">Order status invalid format.</response>
        /// <response code="500">Something wrong happened when adding order. Could be internet connection or database error.</response>
        [HttpPut("{id:Guid}", Name = "Update order status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, string newOrderStatus)
        {
            try
            {
                var order = await _orderAppService.GetOrder(id);
                if (order is null) return NotFound();
                return Ok(await _orderAppService.UpdateOrderStatus(order, newOrderStatus));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating order status.");
            }
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
            var order = await _orderAppService.GetOrder(id);
            if (order is null)
            {
                return NotFound("Could not found order with the specified id");
            }

            var successful = await _orderAppService.DeleteOrder(order);
            if (successful)
            {
                return Ok("Order deleted successfully");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting order.");
        }
        #endregion
    }
}