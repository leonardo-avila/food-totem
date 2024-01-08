using FoodTotem.Gateways.Demand.ViewModels;
using FoodTotem.Gateways.Http;
using Microsoft.Extensions.Configuration;

namespace FoodTotem.Gateways.Demand.Services
{
    public class DemandServices : IDemandServices

    {
        private readonly IHttpHandler _httpHandler;
        private readonly IConfiguration _configuration;

        private readonly string DemandUrl;

        public DemandServices(IHttpHandler httpHandler, IConfiguration configuration)
        {
            _httpHandler = httpHandler;
            _configuration = configuration;
            DemandUrl = _configuration.GetSection("DemandServiceUrl").Value;
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrders()
        {
            return await _httpHandler.GetAsync<IEnumerable<OrderViewModel>>($"{DemandUrl}/order", null);
        }

        public async Task<IEnumerable<OrderViewModel>> GetOngoingOrders()
        {
            return await _httpHandler.GetAsync<IEnumerable<OrderViewModel>>($"{DemandUrl}/order/ongoing", null);
        }

        public async Task<IEnumerable<OrderViewModel>> GetQueuedOrders()
        {
            return await _httpHandler.GetAsync<IEnumerable<OrderViewModel>>($"{DemandUrl}/order/queued", null);
        }

        public async Task<OrderViewModel> GetOrder(string id)
        {
            return await _httpHandler.GetAsync<OrderViewModel>($"{DemandUrl}/order/{id}", null);
        }

        public async Task<OrderViewModel> CreateOrder(CreateOrderViewModel demandItem)
        {
            return await _httpHandler.PostAsync<CreateOrderViewModel, OrderViewModel>($"{DemandUrl}/order", demandItem, null);
        }

        public async Task<OrderViewModel> UpdateOrderStatus(string orderId, string orderStatus)
        {
            return await _httpHandler.PutAsync<string, OrderViewModel>($"{DemandUrl}/order/{orderId}", orderStatus, null);
        }

        public async Task<OrderViewModel> UpdateOrderPayment(string orderId, string qrCode)
        {
            return await _httpHandler.PatchAsync<string, OrderViewModel>($"{DemandUrl}/order/{orderId}", qrCode, null);
        }

        public async Task DeleteOrder(string id)
        {
            await _httpHandler.DeleteAsync($"{DemandUrl}/order/{id}", null);
        }
    }
}
