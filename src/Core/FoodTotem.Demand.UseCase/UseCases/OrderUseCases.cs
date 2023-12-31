﻿using FoodTotem.Demand.UseCase.Ports;
using FoodTotem.Demand.UseCase.InputViewModels;
using FoodTotem.Demand.UseCase.OutputViewModels;
using FoodTotem.Demand.Domain.Models;
using FoodTotem.Demand.Domain.Models.Enums;
using FoodTotem.Demand.Domain.Ports;
using FoodTotem.Demand.Domain.Repositories;
using FoodTotem.Domain.Core;

namespace FoodTotem.Demand.UseCase.UseCases
{
    public class OrderUseCases : IOrderUseCases
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IFoodRepository _foodRepository;
        private readonly IOrderService _orderService;

        public OrderUseCases(IOrderRepository orderRepository,
            IFoodRepository foodRepository,
            IOrderService orderService)
        {
            _orderRepository = orderRepository;
            _foodRepository = foodRepository;
            _orderService = orderService;
        }

        public async Task<IEnumerable<CheckoutOrderViewModel>> GetOrders()
        {
            var orders = await _orderRepository.GetAll();
            return ProduceOrderViewModelCollection(orders);
        }

        public async Task<CheckoutOrderViewModel> GetOrder(Guid id)
        {
            var order = await _orderRepository.Get(id) ?? throw new DomainException("There is no order with this id.");

            return ProduceOrderViewModel(order);
        }

        public async Task<IEnumerable<CheckoutOrderViewModel>> GetQueuedOrders()
        {
            var queuedOrders = await _orderRepository.GetOrderByStatus(OrderStatusEnum.Preparing);
            return ProduceOrderViewModelCollection(queuedOrders);
        }

        public async Task<CheckoutOrderViewModel> UpdateOrder(Guid id, OrderInputViewModel orderViewModel)
        {
            var order = await _orderRepository.Get(id) ?? throw new DomainException("There is no order with this id.");

            order.SetCombo(TransformOrderFoodViewModelIntoOrderFoodCollection(orderViewModel.Combo));

            var foodsInService = await _foodRepository.GetAll();
            _orderService.IsValidOrder(order, foodsInService);

            await _orderRepository.Update(order);

            return ProduceOrderViewModel(order);
        }

        public async Task<IEnumerable<CheckoutOrderViewModel>> GetOngoingOrders()
        {
            var ongoingOrders =  _orderService.FilterOngoingOrders(await _orderRepository.GetAll());
            return ProduceOrderViewModelCollection(ongoingOrders);
        }

        public async Task<CheckoutOrderViewModel> CheckoutOrder(OrderInputViewModel orderViewModel)
        {
            var order = MountOrder(orderViewModel);

            var foodsInService = await _foodRepository.GetAll();

            _orderService.IsValidOrder(order, foodsInService);

            await _orderRepository.Create(order);
            var createdOrder = await _orderRepository.Get(order.Id);

            return ProduceOrderViewModel(createdOrder);
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            var order = await _orderRepository.Get(id) ?? throw new DomainException("There is no order with this id.");
            return await _orderRepository.Delete(order);
        }

        public async Task<CheckoutOrderViewModel> ApproveOrderPayment(Guid id)
        {
            var order = await _orderRepository.Get(id) ?? throw new DomainException("There is no order with this id.");

            order.ApprovePayment();

            await _orderRepository.Update(order);

            return ProduceOrderViewModel(order);
        }

        public async Task<CheckoutOrderViewModel> UpdateOrderStatus(Guid id, string newOrderStatus)
        {
            if (!_orderService.IsValidOrderStatus(newOrderStatus))
            {
                throw new DomainException("Invalid order status");
            }

            var order = await _orderRepository.Get(id) ?? throw new DomainException("There is no order with this id.");

            order.UpdateOrderStatus(newOrderStatus);
            await _orderRepository.Update(order);

            return ProduceOrderViewModel(order);
        }

        private static Order MountOrder(OrderInputViewModel orderViewModel)
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

        private static IEnumerable<CheckoutOrderViewModel> ProduceOrderViewModelCollection(IEnumerable<Order> orders)
        {
            foreach (var order in orders)
            {
                yield return ProduceOrderViewModel(order);
            }
        }

        private static CheckoutOrderViewModel ProduceOrderViewModel(Order order)
        {
            return new CheckoutOrderViewModel()
            {
                Id = order.Id,
                Customer = order.Customer,
                LastStatusDate = order.LastStatusDate,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus.ToString(),
                PaymentStatus = order.PaymentStatus.ToString(),
                Total = order.GetTotal(),
                Combo = ProduceOrderFoodViewModelCollection(order.Combo)
            };
        }

        private static IEnumerable<CheckoutOrderFoodViewModel> ProduceOrderFoodViewModelCollection(IEnumerable<OrderFood> orderFoods)
        {
            foreach (var orderFood in orderFoods)
            {
                yield return ProduceOrderFoodViewModel(orderFood);
            }
        }

        private static CheckoutOrderFoodViewModel ProduceOrderFoodViewModel(OrderFood orderFood)
        {
            return new CheckoutOrderFoodViewModel()
            {
                FoodId = orderFood.FoodId,
                Price = orderFood.Food.Price,
                Quantity = orderFood.Quantity
            };
        }

        private static IEnumerable<OrderFood> TransformOrderFoodViewModelIntoOrderFoodCollection(IEnumerable<OrderFoodInputViewModel> orderFoods)
        {
            foreach (var orderFood in orderFoods)
            {
                yield return new OrderFood(orderFood.FoodId, orderFood.Quantity);
            }
        }
    }
}
