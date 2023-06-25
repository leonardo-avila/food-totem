using Demand.Domain.Models;

namespace Demand.Domain.Services
{
    public interface IOrderService
    {
        Task<Order> GetOrder(Guid id);
        Task<IEnumerable<Order>> GetOrders();
    }
}
