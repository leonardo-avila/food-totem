using Demand.Application.Ports;
using Demand.Domain.Models;
using Demand.Domain.Services;

namespace Demand.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public IEnumerable<Order> GetOrders() 
        {
            return _orderRepository.GetOrders();
        }
    }
}
