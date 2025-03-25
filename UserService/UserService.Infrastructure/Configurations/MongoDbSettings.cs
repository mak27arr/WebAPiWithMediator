namespace UserService.Infrastructure.Configurations
{
    internal class MongoDbSettings
    {
        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
    }
}
