﻿using FoodTotem.Demand.Domain.Models;
using FoodTotem.Gateways.MySQL.Mappings.Demand;
using Microsoft.EntityFrameworkCore;

namespace FoodTotem.Gateways.MySQL.Contexts
{
    public class DemandContext : DbContext
    {
        public DemandContext(DbContextOptions<DemandContext> options) : base(options) 
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<OrderFood> OrderFoods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderMap());
            modelBuilder.ApplyConfiguration(new OrderFoodMap());
            modelBuilder.ApplyConfiguration(new FoodMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
