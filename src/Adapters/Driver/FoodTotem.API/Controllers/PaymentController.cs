using FoodTotem.Domain.Core;
using FoodTotem.Gateways.Payment.Services;
using FoodTotem.Gateways.Payment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;

        public PaymentController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        #region GET Endpoints
        /// <summary>
        /// Get all payments
        /// </summary>
        /// <returns>Returns all payments</returns>
        /// <response code="204">No payments found.</response>
        [HttpGet(Name = "Get Payments")]
        [Authorize("Bearer")]
        public async Task<ActionResult<IEnumerable<PaymentViewModel>>> GetPayments()
        {
            var payments = await _paymentServices.GetPayments();
            if (!payments.Any()) {

                return NoContent();
            }
            return Ok(payments);
        }
        #endregion

        #region POST Endpoints
        /// <summary>
        /// Update an payment of an order
        /// </summary>
        /// <param name="paymentUpdateViewModel">Represents the payment details to be updated</param>
        /// <returns>Returns 200 if the payment was succesfully updated and show details of the payment.</returns>
        /// <response code="500">Something wrong happened when updating payment. Could be internet connection or database error.</response>
        [HttpPost(Name = "Update payment")]
        [Authorize("Bearer")]
        public async Task<ActionResult<PaymentViewModel>> UpdatePaymentStatus(PaymentUpdateViewModel paymentUpdateViewModel)
        {
            try
            {   
                var payment = await _paymentServices.UpdatePaymentStatus(paymentUpdateViewModel);

                return Ok(payment);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating payment.");
            }
        }
        #endregion
    }
}