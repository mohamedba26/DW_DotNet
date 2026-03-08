using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using SalesDW.API.Models.ProductioDB;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SalesDW.API.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<AuthUser> Users { get; set; } = null!;
    public DbSet<AuthProduct> Products { get; set; } = null!;
    public DbSet<Command> Commands { get; set; } = null!;
    public DbSet<CommandLine> CommandLines { get; set; } = null!;

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
            entity.Property(e => e.Image).HasColumnType("nvarchar(max)");
        });

        modelBuilder.Entity<Command>(entity =>
        {
            entity.HasKey(e => e.CommandId);
            entity.ToTable("Commands");
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Approved).HasDefaultValue(0);
            entity.HasMany(e => e.CommandLines)
                  .WithOne(cl => cl.Command)
                  .HasForeignKey(cl => cl.CommandId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CommandLine>(entity =>
        {
            entity.HasKey(e => e.CommandLineId);
            entity.ToTable("CommandLines");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.HasIndex(e => e.ProductId);
        });

        base.OnModelCreating(modelBuilder);
    }
}

