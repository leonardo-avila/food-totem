using Demand.Application.ViewModels;
using Demand.Domain.Models;
using Demand.Domain.Services;
using FluentValidation;
using Identity.Domain.Services;
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
        public async Task<IActionResult> Get()
        {
            return Ok(await _orderService.GetOrders());
        }

        /// <summary>
        /// Get order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the order with the specified id</returns>
        [HttpGet("{id:Guid}", Name = "Get Order By Id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _orderService.GetOrder(id));
        }

        [HttpGet("queued", Name = "Get queued orders")]
        public async Task<IActionResult> GetQueuedOrders()
        {
            return Ok(await _orderService.GetQueuedOrders());
        }

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
                return BadRequest("An error occurred while adding order.");
            }
            return BadRequest(validationResult.ToString());
        }

        [HttpDelete("{id:Guid}", Name = "Delete a order")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _orderService.GetOrder(id);
            if (order is null) return NotFound("Could not found order with the specified id");
            var successful = await _orderService.DeleteOrder(order);
            if (successful) return Ok("Order deleted successfully");
            return BadRequest("An error occurred while deleting order.");
        }
    }
}