using Data.Core;
using Demand.Domain.Models;
using FoodTotem.Infra.Mappings.DemandContext;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Infra.Context
{
    public class DemandContext : DbContext, IUnitOfWork
    {
        public DemandContext(DbContextOptions<DemandContext> options) : base(options) 
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Demand");
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new OrderFoodMap());
            modelBuilder.ApplyConfiguration(new FoodMap());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            var success = await SaveChangesAsync() > 0;

            return success;
        }
    }
}
