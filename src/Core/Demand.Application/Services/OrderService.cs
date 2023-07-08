using Demand.Application.Ports;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
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

        public async Task<Order> GetOrder(Guid id)
        {
            return await _orderRepository.GetOrder(id);
        }

        public async Task<IEnumerable<Order>> GetQueuedOrders()
        {
            return await _orderRepository.GetOrderByStatus(OrderStatusEnum.Preparing);
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            _orderRepository.UpdateOrder(order);
            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> AddOrder(Order order)
        {
            _orderRepository.AddOrder(order);
            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
