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
        Task<bool> AddOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> RemoveOrder(Order order);
    }
}
