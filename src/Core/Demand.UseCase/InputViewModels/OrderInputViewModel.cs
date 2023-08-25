using Demand.UseCase.SharedViewModels;

namespace Demand.UseCase.InputViewModels
{
	public class OrderInputViewModel
	{
		public string Customer { get; set; }
		public IEnumerable<OrderFoodViewModel> Combo { get; set; }
	}
}