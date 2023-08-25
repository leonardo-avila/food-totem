using Identity.Domain.Models;

namespace Identity.Domain.Ports
{
	public interface ICustomerService
	{
		bool IsValidAuthenticationType(string authenticationType);
		void ValidateCustomer(Customer customer);
	}
}

