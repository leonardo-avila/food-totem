using Data.Core;
using Demand.Domain.Models;
using FoodTotem.Infra.Mappings.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Infra.Contexts
{
    public class IdentityContext : DbContext, IUnitOfWork
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.ApplyConfiguration(new CustomerMap());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            var success = await SaveChangesAsync() > 0;

            return success;
        }
    }
}

