using FoodTotem.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FoodTotem.Gateways.Demand.ViewModels;
using FoodTotem.Gateways.Payment.Services;
using FoodTotem.Gateways.Demand.Services;

namespace FoodTotem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IPaymentServices _paymentServices;
        private readonly IDemandServices _demandServices;

        public OrderController(ILogger<OrderController> logger, IPaymentServices paymentServices, IDemandServices demandServices)
        {
            _logger = logger;
            _paymentServices = paymentServices;
            _demandServices = demandServices;
        }

        #region GET Endpoints
        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>Returns all orders</returns>
        /// <response code="204">No orders found.</response>
        [HttpGet(Name = "Get Orders")]
        [Authorize("Bearer")]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetOrders()
        {
            var orders = await _demandServices.GetOrders();
            if (!orders.Any()) {

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
        [HttpGet("{id}", Name = "Get Order By Id")]
        [Authorize("Bearer")]
        public async Task<ActionResult<OrderViewModel>> GetById(string id)
        {
            try
            {
                return Ok(await _demandServices.GetOrder(id));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving order.");
            }
        }

        /// <summary>
        /// Get all orders on the kitchen queue
        /// </summary>
        /// <returns>Returns all orders with status Preparing</returns>
        /// <response code="204">No orders found on the kitchen queue.</response>
        [HttpGet("queued", Name = "Get queued orders")]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetQueuedOrders()
        {
            var queuedOrders = await _demandServices.GetQueuedOrders();
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
        [Authorize("Bearer")]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetOngoingOrders()
        {
            var ongoingOrders = await _demandServices.GetOngoingOrders();
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
        [Authorize("Bearer")]
        public async Task<ActionResult<OrderViewModel>> CheckoutOrder(CreateOrderViewModel orderViewModel)
        {
            try
            {   
                var checkoutOrder = await _demandServices.CreateOrder(orderViewModel);

                return Ok(checkoutOrder);
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
        /// Update an order status. Available statuses: Received, Preparing, Ready, Completed
        /// </summary>
        /// <param name="id">Represents the order id</param>
        /// <param name="newOrderStatus">Represents the order status be setted</param>
        /// <response code="400">Order status invalid format.</response>
        /// <response code="500">Something wrong happened when adding order. Could be internet connection or database error.</response>
        [HttpPut("{id}", Name = "Update order status")]
        [Authorize("Bearer")]
        public async Task<ActionResult<OrderViewModel>> UpdateOrderStatus(string id, string newOrderStatus)
        {
            try
            {
                return Ok(await _demandServices.UpdateOrderStatus(id, newOrderStatus));
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
        [HttpDelete("{id}", Name = "Delete a order")]
        [Authorize("Bearer")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            try
            {
                await _demandServices.DeleteOrder(id);
                return Ok("Order deleted successfully");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting order.");
            }
        }
        #endregion
    }
}