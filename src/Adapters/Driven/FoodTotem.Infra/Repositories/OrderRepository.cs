﻿using Data.Core;
using Demand.Application.Ports;
using Demand.Domain.Models;
using FoodTotem.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Infra.Repositories
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

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}