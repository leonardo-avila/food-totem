using Data.Core;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;

namespace Demand.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrderByStatus(OrderStatusEnum orderStatus);
    }
}
