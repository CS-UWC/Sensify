using MongoDB.Driver;

namespace Sensify.Persistence;

public interface IMongoPersistenceProvider
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
}
