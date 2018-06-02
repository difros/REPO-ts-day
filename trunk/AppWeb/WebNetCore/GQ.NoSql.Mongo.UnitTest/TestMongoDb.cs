using GQ.Core.service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace GQ.NoSql.Mongo.UnitTest
{
    [TestClass]
    public class TestMongoDb
    {

        public void IniciarMongo()
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

            IHostingEnvironment Environment = new HostingEnvironment();
            ServicesContainer.AddHostingEnvironment(Environment);

            var Configuration = ServicesContainer.ConfigurationBuilder();

            ServicesContainer.AddServices(new Microsoft.Extensions.DependencyInjection.ServiceCollection());

            MongoDbServices.Configure(new MongoDbConfig { ConnectionString = "mongodb://draromas:2cfnSapoIJmlBc6fScO4IDLyIGVKx7nJezxJH734E4yMrYQRm0aFo3pqwNG5uHNUIogUbIpfQqqQ5o5Rp1pTDQ==@draromas.documents.azure.com:10255/?ssl=true&replicaSet=globaldb&connectTimeoutMS=120000&socketTimeoutMS=120000&waitQueueTimeoutMS=120000&maxIdleTimeMS=600000", DBName = "IOTServices" });
            //MongoDbServices.Configure(new MongoDbConfig { ConnectionString = "mongodb://IOTUser:g3m1nusQh0m!@gq-test2.cloudapp.net:27017/IOTServices?connectTimeoutMS=120000&socketTimeoutMS=120000&waitQueueTimeoutMS=120000&maxIdleTimeMS=600000&authMechanism=SCRAM-SHA-1", DBName = "IOTServices" });

            ServicesContainer.BuildServiceProvider();
        }

        [TestMethod]
        public void MongoDB_Find_Date()
        {
            IniciarMongo();

            Paging paging;

            paging = new Paging();
           
            paging.PageSize = int.MaxValue;
            
            paging.Filter.Add(new Data.PagingFilter { Property = "Fecha", Condition = ">=", Value = new DateTime(2018, 4, 2) });
            paging.Filter.Add(new Data.PagingFilter { Property = "Fecha", Condition = "<=", Value = (new DateTime(2018, 4, 2)).AddDays(1) });

            paging = Newtonsoft.Json.JsonConvert.DeserializeObject<Paging>(Newtonsoft.Json.JsonConvert.SerializeObject(paging));
            paging.CreateFilter = () =>
            {
                FilterDefinition<BsonDocument> filter = null;

                filter = Builders<BsonDocument>.Filter.Eq("EmpresaId", "5a6734af2ec8d0fd3b56e96e");

                var arr = new List<string>();
                arr.Add("5a9833cbceb2ad10ec24ad50");
                filter = filter & Builders<BsonDocument>.Filter.Eq("EquipoId", "5a9833cbceb2ad10ec24ad50");

                return filter;
            };
            paging.Apply<BsonDocument>("IOT_Equipo_Estados");

            if (paging.RecordCount == 0)
            {
                throw new Exception("Error =");
            }
        }

        [TestMethod]
        public void MongoDB_Find_In()
        {
            IniciarMongo();

            Paging paging;

            paging = new Paging();
            paging.Filter.Add(new Data.PagingFilter { Property = "LocacionId", Condition = "=", Value = 1 });

            paging = Newtonsoft.Json.JsonConvert.DeserializeObject<Paging>(Newtonsoft.Json.JsonConvert.SerializeObject(paging));

            paging.Apply<BsonDocument>("IOT_Equipos");

            if (paging.RecordCount == 0)
            {
                throw new Exception("Error =");
            }

            paging = new Paging();
            paging.Filter.Add(new Data.PagingFilter { Property = "Nombre", Condition = "=", Value = "Doctor Aromas" });

            paging = Newtonsoft.Json.JsonConvert.DeserializeObject<Paging>(Newtonsoft.Json.JsonConvert.SerializeObject(paging));

            paging.Apply<BsonDocument>("IOT_Equipos");

            if (paging.RecordCount == 0)
            {
                throw new Exception("Error =");
            }

            paging = new Paging();
            paging.Filter.Add(new Data.PagingFilter { Property = "LocacionId", Condition = Paging.FILTER_IN, Value = new List<object> { 1, 2, 3, 4, 5 }, ValueType = "System.Collections.Generic.List`1[System.Object]" });

            paging = Newtonsoft.Json.JsonConvert.DeserializeObject<Paging>(Newtonsoft.Json.JsonConvert.SerializeObject(paging));

            paging.Apply<BsonDocument>("IOT_Equipos");

            if (paging.RecordCount == 0)
            {
                throw new Exception("Error Paging.FILTER_IN");
            }


        }

        [TestMethod]
        public void MongoDB_Paging_Group()
        {
            IniciarMongo();

            var paging = new Paging();
            paging.IsAggregate = true;
            paging.Filter.Add(new Data.PagingFilter { Property = "IdentificacionCliente", Condition = "=", Value = 4 });
            paging.Filter.Add(new Data.PagingFilter { Property = "Fecha", Condition = ">", Value = new DateTime(2018, 01, 1) });

            paging.Order.Add(new Data.PagingOrder { Direction = "+", Property = "Fecha" });

            paging.GroupBy = new PagingOperator("_id", new PagingOperator("Year", new PagingOperator("$year", "$Fecha")));
            paging.GroupBy.Value("_id").Add("Month", new PagingOperator("$month", "$Fecha"));
            paging.GroupBy.Value("_id").Add("Day", new PagingOperator("$dayOfMonth", "$Fecha"));
            paging.GroupBy.Value("_id").Add("Hour", new PagingOperator("$hour", "$Fecha"));
            paging.GroupBy.Value("_id").Add("Minute", new PagingOperator("$subtract", new PagingOperator[] { new PagingOperator("$minute", "$Fecha"), new PagingOperator("$mod", new object[] { new PagingOperator("$minute", "$Fecha"), 10 }) }));
            paging.GroupBy.Add("count", new PagingOperator("$sum", 1));

            /*paging.GroupBy.Add(new PagingOperator
            {
                Name = "_id",
                Value =
                {
                    new PagingOperator { Name = "Year", Value={ new PagingOperator {Name= "$year",Value = { new PagingOperator { Name = "$Fecha" } } } } } ,
                    new PagingOperator { Name = "Month", Value = { new PagingOperator { Name = "$month", Value = { new PagingOperator { Name = "$Fecha" } } } }},
                }
            });*/

            paging = Newtonsoft.Json.JsonConvert.DeserializeObject<Paging>(Newtonsoft.Json.JsonConvert.SerializeObject(paging));

            paging.Apply<BsonDocument>("IOT_Equipos");

            var result = paging.Data;

            /*
db.getCollection('IOT_Equipo_Estados').aggregate(
    [
        {
            $match :
            {
                "IdentificacionCliente" : 4
            }
        },
        {
            $group : 
            {
                _id :
                {
                    year:{$year :"$Fecha"},
                    month:{$month :"$Fecha"},
                    day:{$dayOfMonth :"$Fecha"},
                    hour:{$hour :"$Fecha"},
                    minute:{$subtract:[{$minute :"$Fecha"},{$mod:[{$minute :"$Fecha"},10]}]},
                },
                Valor1:{$avg:"$Valor1"},
                count:{$sum:1}
            }
        }
    ]
)

             * */



        }


        public class HostingEnvironment : IHostingEnvironment
        {
            public string EnvironmentName { get; set; }
            public string ApplicationName { get; set; }
            public string WebRootPath { get; set; }
            public IFileProvider WebRootFileProvider { get; set; }
            public string ContentRootPath { get; set; } = @"D:\PROYECTOS\GQBase\code\web\net\src\trunk\AppWeb\WebNetCore\WebNetCore\";
            public IFileProvider ContentRootFileProvider { get; set; }
        }
    }
}
