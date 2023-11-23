using FoodTotem.Gateways.Payment.ViewModels;

namespace FoodTotem.Gateways.Payment.Services
{
    public interface IPaymentServices
    {
        Task<PaymentViewModel> CreatePayment(CreatePaymentViewModel createPaymentViewModel);
        Task<PaymentViewModel> GetPayments();
        Task<PaymentViewModel> GetPaymentByOrderReference(string orderReference);
    }
}