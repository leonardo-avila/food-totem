using Demand.Application.Ports;
using Demand.Domain.Models;
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

		public Task<IEnumerable<Food>> GetFoods()
		{
			return _foodRepository.GetFoods();
		}
	}
}

