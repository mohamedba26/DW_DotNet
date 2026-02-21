using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SalesDW.API.Models;

namespace SalesDW.API.Data;

public partial class DwSalesPurchasingContext : DbContext
{
    public DwSalesPurchasingContext()
    {
    }

    public DwSalesPurchasingContext(DbContextOptions<DwSalesPurchasingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DimCustomer> DimCustomers { get; set; }

    public virtual DbSet<DimDate> DimDates { get; set; }

    public virtual DbSet<DimProduct> DimProducts { get; set; }

    public virtual DbSet<DimTerritory> DimTerritories { get; set; }

    public virtual DbSet<DimVendor> DimVendors { get; set; }

    public virtual DbSet<FactPurchasing> FactPurchasings { get; set; }

    public virtual DbSet<FactSale> FactSales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DimCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerKey).HasName("PK__DimCusto__95011E64E4FE6342");

            entity.ToTable("DimCustomer");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerType).HasMaxLength(50);
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<DimDate>(entity =>
        {
            entity.HasKey(e => e.DateKey).HasName("PK__DimDate__40DF45E3394E20BE");

            entity.ToTable("DimDate");

            entity.Property(e => e.DateKey).ValueGeneratedNever();
            entity.Property(e => e.MonthName).HasMaxLength(20);
        });

        modelBuilder.Entity<DimProduct>(entity =>
        {
            entity.HasKey(e => e.ProductKey).HasName("PK__DimProdu__A15E99B305422E25");

            entity.ToTable("DimProduct");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.ListPrice).HasColumnType("money");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductName).HasMaxLength(200);
            entity.Property(e => e.StandardCost).HasColumnType("money");
            entity.Property(e => e.Subcategory).HasMaxLength(100);
        });

        modelBuilder.Entity<DimTerritory>(entity =>
        {
            entity.HasKey(e => e.TerritoryKey).HasName("PK__DimTerri__C54B735DD9901A7C");

            entity.ToTable("DimTerritory");

            entity.Property(e => e.CountryRegionCode).HasMaxLength(10);
            entity.Property(e => e.GroupName).HasMaxLength(50);
            entity.Property(e => e.TerritoryId).HasColumnName("TerritoryID");
            entity.Property(e => e.TerritoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<DimVendor>(entity =>
        {
            entity.HasKey(e => e.VendorKey).HasName("PK__DimVendo__FDB660179F5DB40C");

            entity.ToTable("DimVendor");

            entity.Property(e => e.AccountNumber).HasMaxLength(50);
            entity.Property(e => e.BusinessEntityId).HasColumnName("BusinessEntityID");
            entity.Property(e => e.VendorName).HasMaxLength(200);
        });

        modelBuilder.Entity<FactPurchasing>(entity =>
        {
            entity.HasKey(e => e.PurchasingKey).HasName("PK__FactPurc__9098E18E6FD91A62");

            entity.ToTable("FactPurchasing");

            entity.Property(e => e.LineTotal).HasColumnType("money");
            entity.Property(e => e.UnitCost).HasColumnType("money");

            entity.HasOne(d => d.OrderDateKeyNavigation).WithMany(p => p.FactPurchasings)
                .HasForeignKey(d => d.OrderDateKey)
                .HasConstraintName("FK_FactPurchasing_Date");

            entity.HasOne(d => d.ProductKeyNavigation).WithMany(p => p.FactPurchasings)
                .HasForeignKey(d => d.ProductKey)
                .HasConstraintName("FK_FactPurchasing_Product");

            entity.HasOne(d => d.VendorKeyNavigation).WithMany(p => p.FactPurchasings)
                .HasForeignKey(d => d.VendorKey)
                .HasConstraintName("FK_FactPurchasing_Vendor");
        });

        modelBuilder.Entity<FactSale>(entity =>
        {
            entity.HasKey(e => e.SalesKey).HasName("PK__FactSale__C104F6F1CCAD5DCE");

            entity.Property(e => e.LineTotal).HasColumnType("money");
            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.HasOne(d => d.CustomerKeyNavigation).WithMany(p => p.FactSales)
                .HasForeignKey(d => d.CustomerKey)
                .HasConstraintName("FK_FactSales_Customer");

            entity.HasOne(d => d.OrderDateKeyNavigation).WithMany(p => p.FactSales)
                .HasForeignKey(d => d.OrderDateKey)
                .HasConstraintName("FK_FactSales_Date");

            entity.HasOne(d => d.ProductKeyNavigation).WithMany(p => p.FactSales)
                .HasForeignKey(d => d.ProductKey)
                .HasConstraintName("FK_FactSales_Product");

            entity.HasOne(d => d.TerritoryKeyNavigation).WithMany(p => p.FactSales)
                .HasForeignKey(d => d.TerritoryKey)
                .HasConstraintName("FK_FactSales_Territory");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
