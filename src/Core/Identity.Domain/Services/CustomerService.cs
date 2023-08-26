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

		public bool IsValidCPF(string cpf)
		{
            if (string.IsNullOrEmpty(cpf)) return false;

            string value = cpf.Replace(".", "");
            value = value.Replace("-", "");

            if (value.Length != 11)
            {
                return false;
            }

            bool equal = true;

            for (int i = 1; i < 11 && equal; i++)
            {
                if (value[i] != value[0])
                {
                    equal = false;
                }
            }

            if (equal || value == "12345678909")
            {
                return false;
            }

            int[] numbers = new int[11];

            for (int i = 0; i < 11; i++)
            {
                numbers[i] = int.Parse(value[i].ToString());
            }

            int sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += (10 - i) * numbers[i];
            }

            int result = sum % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[9] != 0)
                {
                    return false;
                }
            }

            else if (numbers[9] != 11 - result)
            {
                return false;
            }

            sum = 0;

            for (int i = 0; i < 10; i++)
            {
                sum += (11 - i) * numbers[i];
            }

            result = sum % 11;

            if (result == 1 || result == 0)
            {
                if (numbers[10] != 0)
                {
                    return false;
                }
            }

            else
            {
                if (numbers[10] != 11 - result)
                {
                    return false;
                }
            }

            return true;
        }
	}
}

