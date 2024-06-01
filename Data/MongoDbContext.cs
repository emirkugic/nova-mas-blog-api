using MongoDB.Driver;
using nova_mas_blog_api.Models;

namespace nova_mas_blog_api.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            var databaseName = configuration["ConnectionStrings:DatabaseName"];

            // if (string.IsNullOrWhiteSpace(connectionString))
            // {
            //     throw new InvalidOperationException("MongoDB connection string is not configured.");
            // }
            // if (string.IsNullOrWhiteSpace(databaseName))
            // {
            //     throw new InvalidOperationException("MongoDB database name is not configured.");
            // }

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    }
}
