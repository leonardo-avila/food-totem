using Identity.Domain.Models;

namespace Identity.Domain.Services
{
	public interface ICustomerService
	{
		Task<bool> AddCustomer(Customer customer);
		Task<Customer> GetCustomerByCPF(string cpf);
		Task<bool> DeleteCustomer(Guid id);
	}
}

