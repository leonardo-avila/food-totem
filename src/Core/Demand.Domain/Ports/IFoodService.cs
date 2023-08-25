using Demand.Domain.Models;

namespace Demand.Domain.Ports
{
	public interface IFoodService
	{
		bool IsValidCategory(string category);
		void ValidateFood(Food food);
	}
}

