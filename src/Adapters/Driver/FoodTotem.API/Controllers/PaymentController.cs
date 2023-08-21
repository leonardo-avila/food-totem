using Demand.Application.Ports;
using Demand.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IOrderAppService _orderAppService;

        public PaymentController(ILogger<PaymentController> logger,
            IOrderAppService orderAppService)
        {
            _logger = logger;
            _orderAppService = orderAppService;
        }

        #region GET Endpoints
        /// <summary>
        /// Check the payment status for an order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns>Return the order payment status</returns>
        /// <response code="404">No order with the specified id was found.</response>
        [HttpGet("{orderId:Guid}", Name = "Check order payment status")]
        public async Task<IActionResult> CheckOrderPaymentStatus(Guid orderId)
        {
            var order = await _orderAppService.GetOrder(orderId);
            if (order is null)
            {
                return NotFound("Order not found by the specified id");
            }
            return Ok($"Order {orderId} payment status is {order.PaymentStatus}");
        }
        #endregion

        #region PUT Endpoints
        /// <summary>
        /// Execute the payment for the order specified by Id
        /// </summary>
        /// <param name="orderId">Represents the order id that should be updated with payment</param>
        /// <returns>Returns the order with the payment status and orders status updated.</returns>
        /// <response code="404">No order with the specified id was found.</response>
        /// <response code="500">Something wrong happened when updating order payment. Could be internet connection or database connection.</response>
        [HttpPut("{orderId:Guid}", Name = "Pay order")]
        public async Task<IActionResult> PayOrder(Guid orderId)
        {
            var order = await _orderAppService.GetOrder(orderId);
            if (order is null)
            {
                return NotFound("Order not found by the specified id");
            }
            
            var successful = await _orderAppService.ApproveOrderPayment(order);
            if (successful)
            {
                return Ok(order);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred when updating the order");
        }
        #endregion
    }
}
