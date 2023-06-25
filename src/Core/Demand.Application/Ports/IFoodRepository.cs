using Data.Core;
using Demand.Domain.Models;

namespace Demand.Application.Ports
{
	public interface IFoodRepository : IRepository<Food>
	{
		Task<IEnumerable<Food>> GetFoods();
	}
}

