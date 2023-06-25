using Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTotem.Infra.Mappings.Identity
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.AuthenticationType)
                .HasColumnName("AuthenticationType")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(c => c.CPF)
                .HasColumnName("CPF");

            builder.Property(c => c.Email)
                .HasColumnName("Email");

            builder.Property(c => c.Protocol)
                .HasColumnName("Protocol");
        }
    }
}
