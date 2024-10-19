using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Sensify.Persistence;

public sealed class MongoPersistenceProvider : IMongoPersistenceProvider
{
    private readonly MongoClient _client;
    public MongoPersistenceProvider(string connectionString)
    {
        _client = new MongoClient(MongoClientSettings.FromConnectionString(connectionString));
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _client.GetDatabase("sensify").GetCollection<T>(collectionName);
    }
}
