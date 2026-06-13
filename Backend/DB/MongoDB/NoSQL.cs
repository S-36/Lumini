using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace Backend.DB.MongoDB
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }

    public interface IMongoContext
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }

    public class MongoDBContext : IMongoContext
    {
        private readonly IMongoDatabase _database;

        //Usin IOptions to inject the settings from program.cs
        public MongoDBContext(IOptions<MongoDBSettings> settings)
        {
            
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        } 
    }
}