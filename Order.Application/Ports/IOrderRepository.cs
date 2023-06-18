using Demand.Domain.Models;

namespace Demand.Application.Ports
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
    }
}
