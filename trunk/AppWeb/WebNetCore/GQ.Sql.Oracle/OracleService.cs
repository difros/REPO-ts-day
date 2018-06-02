using GQ.Core.service;
using GQ.Data.dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using global::Oracle.ManagedDataAccess.Client;

/// <summary>
/// https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro
/// https://docs.microsoft.com/en-us/ef/core/modeling/relational/data-types
/// https://docs.microsoft.com/en-us/ef/core/
/// </summary>
namespace GQ.Sql.Oracle
{
    public class OracleService : DbContext, IBaseService
    {
        public static OracleServiceDBConfiguration Options;
        public Dictionary<Type, object> DbSetCollection { get; private set; } = new Dictionary<Type, object>();

        public OracleService() : base(ServicesContainer.GetService<OracleServiceDBConfiguration>().Options)
        {
            Options = ServicesContainer.GetService<OracleServiceDBConfiguration>();

            var method = this.GetType().GetMethod("Set");

            var result = Options.Assembly.GetTypes().Where(x => x.GetInterface("IEntityOracle", true) != null);

            foreach (var item in result)
            {
                var db = method.MakeGenericMethod(item).Invoke(this, null);
                DbSetCollection.Add(item, db);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /// ERROR CONECTOR
            //optionsBuilder.UseOracle(Options.ConnectionString, b => b.MigrationsAssembly(Options.Assembly.FullName));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var result = Options.Assembly.GetTypes().Where(x => x.GetInterface("IEntityOracle", true) != null);
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
            return new OracleMapper<T>((DbSet<T>)DbSetCollection[typeof(T)], this);
        }

        public void Migrate()
        {
            this.Database.Migrate();
        }

        public static IBaseService CreateNew
        {
            get { return new OracleService(); }
        }

        public static IBaseService Instance
        {
            get { return ServicesContainer.GetService<OracleService>(); }
        }
    }
}
