using Identity.UseCase.InputViewModels;
using Identity.UseCase.OutputViewModels;

namespace Identity.UseCase.Ports
{
	public interface ICustomerUseCase
	{
		Task<CustomerOutputViewModel> AddCustomer(CustomerInputViewModel customer);
		Task<CustomerOutputViewModel> GetCustomerByCPF(string cpf);
		Task<CustomerOutputViewModel> GetCustomer(Guid id);
		Task<bool> DeleteCustomer(Guid id);
		Task<IEnumerable<CustomerOutputViewModel>> GetCustomers();
	}
}