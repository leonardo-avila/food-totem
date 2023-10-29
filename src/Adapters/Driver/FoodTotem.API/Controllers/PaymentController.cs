using FoodTotem.Demand.UseCase.OutputViewModels;
using FoodTotem.Demand.UseCase.Ports;
using FoodTotem.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IOrderUseCases _orderAppService;

        public PaymentController(ILogger<PaymentController> logger,
            IOrderUseCases orderAppService)
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
        [Authorize("Bearer")]
        public async Task<IActionResult> CheckOrderPaymentStatus(Guid orderId)
        {
            try
            {
                var order = await _orderAppService.GetOrder(orderId);
                return Ok($"Order {orderId} payment status is {order.PaymentStatus}");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred when getting order payment status");
            }
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
        [Authorize("Bearer")]
        public async Task<ActionResult<CheckoutOrderViewModel>> PayOrder(Guid orderId)
        {
            try
            {
                return Ok(await _orderAppService.ApproveOrderPayment(orderId));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred when updating the order");
            }
        }
        #endregion
    }
}
