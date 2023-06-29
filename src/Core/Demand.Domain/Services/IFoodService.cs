using Demand.Domain.Models;
using Demand.Domain.Models.Enums;

namespace Demand.Domain.Services
{
	public interface IFoodService
	{
		Task<IEnumerable<Food>> GetFoods();
		Task<bool> AddFood(Food food);
		Task<Food> GetFood(Guid id);
		Task<IEnumerable<Food>> GetFoodsByCategory(FoodCategoryEnum category);
		Task<bool> DeleteFood(Guid id);
		Task<bool> UpdateFood(Food food);
	}
}

