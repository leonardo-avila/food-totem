using FoodTotem.Demand.Domain.Models;

namespace FoodTotem.Demand.Domain.Ports
{
	public interface IFoodService
	{
		bool IsValidCategory(string category);
		void ValidateFood(Food food);
	}
}

