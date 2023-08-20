using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
using Demand.Domain.Repositories;
using Demand.Domain.Services;

namespace Demand.Application.Services
{
	public class FoodService : IFoodService
	{
		private readonly IFoodRepository _foodRepository;

		public FoodService(IFoodRepository foodRepository)
		{
			_foodRepository = foodRepository;
		}

		public async Task<IEnumerable<Food>> GetFoods()
		{
			return await _foodRepository.GetAll();
		}

		public async Task<bool> AddFood(Food food)
		{
			return await _foodRepository.Create(food);
		}

		public async Task<Food> GetFood(Guid id)
		{
			return await _foodRepository.Get(id);
		}

		public async Task<IEnumerable<Food>> GetFoodsByCategory(FoodCategoryEnum category)
		{
			return await _foodRepository.GetFoodsByCategory(category);
		}

		public async Task<bool> UpdateFood(Food food)
		{
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

