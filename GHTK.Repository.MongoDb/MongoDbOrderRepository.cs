using GHTK.Repository.Abstractions.Entity;
using MongoDB.Driver;

namespace GHTK.Repository;

public class MongoDbOrderRepository : IOrderRepository
{
    private readonly MongoClient mongoClient;
    private readonly IMongoDatabase database;
    private readonly IMongoCollection<Order> orders;
    public MongoDbOrderRepository(MongoClient mongoClient,IMongoDatabase mongoDatabase, IMongoCollection<Order>orders)
    {
        this.mongoClient = mongoClient;
        this.database = mongoDatabase;
        this.orders = orders;
    }

    public Task<Order> FindOrderAsync(string TrackingId, string? partnerId)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.TrackingId, TrackingId);
        if (!string.IsNullOrEmpty(partnerId))
        {
            filter = Builders<Order>.Filter.And(filter, Builders<Order>.Filter.Eq(o => o.PartnerId, partnerId));
        }
        return this.orders.Find(filter).FirstOrDefaultAsync();
    }

    async Task IOrderRepository.CreateOrderAsync(Order orderEntity)
    {
        await this.orders.InsertOneAsync(orderEntity);
    }
}
