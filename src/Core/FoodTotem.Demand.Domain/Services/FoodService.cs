using FoodTotem.Demand.Domain.Models;
using FoodTotem.Demand.Domain.Models.Enums;
using FoodTotem.Demand.Domain.Ports;
using FoodTotem.Domain.Core;
using FluentValidation;

namespace FoodTotem.Demand.Domain.Services
{
    public class FoodService : IFoodService
	{
        private readonly IValidator<Food> _foodValidator;

        public FoodService(IValidator<Food> foodValidator)
        {
            _foodValidator = foodValidator;
        }

        public bool IsValidCategory(string category)
        {
            return Enum.IsDefined(typeof(FoodCategoryEnum), category);
        }

        public void ValidateFood(Food food)
        {
            var validationResult = _foodValidator.Validate(food);

            if (!validationResult.IsValid)
            {
                throw new DomainException(validationResult.ToString());
            }
        }
    }
}

