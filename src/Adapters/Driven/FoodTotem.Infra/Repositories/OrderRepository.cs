using Demand.Application.Ports;
using Demand.Domain.Models;

namespace FoodTotem.Infra.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static List<Order> orders = new List<Order>()
        {
            new Order("Test Order")
        };
        public IEnumerable<Order> GetOrders()
        {
            return orders;
        }
    }
}