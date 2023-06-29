using Demand.Application.Ports;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
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
			return await _foodRepository.GetFoods();
		}

		public async Task<bool> AddFood(Food food)
		{
			_foodRepository.AddFood(food);
			return await _foodRepository.UnitOfWork.Commit();
		}

		public async Task<Food> GetFood(Guid id)
		{
			return await _foodRepository.GetFood(id);
		}

		public async Task<IEnumerable<Food>> GetFoodsByCategory(FoodCategoryEnum category)
		{
			return await _foodRepository.GetFoodsByCategory(category);
		}

		public async Task<bool> UpdateFood(Food food)
		{
			_foodRepository.UpdateFood(food);
			return await _foodRepository.UnitOfWork.Commit();
		}

		public async Task<bool> DeleteFood(Guid id)
		{
			var food = await _foodRepository.GetFood(id);
			if (food is null) return false;
			_foodRepository.RemoveFood(food);
			return await _foodRepository.UnitOfWork.Commit();
		}
		
	}
}

