using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GQ.NoSql.Mongo
{
    public class MongoDbEntity : IDocumentEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }

        public virtual object GetId()
        {
            return Id;
        }
    }
}
