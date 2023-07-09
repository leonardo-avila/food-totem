using Demand.Domain.Models.Enums;
using Demand.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IOrderService _orderService;

        public PaymentController(ILogger<PaymentController> logger,
            IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        #region PUT Endpoints
        /// <summary>
        /// Execute the payment for the order specified by Id
        /// </summary>
        /// <param name="orderId">Represents the order id that should be updated with payment</param>
        /// <returns>Returns the order with the payment status and orders status updated.</returns>
        /// <response code="404">No order with the specified id was found.</response>
        /// <response code="500">Something wrong happened when updating food. Could be internet connection or database connection.</response>
        [HttpPut("{orderId:Guid}", Name = "Pay order")]
        public async Task<IActionResult> PayOrder(Guid orderId)
        {
            var order = await _orderService.GetOrder(orderId);
            if (order is null) return NotFound("Order not found by the specified id");
            order.SetPaymentStatus(PaymentStatusEnum.Approved);
            order.SetOrderStatus(OrderStatusEnum.Preparing);
            var successful = await _orderService.UpdateOrder(order);
            if (successful) return Ok(order);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred when updating the order");
        }
        #endregion
    }
}
