using GHTK.Repository.Abstractions.Entity;
using Microsoft.EntityFrameworkCore;

namespace GHTK.Repository;

public class SqlServerOrderRepository : IOrderRepository
{
    private readonly SqlServerDbContext dbContext;

    public SqlServerOrderRepository(SqlServerDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreateOrderAsync(Order orderEntity)
    {
        dbContext.Orders.Add(orderEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Order?> FindOrderAsync(string id, string? partnerId)
    {
        var query = dbContext.Orders
            .Include(o => o.Products)
            .AsQueryable();
        query = query.Where(o => o.TrackingId == id);
        if (!string.IsNullOrEmpty(partnerId))
        {
            query = query.Where(o => o.PartnerId == partnerId);
        }
        return await query.FirstOrDefaultAsync();
    }
}
