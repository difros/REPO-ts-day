using GQ.Data.dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GQ.Sql
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISessionMapper<T> where T : class, IEntity, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        T Insert(T value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        bool Insert(IEnumerable<T> values);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        T Update(T value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        bool Update(IEnumerable<T> values);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Delete(T value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        bool Delete(IEnumerable<T> values);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        void Reload(T value);

        #region Métodos de Consulta

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T findById(params object[] values);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<T> findBy();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<T> findBy(System.Linq.Expressions.Expression<Func<T, bool>> expression);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        T findByOne(System.Linq.Expressions.Expression<Func<T, bool>> expression);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        dynamic findBySql(string sql);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="returnAlias"></param>
        /// <param name="returnClass"></param>
        /// <returns></returns>
        dynamic findBySql(string sql, string returnAlias, Type returnClass);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="returnAlias"></param>
        /// <param name="returnClass"></param>
        /// <returns></returns>
        dynamic findBySql(string sql, string[] returnAlias, Type[] returnClass);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        long Count(System.Linq.Expressions.Expression<Func<T, bool>> expression);

        #endregion
    }


}
