namespace CityInfo.Infrastructure.Mongo
{
    public sealed class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ReviewsCollectionName { get; set; } = "Reviews";
    }
}
