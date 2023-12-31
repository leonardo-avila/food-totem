﻿using FoodTotem.Demand.Domain.Models;

namespace FoodTotem.Demand.Domain.Ports
{
	public interface IOrderService
	{
		bool IsValidOrder(Order order, IEnumerable<Food> foodsInService);
		IEnumerable<Order> FilterOngoingOrders(IEnumerable<Order> orders);
		bool IsValidOrderStatus(string orderStatus);
	}
}

