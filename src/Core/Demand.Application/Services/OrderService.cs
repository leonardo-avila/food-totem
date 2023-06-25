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

        public async Task<IEnumerable<Order>> GetOrders() 
        {
            return await _orderRepository.GetOrders();
        }
    }
}
