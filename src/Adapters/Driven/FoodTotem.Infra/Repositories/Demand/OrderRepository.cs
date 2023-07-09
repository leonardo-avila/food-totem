using Data.Core;
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
            return await DbSet.Include(o => o.Combo)
                .ThenInclude(c => c.Food)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomer(string customer)
        {
            return await DbSet.Where(o => o.Customer.Equals(customer)).ToListAsync();
        }

        public async Task<Order> GetOrder(Guid orderId)
        {
            return await DbSet.Include(o => o.Combo)
                .ThenInclude(c => c.Food)
                .FirstOrDefaultAsync(o => o.Id.Equals(orderId));
        }

        public async Task<IEnumerable<Order>> GetOrderByStatus(OrderStatusEnum orderStatus)
        {
            return await DbSet.Include(o => o.Combo)
                .ThenInclude(c => c.Food)
                .Where(o => o.OrderStatus.Equals(orderStatus)).ToListAsync();
        }

        public async Task<bool> AddOrder(Order order)
        {
            DbSet.Add(order);
            return await UnitOfWork.Commit();
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            DbSet.Update(order);
            return await UnitOfWork.Commit();
        }

        public async Task<bool> RemoveOrder(Order order)
        {
            DbSet.Remove(order);
            return await UnitOfWork.Commit();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}