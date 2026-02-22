using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using SalesDW.API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SalesDW.API.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<AuthUser> Users { get; set; } = null!;
    public DbSet<AuthProduct> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Salt).HasMaxLength(200).HasDefaultValue("");
            entity.Property(e => e.Role).HasDefaultValue(2);
        });

        modelBuilder.Entity<AuthProduct>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProductName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Subcategory).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.StandardCost).HasColumnType("money");
            entity.Property(e => e.ListPrice).HasColumnType("money");
        });

        base.OnModelCreating(modelBuilder);
    }
}

