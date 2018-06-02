namespace GQ.NoSql.Mongo
{
    public class MongoDbConfig : IDocumentConfig
    {
        public string ConnectionString { get; set; }
        public string DBName { get; set; }
        public string TypeDB { get; private set; } = "MongoDb";
        public long RecicledTime { get; set; } = 600;
    }
}
