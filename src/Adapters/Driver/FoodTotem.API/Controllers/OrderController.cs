using FoodTotem.Demand.UseCase.Ports;
using FoodTotem.Demand.UseCase.InputViewModels;
using FoodTotem.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using FoodTotem.Demand.UseCase.OutputViewModels;
using FoodTotem.Gateways.MercadoPago.ViewModels;
using FoodTotem.Gateways.MercadoPago;
using Microsoft.AspNetCore.Authorization;

namespace FoodTotem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderUseCases _orderUseCases;
        private readonly IMercadoPagoPaymentService _mercadoPagoPaymentService;

        public OrderController(ILogger<OrderController> logger,
            IOrderUseCases orderUseCases,
            IMercadoPagoPaymentService mercadoPagoPaymentService)
        {
            _logger = logger;
            _orderUseCases = orderUseCases;
            _mercadoPagoPaymentService = mercadoPagoPaymentService;
        }

        #region GET Endpoints
        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns>Returns all orders</returns>
        /// <response code="204">No orders found.</response>
        [HttpGet(Name = "Get Orders")]
        [Authorize("Bearer")]
        public async Task<ActionResult<IEnumerable<CheckoutOrderViewModel>>> GetOrders()
        {
            var orders = await _orderUseCases.GetOrders();
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
        [HttpGet("{id:Guid}", Name = "Get Order By Id")]
        [Authorize("Bearer")]
        public async Task<ActionResult<CheckoutOrderViewModel>> GetById(Guid id)
        {
            try
            {
                return Ok(await _orderUseCases.GetOrder(id));
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
        public async Task<ActionResult<IEnumerable<CheckoutOrderViewModel>>> GetQueuedOrders()
        {
            var queuedOrders = await _orderUseCases.GetQueuedOrders();
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
        public async Task<ActionResult<IEnumerable<CheckoutOrderViewModel>>> GetOngoingOrders()
        {
            var ongoingOrders = await _orderUseCases.GetOngoingOrders();
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
        public async Task<ActionResult<CheckoutOrderViewModel>> CheckoutOrder(OrderInputViewModel orderViewModel)
        {
            try
            {
                var checkoutOrder = await _orderUseCases.CheckoutOrder(orderViewModel);
                var paymentOrder = ProducePaymentInformationViewModel(checkoutOrder);
                var paymentQROrder = await _mercadoPagoPaymentService.GetPaymentQRCode(paymentOrder);

                checkoutOrder.QRCode = paymentQROrder.qr_data;
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

        private PaymentInformationViewModel ProducePaymentInformationViewModel(CheckoutOrderViewModel checkoutOrder)
        {
            var expiration = DateTimeOffset.Now.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ss.fffK");
            return new PaymentInformationViewModel()
            {
                expiration_date = $"{expiration}",
                total_amount = checkoutOrder.Total,
                external_reference = checkoutOrder.Id.ToString(),
                title = "Food Totem Order",
                items = ProducePaymentItemViewModelCollection(checkoutOrder.Combo),
                description = $"Food Totem Order{checkoutOrder.Id}"
            };
        }

        private IEnumerable<PaymentItemViewModel> ProducePaymentItemViewModelCollection(IEnumerable<CheckoutOrderFoodViewModel> orderFoods)
        {
            foreach (var orderFood in orderFoods)
            {
                yield return new PaymentItemViewModel()
                {
                    sku_number = orderFood.FoodId.ToString(),
                    unit_measure = "unit",
                    unit_price = orderFood.Price,
                    quantity = orderFood.Quantity,
                    total_amount = orderFood.Price * orderFood.Quantity,
                    title = "Food"
                };
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
        [HttpPut("{id:Guid}", Name = "Update order status")]
        [Authorize("Bearer")]
        public async Task<ActionResult<CheckoutOrderViewModel>> UpdateOrderStatus(Guid id, string newOrderStatus)
        {
            try
            {
                return Ok(await _orderUseCases.UpdateOrderStatus(id, newOrderStatus));
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
        [Authorize("Bearer")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            try
            {
                await _orderUseCases.DeleteOrder(id);
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