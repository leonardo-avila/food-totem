using Demand.Domain.Models;

namespace Demand.Domain.Services
{
	public interface IFoodService
	{
		Task<IEnumerable<Food>> GetFoods();
	}
}

