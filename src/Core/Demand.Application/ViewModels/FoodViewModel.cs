using Demand.Domain.Models.Enums;

namespace Demand.Application.ViewModels
{
	public class FoodViewModel
	{
		public string Name { get; set; }
		public FoodCategoryEnum Category { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public string ImageUrl { get; set; }
	}
}

