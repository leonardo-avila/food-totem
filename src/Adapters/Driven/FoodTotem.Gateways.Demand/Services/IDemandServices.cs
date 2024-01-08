using FoodTotem.Gateways.Demand.ViewModels;

namespace FoodTotem.Gateways.Demand.Services
{
    public interface IDemandServices
    {
        Task<IEnumerable<OrderViewModel>> GetOrders();
        Task<IEnumerable<OrderViewModel>> GetOngoingOrders();
        Task<IEnumerable<OrderViewModel>> GetQueuedOrders();
        Task<OrderViewModel> GetOrder(string id);
        Task<OrderViewModel> CreateOrder(CreateOrderViewModel demandItem);
        Task<OrderViewModel> UpdateOrderStatus(string orderId, string orderStatus);
        Task<OrderViewModel> UpdateOrderPayment(string orderId, string qrCode);
        Task DeleteOrder(string id);
    }
}