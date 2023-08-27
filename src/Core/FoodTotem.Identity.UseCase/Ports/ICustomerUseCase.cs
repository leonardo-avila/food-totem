using FoodTotem.Identity.UseCase.InputViewModels;
using FoodTotem.Identity.UseCase.OutputViewModels;

namespace FoodTotem.Identity.UseCase.Ports
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