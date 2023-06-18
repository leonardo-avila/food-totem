using Demand.Domain.Models;

namespace Demand.Domain.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();
    }
}
