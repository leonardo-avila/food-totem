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

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>Returns all orders</returns>
        [HttpGet(Name = "Get Orders")]
        public IActionResult Get()
        {
            return Ok(_orderService.GetOrders());
        }

        /// <summary>
        /// Get order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the order with the specified id</returns>
        [HttpGet("{id:Guid}", Name = "Get Order By Id")]
        public IActionResult GetById(Guid id)
        {
            return Ok(_orderService.GetOrder(id));
        }

        [HttpGet("queued", Name = "Get queued orders")]
        public IActionResult GetQueuedOrders()
        {
            return Ok(_orderService.GetQueuedOrders());
        }

        
    }
}