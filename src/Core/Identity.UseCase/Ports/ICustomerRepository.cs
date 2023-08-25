using Data.Core;
using Identity.Domain.Models;

namespace Identity.UseCase.Ports
{
	public interface ICustomerRepository : IRepository<Customer>
	{
		Task<Customer> GetCustomerByCPF(string customerCPF);
    }
}

