using FluentValidation;
using Identity.Application.ViewModels;
using Identity.Domain.Models;
using Identity.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodTotem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IValidator<Customer> _customerValidator;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger,
            ICustomerService customerService,
            IValidator<Customer> customerValidator)
        {
            _logger = logger;
            _customerService = customerService;
            _customerValidator = customerValidator;
        }

        #region GET Endpoints
        /// <summary>
        /// Get the customer with the specified CPF
        /// </summary>
        /// <param name="cpf">Represents the CPF of the customer</param>
        /// <returns>Returns the customer with the specified CPF</returns>
        /// <response code="204">No customer found with the specified CPF.</response>
        [HttpGet("{cpf}", Name = "Get customer by CPF")]
        public async Task<IActionResult> GetCustomerByCPF(string cpf)
        {
            var customer = await _customerService.GetCustomerByCPF(cpf);
            if (customer is null) return NoContent();

            return Ok(customer);
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>Returns all customers registered.</returns>
        /// <response code="204">No customer found on the database.</response>
        [HttpGet(Name = "Get all customers")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _customerService.GetCustomers();
            if (customers is null || customers.Count() == 0)
            {
                return NoContent();
            }
            return Ok(customers);
        }
        #endregion

        #region POST Endpoints
        /// <summary>
        /// Add a customer with an specified CPF
        /// </summary>
        /// <param name="customerViewModel">Represents the customer that should be added</param>
        /// <returns>Return 200 when successfully added customer.</returns>
        /// <response code="400">Customer not in valid format. Model validation errors will be prompted.</response>
        /// <response code="500">Something wrong happened when adding customer. Could be internet connection or database error.</response>
        [HttpPost(Name = "Create customer")]
        public async Task<IActionResult> CreateCustomerByCPF(CustomerViewModel customerViewModel)
        {
            var customer = new Customer(Identity.Domain.Models.Enums.AuthenticationTypeEnum.CPF, customerViewModel.CPF);
            var validationResult = _customerValidator.Validate(customer);

            if (validationResult.IsValid)
            {
                var successful = await _customerService.AddCustomer(customer);
                if (successful) return Ok("Customer added succesfuly");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding customer");
            }
            return BadRequest(validationResult.ToString());
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
            var successful = await _customerService.DeleteCustomer(id);

            if (successful)
                return Ok("Customer deleted");

            return NotFound("Could not found customer with the specified id");
        }
        #endregion
    }
}
