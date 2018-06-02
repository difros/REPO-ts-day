using GQ.Data.dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace GQ.Sql.SQLServer
{
    public class SQLServerMapper<T> : ISessionMapper<T> where T : class, IEntity, new()
    {
        public DbSet<T> Session { get; private set; }
        public SQLServerService Context { get; private set; }

        public SQLServerMapper(DbSet<T> db, SQLServerService context)
        {
            Session = db;
            Context = context;
        }

        public IEnumerable<PropertyInfo> GetPrimary()
        {
            var poperties = typeof(T).GetProperties().Where(x => x.GetCustomAttributes(typeof(KeyAttribute), true).Any());
            return poperties;
        }

        public void Reload(T value)
        {
            Context.Entry<T>(value).Reload();
        }

        public long Count()
        {
            return Session.Count();
        }

        public long Count(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return Session.Where(expression).Count();
        }

        public bool Delete(T value)
        {
            Session.Remove(value);
            Context.SaveChanges();
            return true;
        }

        public bool Delete(IEnumerable<T> values)
        {
            foreach (var item in values)
                Delete(item);
            return true;
        }

        public IQueryable<T> findBy()
        {
            return Session;
        }

        public IQueryable<T> findBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return Session.Where(expression);
        }

        public T findById(params object[] values)
        {
            IQueryable<T> source = Session;
            var index = 0;
            foreach (var pro in GetPrimary())
            {
                source = source.Where(pro.Name + " = @0", values[index]);
                index++;
            }
            return source.FirstOrDefault();
        }

        public T findByOne(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return Session.Where(expression).FirstOrDefault();
        }

        public dynamic findBySql(string sql)
        {
            return Session.FromSql<T>(sql);
        }

        public dynamic findBySql(string sql, string returnAlias, Type returnClass)
        {
            throw new NotImplementedException();
        }

        public dynamic findBySql(string sql, string[] returnAlias, Type[] returnClass)
        {
            throw new NotImplementedException();
        }

        public T Insert(T value)
        {
            Session.Add(value);
            Context.SaveChanges();
            return value;
        }

        public bool Insert(IEnumerable<T> values)
        {
            foreach (var item in values)
                Insert(item);
            return true;
        }

        public T Update(T value)
        {
            Session.Update(value);
            Context.SaveChanges();
            return value;
        }

        public bool Update(IEnumerable<T> values)
        {
            foreach (var item in values)
                Update(item);
            return true;
        }
    }
}
