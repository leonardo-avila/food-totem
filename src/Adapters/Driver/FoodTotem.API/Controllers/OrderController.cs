using Demand.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger,
            IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet(Name = "GetOrders")]
        public IActionResult Get()
        {
            return Ok(_orderService.GetOrders());
        }
    }
}