using MongoDB.Bson;
using System.Collections.Generic;

namespace GQ.NoSql.Mongo
{
    public static class PagingOperatorExtends
    {
        public static PagingOperator Value(this PagingOperator value, string key)
        {
            return (PagingOperator)value[key];
        }
    }

    public class PagingOperator : Dictionary<string, object>
    {
        public PagingOperator():base()
        {

        }

        public PagingOperator(string key, object value) : base()
        {
            this.Add(key, value);
        }

        public virtual BsonDocument ToBsonDocument()
        {
            return new BsonDocument();
        }
    }
}
