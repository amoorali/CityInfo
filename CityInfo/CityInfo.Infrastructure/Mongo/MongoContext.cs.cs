using MongoDB.Driver;

namespace CityInfo.Infrastructure.Mongo
{
    public sealed class MongoContext
    {
        public IMongoDatabase Database { get; }

        public MongoContext(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            Database = client.GetDatabase(settings.DatabaseName);
        }
    }
}
