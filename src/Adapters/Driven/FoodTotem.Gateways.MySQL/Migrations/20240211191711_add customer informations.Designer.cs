﻿// <auto-generated />
using System;
using FoodTotem.Gateways.MySQL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FoodTotem.Gateways.MySQL.Migrations
{
    [DbContext(typeof(IdentityContext))]
    [Migration("20240211191711_add customer informations")]
    partial class addcustomerinformations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("identity")
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FoodTotem.Identity.Domain.Models.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Address")
                        .HasColumnType("longtext")
                        .HasColumnName("Address");

                    b.Property<string>("AuthenticationType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("AuthenticationType");

                    b.Property<string>("CPF")
                        .HasColumnType("longtext")
                        .HasColumnName("CPF");

                    b.Property<string>("Email")
                        .HasColumnType("longtext")
                        .HasColumnName("Email");

                    b.Property<string>("Name")
                        .HasColumnType("longtext")
                        .HasColumnName("Name");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext")
                        .HasColumnName("Phone");

                    b.Property<Guid?>("Protocol")
                        .HasColumnType("char(36)")
                        .HasColumnName("Protocol");

                    b.HasKey("Id");

                    b.ToTable("Customer", "identity");
                });
#pragma warning restore 612, 618
        }
    }
}
