using GQ.Data.dto;
using System;

namespace GQ.Sql
{
    /// REVISAR
    ///http://ayende.com/blog/4137/nhibernate-perf-tricks

    /// <summary>
    /// Clase base de servicios de datos para NHibernate
    /// </summary>
    public class BaseService : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ISessionMapper<T> GetSession<T>() where T : class, IEntity, new()
        {
            return GetSession<T>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public ISessionMapper<T> GetSession<T>(dynamic data) where T : class, IEntity, new()
        {
            return ServiceDBConfigure.Service.GetSession<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }
    }
}
