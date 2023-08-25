using Demand.Domain.Models;
using Demand.Domain.Models.Enums;
using Demand.Domain.Repositories;
using FoodTotem.Gateways.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Gateways.Repositories.Demand
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        protected readonly DemandContext Db;
        protected readonly DbSet<Order> DbSet;

        public OrderRepository(DemandContext context) : base (context)
        {
            Db = context;
            DbSet = Db.Set<Order>();
        }

        public override async Task<IEnumerable<Order>> GetAll()
        {
            return await DbSet.Include(o => o.Combo)
                .ThenInclude(c => c.Food)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomer(string customer)
        {
            return await DbSet.Where(o => o.Customer.Equals(customer)).ToListAsync();
        }

        public override async Task<Order> Get(Guid orderId)
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
    }
}