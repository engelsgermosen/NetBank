﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetBank.Infraestructure.Persistence.Context;

#nullable disable

namespace NetBank.Infraestructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NetBank.Core.Domain.Entities.Beneficiarie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Beneficiarios", (string)null);
                });

            modelBuilder.Entity("NetBank.Core.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(14,4)");

                    b.Property<decimal?>("CreditLimit")
                        .HasColumnType("decimal(14,4)");

                    b.Property<bool>("IsMain")
                        .HasColumnType("bit");

                    b.Property<byte>("ProductType")
                        .HasColumnType("tinyint");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Productos", (string)null);
                });

            modelBuilder.Entity("NetBank.Core.Domain.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(14,4)");

                    b.Property<int?>("DestinationProductId")
                        .HasColumnType("int");

                    b.Property<int?>("OriginProductId")
                        .HasColumnType("int");

                    b.Property<bool>("State")
                        .HasColumnType("bit");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<byte>("TransactionType")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("DestinationProductId");

                    b.HasIndex("OriginProductId");

                    b.ToTable("Transacciones", (string)null);
                });

            modelBuilder.Entity("NetBank.Core.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Identification")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("InitialAmount")
                        .HasColumnType("decimal(14,4)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("UserType")
                        .HasColumnType("tinyint");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Identification")
                        .IsUnique();

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("NetBank.Core.Domain.Entities.Beneficiarie", b =>
                {
                    b.HasOne("NetBank.Core.Domain.Entities.User", "User")
                        .WithMany("Beneficiaries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("User");
                });

            modelBuilder.Entity("NetBank.Core.Domain.Entities.Product", b =>
                {
                    b.HasOne("NetBank.Core.Domain.Entities.User", "User")
                        .WithMany("Products")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("User");
                });

            modelBuilder.Entity("NetBank.Core.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("NetBank.Core.Domain.Entities.Product", "DestinationProduct")
                        .WithMany("DestinationTransactions")
                        .HasForeignKey("DestinationProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("NetBank.Core.Domain.Entities.Product", "OriginProduct")
                        .WithMany("OriginTransactions")
                        .HasForeignKey("OriginProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("DestinationProduct");

                    b.Navigation("OriginProduct");
                });

            modelBuilder.Entity("NetBank.Core.Domain.Entities.Product", b =>
                {
                    b.Navigation("DestinationTransactions");

                    b.Navigation("OriginTransactions");
                });

            modelBuilder.Entity("NetBank.Core.Domain.Entities.User", b =>
                {
                    b.Navigation("Beneficiaries");

                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
