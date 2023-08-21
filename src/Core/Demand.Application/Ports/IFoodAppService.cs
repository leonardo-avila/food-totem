using Demand.Application.ViewModels;
using Demand.Domain.Models;

namespace Demand.Application.Ports
{
	public interface IFoodAppService
	{
		Task<IEnumerable<Food>> GetFoods();
		Task<bool> AddFood(FoodViewModel foodViewModel);
		Task<Food> GetFood(Guid id);
		Task<IEnumerable<Food>> GetFoodsByCategory(string category);
		Task<bool> DeleteFood(Guid id);
		Task<bool> UpdateFood(Food food, FoodViewModel foodViewModel);
	}
}

