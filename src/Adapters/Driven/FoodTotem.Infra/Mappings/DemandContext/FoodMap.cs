﻿using Demand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodTotem.Infra.Mappings.DemandContext
{
    public class FoodMap : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.ToTable("Food");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(f => f.Name)
                .HasColumnName("Name")
                .IsRequired();

            builder.Property(f => f.Description)
                .HasColumnName("Description")
                .IsRequired();

            builder.Property(f => f.ImageUrl)
                .HasColumnName("ImageUrl")
                .IsRequired();

            builder.Property(f => f.Price)
                .HasColumnName("Price")
                .IsRequired();

            builder.Property(f => f.Category)
                .HasColumnName("Category")
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
