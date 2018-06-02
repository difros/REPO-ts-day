using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GQ.NoSql.Mongo
{
    public class MongoDbManager : BaseDBManager, IDocumentDB
    {
        protected static MongoDbConfig Config;

        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public MongoDbManager(IDocumentConfig config)
        {
            Config = (MongoDbConfig)config;
        }

        public IMongoClient GetClient()
        {
            if (_client == null)
            {
                _client = new MongoClient(Config.ConnectionString);
            }
            return _client;
        }

        public IMongoDatabase GetDataBase()
        {
            if (_database == null)
            {
                _database = GetClient().GetDatabase(Config.DBName);
            }
            return _database;
        }
        //IMongoCollection<T>
        public object GetCollection<T>() where T : IDocumentEntity
        {
            string CollectionName = GetCollectionName(typeof(T));
            return GetDataBase().GetCollection<T>(CollectionName);
        }

        public void DropCollection<T>() where T : IDocumentEntity
        {
            string CollectionName = GetCollectionName(typeof(T));
            GetDataBase().DropCollection(CollectionName);
        }
    }

    public static class BsonClassMapHelper
    {
        public static void Unregister<T>()
        {
            var classType = typeof(T);
            GetClassMap().Remove(classType);
        }

        public static void Unregister(string name)
        {
            Log.Log.GetLog().Info("BsonClassMapHelper.Unregister : " + name);
            var cm = GetClassMap();
            foreach (var type in cm.Keys)
            {
                if (type.Namespace + "." + type.Name == name)
                {
                    Log.Log.GetLog().Info("BsonClassMapHelper.Unregister : " + name + " OK");
                    cm.Remove(type);
                    break;
                }
            }
            var ClassName = name.Split('.')[name.Split('.').Length - 1];
            var d = GetDiscriminators();
            foreach (var type in d.Keys)
            {
                if (type.ToString() == ClassName)
                {
                    Log.Log.GetLog().Info("BsonClassMapHelper.Unregister : " + name + " OK");
                    d.Remove(type);
                    break;
                }
            }
            var dt = GetDiscriminatedTypes();
            foreach (var type in dt)
            {
                if (type.Namespace + "." + type.Name == name)
                {
                    Log.Log.GetLog().Info("BsonClassMapHelper.Unregister : " + name + " OK");
                    dt.Remove(type);
                    break;
                }
            }
        }

        static Dictionary<Type, BsonClassMap> GetClassMap()
        {
            var cm = BsonClassMap.GetRegisteredClassMaps().First();
            var fi = typeof(BsonClassMap).GetField("__classMaps", BindingFlags.Static | BindingFlags.NonPublic);
            var classMaps = (Dictionary<Type, BsonClassMap>)fi.GetValue(cm);
            return classMaps;
        }

        static Dictionary<BsonValue, HashSet<Type>> GetDiscriminators()
        {
            //var cm = BsonSerializer..First();
            var fi = typeof(BsonSerializer).GetField("__discriminators", BindingFlags.Static | BindingFlags.NonPublic);
            var classMaps = (Dictionary<BsonValue, HashSet<Type>>)fi.GetValue(null);
            return classMaps;
        }

        static HashSet<Type> GetDiscriminatedTypes()
        {
            //var cm = BsonSerializer..First();
            var fi = typeof(BsonSerializer).GetField("__discriminatedTypes", BindingFlags.Static | BindingFlags.NonPublic);
            var classMaps = (HashSet<Type>)fi.GetValue(null);
            return classMaps;
        }
        

        public static void Clear()
        {
            GetClassMap().Clear();
        }
    }
}
