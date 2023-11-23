namespace FoodTotem.Gateways.Demand.ViewModels
{
    public class OrderItemViewModel
    {
        public string FoodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public int Quantity  { get; set; }
    }
}