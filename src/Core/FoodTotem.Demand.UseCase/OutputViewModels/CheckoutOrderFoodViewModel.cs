namespace FoodTotem.Demand.UseCase.OutputViewModels
{
	public class CheckoutOrderFoodViewModel
	{
		public Guid FoodId { get; set; }
		public double Price { get; set; }
		public int Quantity { get; set; }
	}
}

