using GHTK.Repository.Abstractions.Entity;
using MongoDB.Driver;

public static class MongoDbClientExtensions
{
    public static void AddMongoDbClient(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("MongoDb")
            ?? throw new ArgumentNullException("MongoDb connection string not found in configuration.");

        var databaseName = config["MongoDbSettings:DatabaseName"]
            ?? throw new ArgumentNullException("MongoDbSettings:DatabaseName not found in configuration.");

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var ordersCollection = database.GetCollection<Order>("Orders");

        services.AddSingleton(client);                       // MongoClient
        services.AddSingleton(database);                     // IMongoDatabase
        services.AddSingleton(ordersCollection);             // IMongoCollection<Order>
    }
}
