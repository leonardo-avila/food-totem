using FoodTotem.Gateways.Payment.ViewModels;

namespace FoodTotem.Gateways.Payment.Services
{
    public interface IPaymentServices
    {
        Task<PaymentViewModel> CreatePayment(CreatePaymentViewModel createPaymentViewModel);
        Task<IEnumerable<PaymentViewModel>> GetPayments();
        Task<PaymentViewModel> GetPaymentByOrderReference(string orderReference);
        Task<PaymentViewModel> UpdatePaymentStatus(PaymentUpdateViewModel paymentUpdateViewModel);
    }
}