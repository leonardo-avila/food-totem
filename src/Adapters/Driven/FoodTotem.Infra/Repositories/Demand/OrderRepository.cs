﻿using Data.Core;
using Demand.Application.Ports;
using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
using FoodTotem.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Infra.Repositories.Demand
{
    public class OrderRepository : IOrderRepository
    {
        protected readonly DemandContext Db;
        protected readonly DbSet<Order> DbSet;

        public OrderRepository(DemandContext context)
        {
            Db = context;
            DbSet = Db.Set<Order>();
        }

        public IUnitOfWork UnitOfWork => Db;

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomer(string customer)
        {
            return await DbSet.Where(o => o.Customer.Equals(customer)).ToListAsync();
        }

        public async Task<Order> GetOrder(Guid orderId)
        {
            return await DbSet.FindAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrderByStatus(OrderStatusEnum orderStatus)
        {
            return await DbSet.Where(o => o.OrderStatus.Equals(orderStatus)).ToListAsync();
        }

        public void AddOrder(Order order)
        {
            DbSet.Add(order);
        }

        public void UpdateOrder(Order order)
        {
            DbSet.Update(order);
        }

        public void RemoveOrder(Order order)
        {
            DbSet.Remove(order);
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}