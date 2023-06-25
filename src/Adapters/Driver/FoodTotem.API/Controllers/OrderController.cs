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

        [HttpGet(Name = "orders")]
        public IActionResult Get()
        {
            return Ok(_orderService.GetOrders());
        }

        [HttpGet(Name = "orders/{id}")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_orderService.GetOrder(id));
        }
    }
}