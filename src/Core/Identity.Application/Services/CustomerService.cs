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
			_customerRepository.Add(customer);
			return await _customerRepository.UnitOfWork.Commit();
		}

		public async Task<bool> DeleteCustomer(Guid id)
		{
			var customer = await _customerRepository.GetCustomer(id);
			_customerRepository.Remove(customer);
			return await _customerRepository.UnitOfWork.Commit();
		}

		public async Task<Customer> GetCustomerByCPF(string cpf)
		{
			return await _customerRepository.GetCustomerByCPF(cpf);
		}
		public async Task<IEnumerable<Customer>> GetCustomers()
		{
			return await _customerRepository.GetCustomers();
		}

	}
}

