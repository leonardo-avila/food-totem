using Data.Core;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;

namespace Demand.Application.Ports
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetOrder(Guid id);
        Task<IEnumerable<Order>> GetOrderByStatus(OrderStatusEnum orderStatus);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void RemoveOrder(Order order);
    }
}
