using Demand.Application.ViewModels;
using Demand.Domain.Models;
using Demand.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpPost(Name = "Add new order")]
        public async Task<IActionResult> AddOrder(OrderViewModel orderViewModel)
        {
            var order = new Order(orderViewModel.Customer);
            order.SetCombo(orderViewModel.Combo);
            var validationResult = _orderValidator.Validate(order);
            if (validationResult.IsValid)
            {
                var successful = await _orderService.AddOrder(order);
                if (successful) return Ok("Order added successfully.");
                return BadRequest("An error occurred while adding order.");
            }
            return BadRequest(validationResult.ToString());
        }
    }
}