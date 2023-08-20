using Data.Core;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;

namespace Demand.Domain.Repositories
{
	public interface IFoodRepository : IRepository<Food>
	{
		Task<IEnumerable<Food>> GetFoodsByCategory(FoodCategoryEnum category);
	}
}
