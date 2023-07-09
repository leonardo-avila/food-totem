using Data.Core;
using Identity.Domain.Models;

namespace Identity.Application.Ports
{
	public interface ICustomerRepository : IRepository<Customer>
	{
		void Add(Customer customer);
		Task<Customer> GetCustomer(Guid id);
		Task<Customer> GetCustomerByCPF(string customerCPF);
		Task<IEnumerable<Customer>> GetCustomers();
		void Update(Customer customer);
		void Remove(Customer customer);

    }
}

