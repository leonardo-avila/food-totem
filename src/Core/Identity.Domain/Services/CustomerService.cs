using Domain.Core;
using FluentValidation;
using Identity.Domain.Models;
using Identity.Domain.Models.Enums;
using Identity.Domain.Ports;

namespace Identity.Domain.Services
{
	public class CustomerService : ICustomerService
	{
		private readonly IValidator<Customer> _customerValidator;

		public CustomerService(IValidator<Customer> customerValidator)
		{
			_customerValidator = customerValidator;
		}

		public bool IsValidAuthenticationType(string authenticationType)
		{
			return Enum.IsDefined(typeof(AuthenticationTypeEnum), authenticationType);
		}

		public void ValidateCustomer(Customer customer)
		{
			var validationResult = _customerValidator.Validate(customer);

			if (!validationResult.IsValid)
			{
				throw new DomainException(validationResult.ToString());
			}
		}
	}
}

