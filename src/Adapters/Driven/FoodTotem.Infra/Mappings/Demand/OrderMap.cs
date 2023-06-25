using Demand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTotem.Infra.Mappings.Demand
{
    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            {
                builder.ToTable("Order");

                builder.HasKey(o => o.Id);

                builder.Property(o => o.Id)
                    .HasColumnName("Id")
                    .IsRequired();

                builder.Property(o => o.Customer)
                    .HasColumnName("Customer")
                    .IsRequired();

                builder.Property(o => o.OrderDate)
                    .HasColumnName("OrderDate")
                    .IsRequired();

                builder.Property(o => o.OrderStatus)
                    .HasColumnName("OrderStatus")
                    .HasConversion<string>()
                    .IsRequired();

                builder.Property(o => o.PaymentStatus)
                    .HasColumnName("PaymentStatus")
                    .HasConversion<string>()
                    .IsRequired();

                builder.Property(o => o.LastStatusDate)
                    .HasColumnName("LastStatusDate");

            }
        }
    }
}
