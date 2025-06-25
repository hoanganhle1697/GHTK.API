using GHTK.Repository.Abstractions.Entity;
using Microsoft.EntityFrameworkCore;

namespace GHTK.Repository.SqlServer;

public class SqlServerDbContext : DbContext
{
    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(order =>
        {
            order.OwnsMany(o => o.Products, pb =>
            {
                pb.ToTable("OrderProducts");
                pb.WithOwner().HasForeignKey("OrderId");
                pb.Property<int>("Id");
                pb.HasKey("Id");
            });
        });

        base.OnModelCreating(modelBuilder);
    }
}
