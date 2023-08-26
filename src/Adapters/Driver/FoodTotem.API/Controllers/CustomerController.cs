using Identity.UseCase.Ports;
using Identity.UseCase.InputViewModels;
using Microsoft.AspNetCore.Mvc;
using Domain.Core;
using Identity.UseCase.OutputViewModels;

namespace FoodTotem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerUseCase _customerUseCase;

        public CustomerController(ILogger<CustomerController> logger,
            ICustomerUseCase customerUseCase)
        {
            _logger = logger;
            _customerUseCase = customerUseCase;
        }

        #region GET Endpoints
        /// <summary>
        /// Get the customer with the specified CPF
        /// </summary>
        /// <param name="cpf">Represents the CPF of the customer</param>
        /// <returns>Returns the customer with the specified CPF</returns>
        /// <response code="204">No customer found with the specified CPF.</response>
        [HttpGet("{cpf}", Name = "Get customer by CPF")]
        public async Task<ActionResult<CustomerOutputViewModel>> GetCustomerByCPF(string cpf)
        {
            try
            {
                return Ok(await _customerUseCase.GetCustomerByCPF(cpf));
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something wrong occurred while trying to retrieve customer.");
            }
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>Returns all customers registered.</returns>
        /// <response code="204">No customer found on the database.</response>
        [HttpGet(Name = "Get all customers")]
        public async Task<ActionResult<IEnumerable<CustomerOutputViewModel>>> GetCustomers()
        {
            var customers = await _customerUseCase.GetCustomers();

            if (customers is null || !customers.Any())
            {
                return NoContent();
            }

            return Ok(customers);
        }
        #endregion

        #region POST Endpoints
        /// <summary>
        /// Add a customer with an specified CPF. Valid authenticationTypes: CPF
        /// </summary>
        /// <param name="customerViewModel">Represents the customer that should be added</param>
        /// <returns>Return 200 when successfully added customer.</returns>
        /// <response code="400">Customer not in valid format. Model validation errors will be prompted.</response>
        /// <response code="500">Something wrong happened when adding customer. Could be internet connection or database error.</response>
        [HttpPost(Name = "Create customer")]
        public async Task<ActionResult<CustomerOutputViewModel>> CreateCustomerByCPF(CustomerInputViewModel customerViewModel)
        {
            try
            {
                var customer = await _customerUseCase.AddCustomer(customerViewModel);

                return Ok(customer);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding customer");
            }
        }       
        #endregion

        #region DELETE Endpoints
        /// <summary>
        /// Delete the customer with the specified id
        /// </summary>
        /// <param name="id">Represents the id of the customer that should be removed</param>
        /// <returns>Returns 200 when successfully deleted customer.</returns>
        /// <response code="404">No customer found with the specified id.</response>
        [HttpDelete("{id:Guid}", Name = "Delete a customer")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                await _customerUseCase.DeleteCustomer(id);

                return Ok("Customer deleted");
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting customer");
            }
        }
        #endregion
    }
}
