using GQ.Core.service;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GQ.NoSql.Mongo
{
    public class MongoDbServices
    {
        private DateTime recicledTime;
        private IDocumentDB db;
        private IDocumentConfig Config;

        public static MongoDbServices Instance(Type type = null)
        {
            if (type == null)
                return ServicesContainer.GetService<MongoDbServices>();
            return (MongoDbServices)ServicesContainer.GetService(type);
        }

        public MongoDbServices(IDocumentConfig Config)
        {
            this.Config = Config;
        }

        public static void Configure(IDocumentConfig config)
        {
            ServicesContainer.AddScoped<MongoDbServices>((sp) =>
            {
                return new MongoDbServices(config);
            });
        }

        public IDocumentDB GetDB()
        {
            if (db == null || recicledTime.Ticks < DateTime.Now.Ticks)
            {
                recicledTime = DateTime.Now.AddSeconds(Config.RecicledTime);
                db = new MongoDbManager(Config);
            }
            return db;
        }

        public IMongoCollection<T> GetCollection<T>() where T : IDocumentEntity
        {
            return (IMongoCollection<T>)GetDB().GetCollection<T>();
        }

        public void DropCollection<T>() where T : IDocumentEntity
        {
            GetDB().DropCollection<T>();
        }

        public UpdateDefinition<T> GetUpdate<T>(T value)
        {
            UpdateDefinition<T> update = null;
            var properties = value.GetType().GetProperties();
            foreach (PropertyInfo porperty in properties)
            {
                var addPropertir = !porperty.Name.Equals("Id");
                if (addPropertir)
                {
                    if (porperty.PropertyType == typeof(List<string>))
                    {
                        if (update == null)
                            update = Builders<T>.Update.Set<List<string>>(porperty.Name, (List<string>)porperty.GetValue(value));
                        else
                            update = update.Set<T, List<string>>(porperty.Name, (List<string>)porperty.GetValue(value));
                    }
                    else
                    {
                        if (update == null)
                            update = Builders<T>.Update.Set(porperty.Name, porperty.GetValue(value));
                        else
                            update = update.Set(porperty.Name, porperty.GetValue(value));
                    }
                }
            }
            return update;
        }
    }
}
