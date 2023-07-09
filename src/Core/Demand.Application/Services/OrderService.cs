using Demand.Application.Ports;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
using Demand.Domain.Services;

namespace Demand.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IFoodRepository _foodRepository;

        public OrderService(IOrderRepository orderRepository,
            IFoodRepository foodRepository)
        {
            _orderRepository = orderRepository;
            _foodRepository = foodRepository;
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
            return await _orderRepository.UpdateOrder(order);
        }

        public async Task<bool> AddOrder(Order order)
        {
            var isValidFoods = await CheckFoods(order.Combo);
            if (!isValidFoods) return false;
            return await _orderRepository.AddOrder(order);
        }

        public async Task<bool> DeleteOrder(Order order)
        {
            return await _orderRepository.RemoveOrder(order);
        }

        private async Task<bool> CheckFoods(List<OrderFood> combo)
        {
            var foodsInService = await _foodRepository.GetFoods();
            foreach (var food in combo)
            {
                if (!foodsInService.Select(f => f.Id).Contains(food.FoodId)) return false;
            }
            return true;
        }
    }
}
