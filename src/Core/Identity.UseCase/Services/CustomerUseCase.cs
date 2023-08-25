using Identity.UseCase.Ports;
using Identity.Domain.Models;
using Identity.UseCase.OutputViewModels;
using Identity.UseCase.InputViewModels;
using Identity.Domain.Models.Enums;
using Domain.Core;
using Identity.Domain.Ports;

namespace Identity.UseCase.Services
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
			return await _customerRepository.Delete(customer);
		}

		public async Task<CustomerOutputViewModel> GetCustomerByCPF(string cpf)
		{
			var customer = await _customerRepository.GetCustomerByCPF(cpf);

			return new CustomerOutputViewModel()
			{
				AuthenticationType = customer.AuthenticationType.ToString(),
				Identification = customer.CPF!
			};
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
                Identification = customer.ToString(),
                AuthenticationType = customer.AuthenticationType.ToString()
            };
        }
	}
}

