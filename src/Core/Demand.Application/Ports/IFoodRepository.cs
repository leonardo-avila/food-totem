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
		void AddFood(Food food);
		void UpdateFood(Food food);
		void RemoveFood(Food food); 
	}
}

