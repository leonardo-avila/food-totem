using Data.Core;
using Demand.Domain.Models;

namespace Demand.Application.Ports
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrders();
    }
}
