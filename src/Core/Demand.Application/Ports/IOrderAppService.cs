using Demand.Application.ViewModels;
using Demand.Domain.Models;

namespace Demand.Application.Ports
{
    public interface IOrderAppService
    {
        Task<Order> GetOrder(Guid id);
        Task<IEnumerable<Order>> GetOrders();
        Task<IEnumerable<Order>> GetQueuedOrders();
        Task<IEnumerable<Order>> GetOngoingOrders();
        Task<bool> UpdateOrderStatus(Order order, string newOrderStatus);
        Task<bool> UpdateOrder(Order order);
        Task<CheckoutOrderViewModel> CheckoutOrder(OrderViewModel orderViewModel);
        Task<bool> DeleteOrder(Order order);
        Task<bool> ApproveOrderPayment(Order order);
    }
}
