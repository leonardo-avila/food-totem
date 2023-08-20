using Identity.Application.Ports;
using Identity.Domain.Models;
using Identity.Domain.Services;

namespace Identity.Application.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly ICustomerRepository _customerRepository;

		public CustomerService(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public async Task<bool> AddCustomer(Customer customer)
		{
			return await _customerRepository.Create(customer);
		}

		public async Task<bool> DeleteCustomer(Guid id)
		{
			var customer = await _customerRepository.Get(id);
			return await _customerRepository.Delete(customer);
		}

		public async Task<Customer> GetCustomerByCPF(string cpf)
		{
			return await _customerRepository.GetCustomerByCPF(cpf);
		}
		public async Task<IEnumerable<Customer>> GetCustomers()
		{
			return await _customerRepository.GetAll();
		}

	}
}

