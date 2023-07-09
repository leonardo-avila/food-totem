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

        [HttpPost(Name = "Create customer")]
        public async Task<IActionResult> CreateCustomerByCPF(CustomerViewModel customerViewModel)
        {
            var customer = new Customer(Identity.Domain.Models.Enums.AuthenticationTypeEnum.CPF, customerViewModel.CPF);
            var validationResult = _customerValidator.Validate(customer);

            if (validationResult.IsValid)
            {
                var successful = await _customerService.AddCustomer(customer);
                if (successful) return Ok("Customer added succesfuly");
                return BadRequest("An error occurred while adding customer");
            }
            return BadRequest(validationResult.ToString());
        }

        [HttpDelete("{id:Guid}", Name = "Delete a customer")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var successful = await _customerService.DeleteCustomer(id);

            if (successful)
                return Ok("Customer deleted");

            return NotFound("Could not found customer with the specified id");
        }

        [HttpGet("{cpf}", Name = "Get customer by CPF")]
        public async Task<IActionResult> GetCustomerByCPF(string cpf)
        {
            var customer = await _customerService.GetCustomerByCPF(cpf);
            if (customer is null) return NotFound("Could not found customer with the specified id");

            return Ok(customer);
        }

        [HttpGet(Name = "Get all customers")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _customerService.GetCustomers();
            if (customers is null || customers.Count() == 0)
            {
                Response.StatusCode = StatusCodes.Status204NoContent;
                return new List<Customer>();
            }
            return Ok(customers);
        }
    }
}
