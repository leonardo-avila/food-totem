using Data.Core;
using Identity.Domain.Models;

namespace Identity.Application.Ports
{
	public interface ICustomerRepository : IRepository<Customer>
	{
		Task<Customer> GetCustomerByCPF(string customerCPF);
    }
}

