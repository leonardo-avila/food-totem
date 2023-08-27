using FoodTotem.Demand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTotem.Gateways.MySQL.Mappings.Demand
{
    public class OrderFoodMap : IEntityTypeConfiguration<OrderFood>
    {
        public void Configure(EntityTypeBuilder<OrderFood> builder)
        {
            builder.ToTable("OrderFood");

            builder.HasKey(of => of.Id);

            builder.Property(of => of.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(of => of.OrderId)
                .HasColumnName("OrderId")
                .IsRequired();

            builder.Property(of => of.FoodId)
                .HasColumnName("FoodId")
                .IsRequired();

            builder.HasOne(of => of.Order)
                .WithMany(o => o.Combo)
                .HasForeignKey(of => of.OrderId);

            builder.HasOne(of => of.Food)
                .WithMany(f => f.Orders)
                .HasForeignKey(of => of.FoodId);
        }
    }
}
