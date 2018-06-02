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
namespace GQ.Sql.MySQL
{
    public class MySQLService : DbContext, IBaseService
    {
        public static MySQLServiceDBConfiguration Options;
        public Dictionary<Type, object> DbSetCollection { get; private set; } = new Dictionary<Type, object>();

        public MySQLService() : base(ServicesContainer.GetService<MySQLServiceDBConfiguration>().Options)
        {
            Options = ServicesContainer.GetService<MySQLServiceDBConfiguration>();

            var method = this.GetType().GetMethod("Set");

            var result = Options.Assembly.GetTypes().Where(x => x.GetInterface("IEntityMySQL", true) != null);

            foreach (var item in result)
            {
                DbSetCollection.Add(item, method.MakeGenericMethod(item).Invoke(this, null));
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Options.ConnectionString, b => b.MigrationsAssembly(Options.Assembly.FullName));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var result = Options.Assembly.GetTypes().Where(x => x.GetInterface("IEntityMySQL", true) != null);
            foreach (var key in result)
            {
                var db = modelBuilder.Entity(key);
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
            return new MySQLMapper<T>((DbSet<T>)DbSetCollection[typeof(T)], this);
        }

        public void Migrate()
        {
            this.Database.Migrate();
        }

        public static IBaseService CreateNew
        {
            get { return new MySQLService(); }
        }

        public static IBaseService Instance
        {
            get { return ServicesContainer.GetService<MySQLService>(); }
        }
    }
}
