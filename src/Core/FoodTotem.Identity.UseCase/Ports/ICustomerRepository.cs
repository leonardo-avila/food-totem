using FoodTotem.Data.Core;
using FoodTotem.Identity.Domain.Models;

namespace FoodTotem.Identity.UseCase.Ports
{
	public interface ICustomerRepository : IRepository<Customer>
	{
		Task<Customer> GetCustomerByCPF(string customerCPF);
    }
}

