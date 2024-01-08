namespace FoodTotem.Gateways.Demand.ViewModels
{
	public class CreateOrderViewModel
	{
		public string Customer { get; set; }
		public IEnumerable<OrderItemViewModel> Combo { get; set; }
	}
}