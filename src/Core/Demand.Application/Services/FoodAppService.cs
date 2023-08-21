using Demand.Application.Ports;
using Demand.Application.ViewModels;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
using Demand.Domain.Models.Validators;
using Demand.Domain.Repositories;
using Domain.Core;
using FluentValidation;

namespace Demand.Application.Services
{
	public class FoodAppService : IFoodAppService
	{
		private readonly IFoodRepository _foodRepository;
		private readonly IValidator<Food> _foodValidator;

        public FoodAppService(IFoodRepository foodRepository,
            IValidator<Food> foodValidator)
		{
			_foodRepository = foodRepository;
            _foodValidator = foodValidator;
        }

		public async Task<IEnumerable<Food>> GetFoods()
		{
			return await _foodRepository.GetAll();
		}

		public async Task<bool> AddFood(FoodViewModel foodViewModel)
		{
            var food = new Food(
                foodViewModel.Name,
                foodViewModel.Description,
                foodViewModel.ImageUrl,
                foodViewModel.Price,
                foodViewModel.Category
                );
            var validationResult = _foodValidator.Validate(food);

			if (!validationResult.IsValid)
			{
				throw new DomainException(validationResult.ToString());
			}

            return await _foodRepository.Create(food);
		}

		public async Task<Food> GetFood(Guid id)
		{
			return await _foodRepository.Get(id);
		}

		public async Task<IEnumerable<Food>> GetFoodsByCategory(string category)
		{
            var existentCategory = Enum.TryParse(category, false, out FoodCategoryEnum categoryEnum);
			if (!existentCategory) throw new DomainException("Invalid category.");

            return await _foodRepository.GetFoodsByCategory(categoryEnum);
		}

		public async Task<bool> UpdateFood(Food food, FoodViewModel foodViewModel)
		{
            food.UpdateName(foodViewModel.Name);
            food.UpdateDescription(foodViewModel.Description);
            food.UpdateImageUrl(foodViewModel.ImageUrl);
            food.UpdatePrice(foodViewModel.Price);
            food.UpdateCategory(foodViewModel.Category);
            var validationResult = _foodValidator.Validate(food);

			if (!validationResult.IsValid)
			{
                throw new DomainException(validationResult.ToString());
            }
            return await _foodRepository.Update(food);
        }

		public async Task<bool> DeleteFood(Guid id)
		{
			var food = await _foodRepository.Get(id);
			if (food is null) return false;
			return await _foodRepository.Delete(food);
		}
		
	}
}

