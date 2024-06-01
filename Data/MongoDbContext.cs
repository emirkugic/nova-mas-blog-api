using MongoDB.Driver;
using nova_mas_blog_api.Models;

namespace blog_website_api.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            var databaseName = configuration.GetSection("MongoDb:DatabaseName").Value;
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    }
}
