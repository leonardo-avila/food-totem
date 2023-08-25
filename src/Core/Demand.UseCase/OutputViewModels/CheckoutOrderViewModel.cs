using Demand.UseCase.SharedViewModels;

namespace Demand.UseCase.OutputViewModels
{
    public class CheckoutOrderViewModel
    {
        public string Customer { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime LastStatusDate { get; set; }
        public double Total { get; set; }
        public IEnumerable<OrderFoodViewModel> Combo { get; set; }
    }
}

