using GQ.Core.service;
using GQ.Data.dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro
/// https://docs.microsoft.com/en-us/ef/core/modeling/relational/data-types
/// https://docs.microsoft.com/en-us/ef/core/
/// </summary>
namespace GQ.Sql.SQLServer
{
    public class SQLServerService : DbContext, IBaseService
    {
        public static SQLServerServiceDBConfiguration Options;
        public Dictionary<Type, object> DbSetCollection { get; private set; } = new Dictionary<Type, object>();

        public SQLServerService() : base(ServicesContainer.GetService<SQLServerServiceDBConfiguration>().Options)
        {
            Options = ServicesContainer.GetService<SQLServerServiceDBConfiguration>();

            var method = this.GetType().GetMethod("Set");

            var result = Options.Assembly.GetTypes().Where(x => x.GetInterface("IEntitySQLServer", true) != null);

            foreach (var item in result)
            {
                var db = method.MakeGenericMethod(item).Invoke(this, null);
                DbSetCollection.Add(item, db);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Options.ConnectionString, b => b.MigrationsAssembly(Options.Assembly.FullName));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var result = Options.Assembly.GetTypes().Where(x => x.GetInterface("IEntitySQLServer", true) != null);
            foreach (var key in result)
            {
                var etb = modelBuilder.Entity(key);
            }
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            Database.RollbackTransaction();
        }

        public void CommitTransaction()
        {
            Database.CommitTransaction();
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            return base.SaveChanges();
        }

        public ISessionMapper<T> GetSession<T>() where T : class, IEntity, new()
        {
            return new SQLServerMapper<T>((DbSet<T>)DbSetCollection[typeof(T)], this);
        }

        public void Migrate()
        {
            this.Database.Migrate();
        }

        public static IBaseService CreateNew
        {
            get { return new SQLServerService(); }
        }

        public static IBaseService Instance
        {
            get { return ServicesContainer.GetService<SQLServerService>(); }
        }
    }
}
