using Demand.Application.Ports;
using Demand.Application.ViewModels;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
using Demand.Domain.Ports;
using Demand.Domain.Repositories;
using Domain.Core;

namespace Demand.Application.Services
{
    public class OrderAppService : IOrderAppService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IOrderService _orderService;

        public OrderAppService(IOrderRepository orderRepository,
            IFoodRepository foodRepository,
            IOrderService orderService)
        {
            _orderRepository = orderRepository;
            _foodRepository = foodRepository;
            _orderService = orderService;
        }

        public async Task<IEnumerable<Order>> GetOrders() 
        {
            return await _orderRepository.GetAll();
        }

        public async Task<Order> GetOrder(Guid id)
        {
            return await _orderRepository.Get(id);
        }

        public async Task<IEnumerable<Order>> GetQueuedOrders()
        {
            return await _orderRepository.GetOrderByStatus(OrderStatusEnum.Preparing);
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            return await _orderRepository.Update(order);
        }

        public async Task<IEnumerable<Order>> GetOngoingOrders()
        {
            return _orderService.FilterOngoingOrders(await GetOrders());
        }

        public async Task<CheckoutOrderViewModel> CheckoutOrder(OrderViewModel orderViewModel)
        {
            var order = MountOrder(orderViewModel);

            var foodsInService = await _foodRepository.GetAll();

            _orderService.IsValidOrder(order, foodsInService);

            await _orderRepository.Create(order);
            var createdOrder = await GetOrder(order.Id);

            var checkoutOrder = new CheckoutOrderViewModel()
            {
                Customer = createdOrder.Customer,
                OrderStatus = createdOrder.OrderStatus,
                OrderDate = createdOrder.OrderDate,
                Total = $"R$ {createdOrder.GetTotal()}",
                Combo = createdOrder.Combo
            };

            return checkoutOrder;
        }

        public async Task<bool> DeleteOrder(Order order)
        {
            return await _orderRepository.Delete(order);
        }

        public async Task<bool> ApproveOrderPayment(Order order)
        {
            order.ApprovePayment();
            return await UpdateOrder(order);
        }

        public async Task<bool> UpdateOrderStatus(Order order, string newOrderStatus)
        {
            if(!_orderService.IsValidOrderStatus(newOrderStatus))
            {
                throw new DomainException("Invalid order status");
            }

            order.UpdateOrderStatus(newOrderStatus);
            return await _orderRepository.Update(order);
        }

        private static Order MountOrder(OrderViewModel orderViewModel)
        {
            var order = new Order(orderViewModel.Customer);
            var combo = new List<OrderFood>();

            foreach (var food in orderViewModel.Combo)
            {
                combo.Add(new OrderFood(food.FoodId, food.Quantity));
            }
            order.SetCombo(combo);

            return order;
        }
    }
}
