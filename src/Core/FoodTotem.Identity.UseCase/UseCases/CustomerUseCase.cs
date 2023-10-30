using FoodTotem.Identity.UseCase.Ports;
using FoodTotem.Identity.Domain.Models;
using FoodTotem.Identity.UseCase.OutputViewModels;
using FoodTotem.Identity.UseCase.InputViewModels;
using FoodTotem.Identity.Domain.Models.Enums;
using FoodTotem.Domain.Core;
using FoodTotem.Identity.Domain.Ports;

namespace FoodTotem.Identity.UseCase.UseCases
{
	public class CustomerUseCase : ICustomerUseCase
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly ICustomerService _customerService;

		public CustomerUseCase(ICustomerRepository customerRepository,
			ICustomerService customerService)
		{
			_customerRepository = customerRepository;
			_customerService = customerService;
		}

		public async Task<CustomerOutputViewModel> GetCustomer(Guid id)
		{
			var customer = await _customerRepository.Get(id);

			if (customer is null)
			{
				throw new DomainException("No customer found for this id");
			}

			return ProduceCustomerViewModel(customer);
		}

		public async Task<CustomerOutputViewModel> AddCustomer(CustomerInputViewModel customerInput)
		{
			if (_customerService.IsValidAuthenticationType(customerInput.AuthenticationType))
			{
				var customer = new Customer(Enum.Parse<AuthenticationTypeEnum>(customerInput.AuthenticationType),
					customerInput.Identification);

				_customerService.ValidateCustomer(customer);

				var customerExists = await _customerRepository.GetCustomerByCPF(customerInput.Identification) is not null;

				if (customerExists) throw new DomainException("Customer already exists.");

				await _customerRepository.Create(customer);

				return ProduceCustomerViewModel(customer);
			}
			else
			{
				throw new DomainException("Invalid authentication type.");
			}
        }

		public async Task<bool> DeleteCustomer(Guid id)
		{
			var customer = await _customerRepository.Get(id);

			return customer is null
                ? throw new DomainException("No customer found with this id.")
                : await _customerRepository.Delete(customer);
		}

		public async Task<CustomerOutputViewModel> GetCustomerByCPF(string cpf)
		{
			if (!_customerService.IsValidCPF(cpf)) throw new DomainException("Invalid CPF.");

			var customer = await _customerRepository.GetCustomerByCPF(cpf);

			return customer is null
				? throw new DomainException("No customer found with this id.")
				: ProduceCustomerViewModel(customer);
		}

		public async Task<IEnumerable<CustomerOutputViewModel>> GetCustomers()
		{
			var customers = await _customerRepository.GetAll();

			return ProduceCustomerViewModelCollection(customers);

		}

		private static IEnumerable<CustomerOutputViewModel> ProduceCustomerViewModelCollection(IEnumerable<Customer> customers)
		{
			foreach (var customer in customers)
			{
				yield return ProduceCustomerViewModel(customer);
			}
		}

		private static CustomerOutputViewModel ProduceCustomerViewModel(Customer customer)
		{
            return new CustomerOutputViewModel()
            {
								Id = customer.Id,
                Identification = customer.ToString(),
                AuthenticationType = customer.AuthenticationType.ToString()
            };
        }
	}
}

