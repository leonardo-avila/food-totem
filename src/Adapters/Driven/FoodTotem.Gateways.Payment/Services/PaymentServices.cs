using FoodTotem.Gateways.Http;
using FoodTotem.Gateways.Payment.ViewModels;
using Microsoft.Extensions.Configuration;

namespace FoodTotem.Gateways.Payment.Services;

public class PaymentServices : IPaymentServices
{
    private readonly IHttpHandler _httpHandler;
    private readonly IConfiguration _configuration;
    private readonly string PaymentUrl;

    public PaymentServices(IHttpHandler httpHandler, IConfiguration configuration)
    {
        _httpHandler = httpHandler;
        _configuration = configuration;
        PaymentUrl = _configuration.GetSection("PaymentServiceUrl").Value;
    }

    public async Task<PaymentViewModel> CreatePayment(CreatePaymentViewModel createPaymentViewModel)
    {
        return await _httpHandler.PostAsync<CreatePaymentViewModel, PaymentViewModel>($"{PaymentUrl}/payment", createPaymentViewModel, null);
    }

    public async Task<PaymentViewModel> GetPayments()
    {
        return await _httpHandler.GetAsync<PaymentViewModel>($"{PaymentUrl}/payment", null);
    }

    public async Task<PaymentViewModel> GetPaymentByOrderReference(string orderReference)
    {
        return await _httpHandler.GetAsync<PaymentViewModel>($"{PaymentUrl}/payment/{orderReference}", null);
    }
}