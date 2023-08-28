using FoodTotem.Identity.Domain.Models;

namespace FoodTotem.Identity.Domain.Ports
{
	public interface ICustomerService
	{
		bool IsValidAuthenticationType(string authenticationType);
		void ValidateCustomer(Customer customer);
		bool IsValidCPF(string cpf);
	}
}

