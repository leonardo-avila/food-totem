using Demand.Domain.Models.Enums;
using Demand.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [Route("[controller]")]
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

        [HttpPut("{orderId:Guid}", Name = "Pay order")]
        public async Task<IActionResult> PayOrder(Guid orderId)
        {
            var order = await _orderService.GetOrder(orderId);
            if (order is null) return NotFound("Order not found by the specified id");
            order.SetOrderStatus(OrderStatusEnum.Preparing);
            var successful = await _orderService.UpdateOrder(order);
            if (successful) return Ok(order);
            return BadRequest("An error occurred when updating the order");
        }
    }
}
