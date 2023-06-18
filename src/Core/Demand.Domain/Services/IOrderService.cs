using Demand.Domain.Models;

namespace Demand.Domain.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrders();
    }
}
