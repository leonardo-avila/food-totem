using Data.Core;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;

namespace Demand.Application.Ports
{
	public interface IFoodRepository : IRepository<Food>
	{
		Task<IEnumerable<Food>> GetFoods();
		Task<IEnumerable<Food>> GetFoodsByCategory(FoodCategoryEnum category);
		Task<Food> GetFood(Guid id);
		Task<bool> AddFood(Food food);
		Task<bool> UpdateFood(Food food);
		Task<bool> RemoveFood(Food food); 
	}
}

