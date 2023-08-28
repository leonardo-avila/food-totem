using FoodTotem.Data.Core;
using FoodTotem.Demand.Domain.Models;
using FoodTotem.Demand.Domain.Models.Enums;

namespace FoodTotem.Demand.Domain.Repositories
{
	public interface IFoodRepository : IRepository<Food>
	{
		Task<IEnumerable<Food>> GetFoodsByCategory(FoodCategoryEnum category);
	}
}
