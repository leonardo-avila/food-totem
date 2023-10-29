using FoodTotem.Identity.UseCase.Ports;
using FoodTotem.Identity.UseCase.InputViewModels;
using Microsoft.AspNetCore.Mvc;
using FoodTotem.Domain.Core;
using FoodTotem.Identity.UseCase.OutputViewModels;
using Microsoft.AspNetCore.Authorization;
using FoodTotem.Gateways.Http;

namespace FoodTotem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerUseCase _customerUseCase;
        private readonly IHttpHandler _httpHandler;
        private readonly IConfiguration _configuration;

        public CustomerController(ILogger<CustomerController> logger,
            ICustomerUseCase customerUseCase,
            IHttpHandler httpHandler,
            IConfiguration configuration)
        {
            _logger = logger;
            _customerUseCase = customerUseCase;
            _httpHandler = httpHandler;
            _configuration = configuration;
        }

        #region GET Endpoints
        /// <summary>
        /// Get the customer with the specified CPF
        /// </summary>
        /// <param name="cpf">Represents the CPF of the customer</param>
        /// <returns>Returns the customer with the specified CPF</returns>
        /// <response code="204">No customer found with the specified CPF.</response>
        [HttpGet("{cpf}", Name = "Get customer by CPF")]
        [Authorize("Bearer")]
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
        [Authorize("Bearer")]
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

        /// <summary>
        /// Get customer token to authorize requests
        /// </summary>
        /// <param name="customerInputViewModel">Customer CPF</param>
        /// <returns>Returns the JWT token to authorize requests</returns>
        /// /// <response code="400">Customer not in valid format. Model validation errors will be prompted.</response>
        /// <response code="500">Something wrong happened when getting customer token. Could be internet connection or database error.</response>
        [HttpPost("auth", Name = "Get customer token")]
        public async Task<ActionResult<CustomerTokenOutputViewModel>> GetCustomerToken(CustomerAuthInputViewModel customerInputViewModel)
        {
            try
            {
                var customer = await _customerUseCase.GetCustomerByCPF(customerInputViewModel.CPF);
                CustomerTokenOutputViewModel customerToken = await _httpHandler.PostAsync<CustomerAuthInputViewModel, CustomerTokenOutputViewModel>(_configuration["AuthenticationUrl"], customerInputViewModel, null);

                return Ok(customerToken);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting customer token.");
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
        [Authorize("Bearer")]
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
