using GQ.Data.dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GQ.Sql
{
    /// <summary>
    /// Clase generica con metos basicos de servicios de busqueda en base de datos
    /// </summary>
    /// <typeparam name="T">Entidad de base de datos</typeparam>
    public class GenericService<T> : BaseService where T : class, IEntity, new()
    {
        private ISessionMapper<T> _SessionMode = null;
        protected ISessionMapper<T> SessionMode
        {
            get
            {
                if (_SessionMode == null)
                {
                    _SessionMode = GetSession<T>();
                }
                return _SessionMode;
            }
        }

        #region Métodos ABM
        /// <summary>
        ///  Agrega una entidad a la base de datos
        /// </summary>
        /// <param name="pObj">Entidad</param>
        /// <returns>Entidad actualizada</returns>
        public virtual T Agregar(T pObj)
        {
            if (SessionMode != null)
            {
                SessionMode.Insert(pObj);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return pObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pObj"></param>
        /// <returns></returns>
        public virtual bool Agregar(IEnumerable<T> pObj)
        {
            if (SessionMode != null)
            {
                SessionMode.Insert(pObj);
            }
            else
            {
                throw new Exception("No tiene session");
            }

            return true;
        }

        /// <summary>
        ///  Actualiza una entidad a la base de datos
        /// </summary>
        /// <param name="pObj">Entidad</param>
        /// <returns>Entidad actualizada</returns>
        public virtual T Actualizar(T pObj)
        {
            if (SessionMode != null)
            {
                SessionMode.Update(pObj);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return pObj;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pObj"></param>
        /// <returns></returns>
        public virtual bool Actualizar(IEnumerable<T> pObj)
        {
            if (SessionMode != null)
            {
                SessionMode.Update(pObj);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return true;
        }

        /// <summary>
        ///  Borra una entidad a la base de datos
        /// </summary>
        /// <param name="pObj">Entidad</param>
        /// <returns>True = se pudo borrar</returns>
        public virtual bool Borrar(T pObj)
        {
            if (SessionMode != null)
            {
                SessionMode.Delete(pObj);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pObj"></param>
        /// <returns></returns>
        public virtual bool Borrar(IEnumerable<T> pObj)
        {
            if (SessionMode != null)
            {
                SessionMode.Delete(pObj);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return true;
        }
        #endregion

        #region Métodos de Consulta

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Exist(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return Count(expression) > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T findById(long? id)
        {
            T result = null;
            if (SessionMode != null)
            {
                result = SessionMode.findById(id.Value);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return result;
        }

        /// <summary>
        /// Busca Las entidaddes 
        /// </summary>
        /// <returns>Lista con los resultados</returns>
        public virtual IQueryable<T> findBy()
        {
            IQueryable<T> result = null;

            if (SessionMode != null)
            {
                result = SessionMode.findBy();
            }
            else
            {
                throw new Exception("No tiene session");
            }

            return result;
        }

        /// <summary>
        /// Busca los registros
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns></returns>
        public virtual IQueryable<T> findBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            IQueryable<T> result = null;

            if (SessionMode != null)
            {
                result = SessionMode.findBy(expression);
            }
            else
            {
                throw new Exception("No tiene session");
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual T findByOne(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            T result = null;
            if (SessionMode != null)
            {
                result = SessionMode.findByOne(expression);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public dynamic findBySql(string sql)
        {
            dynamic result = null;
            if (SessionMode != null)
            {
                result = SessionMode.findBySql(sql);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="returnAlias"></param>
        /// <param name="returnClass"></param>
        /// <returns></returns>
        public dynamic findBySql(string sql, string returnAlias, Type returnClass)
        {
            dynamic result = null;
            if (SessionMode != null)
            {
                result = SessionMode.findBySql(sql, returnAlias, returnClass);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="returnAlias"></param>
        /// <param name="returnClass"></param>
        /// <returns></returns>
        public dynamic findBySql(string sql, string[] returnAlias, Type[] returnClass)
        {
            dynamic result = null;
            if (SessionMode != null)
            {
                result = SessionMode.findBySql(sql, returnAlias, returnClass);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long Count()
        {
            long result = -1;
            if (SessionMode != null)
            {
                result = SessionMode.Count();
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public long Count(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            long result = -1;
            if (SessionMode != null)
            {
                result = SessionMode.Count(expression);
            }
            else
            {
                throw new Exception("No tiene session");
            }
            return result;
        }

        #endregion
    }
}
