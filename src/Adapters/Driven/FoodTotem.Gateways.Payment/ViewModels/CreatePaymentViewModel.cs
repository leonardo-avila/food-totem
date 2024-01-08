namespace FoodTotem.Gateways.Payment.ViewModels
{
    public class CreatePaymentViewModel
    {
        public string OrderReference { get; set; }
        public double Total { get; set; }
        public IEnumerable<PaymentOrderItemViewModel> OrderItems { get; set; }
    }
}