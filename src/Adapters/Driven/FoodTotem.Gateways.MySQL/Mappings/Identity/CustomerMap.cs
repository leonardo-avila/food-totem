﻿using FoodTotem.Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTotem.Gateways.MySQL.Mappings.Identity
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.AuthenticationType)
                .HasColumnName("AuthenticationType")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(c => c.CPF)
                .HasColumnName("CPF");

            builder.Property(c => c.Email)
                .HasColumnName("Email");
            
            builder.Property(c => c.Name)
                .HasColumnName("Name");

            builder.Property(c => c.Address)
                .HasColumnName("Address");

            builder.Property(c => c.Phone)
                .HasColumnName("Phone");

            builder.Property(c => c.Protocol)
                .HasColumnName("Protocol");
        }
    }
}
