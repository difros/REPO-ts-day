using GQ.Data;
using GQ.Data.dto;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace GQ.Sql
{
    /// <summary>
    /// Permite paginar una busqueda de la base de datos
    /// </summary>
    public class Paging : IPaging
    {
        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_IN = "in";

        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_IN_ARRAY = "inArray";

        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_CON = "con";

        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_MATCH = "match";

        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_T = "=|T";

        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_NONE = "x";

        /// <summary>
        /// Pagina que va a leer valor minimo 1
        /// </summary>
        public virtual int? PageIndex { get; set; }
        /// <summary>
        /// Tamaño de la pagina valor minimo 1
        /// </summary>
        public virtual int? PageSize { get; set; }
        /// <summary>
        /// Total de paginas
        /// </summary>
        public virtual long? PageCount { get; set; }
        /// <summary>
        /// Catidad de registros
        /// </summary>
        public virtual long? RecordCount { get; set; }
        /// <summary>
        /// Filtro de los resultados
        /// </summary>
        public List<PagingFilter> Filter { get; set; } = new List<PagingFilter>();
        /// <summary>
        /// Orden de los resultados
        /// </summary>
        public List<PagingOrder> Order { get; set; } = new List<PagingOrder>();
        /// <summary>
        /// Datos que va a mostrar
        /// </summary>
        public IEnumerable Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Paging()
        {
            PageIndex = 1;
            PageSize = 25;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        public Paging(int PageIndex, int PageSize)
        {
            this.PageIndex = PageIndex;
            this.PageSize = PageSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Entidad</typeparam>
        /// <typeparam name="E">Entidad IDto</typeparam>
        /// <param name="source">Consulta Linq</param>
        public void Apply<T, E>(IQueryable<T> source)
            where T : IEntity, new()
            where E : IDto<T, E>, new()
        {

            if (Filter != null)
            {
                //Aplicar Filtro
                if (Filter.Count > 0)
                {
                    foreach (var item in Filter)
                    {
                        if (item.Condition != null)
                        {
                            switch (item.Condition.ToString())
                            {
                                case FILTER_IN:
                                    {
                                        source = source.Where(item.Property + " in (@0)", item.Value.ToString());
                                        break;
                                    }
                                case FILTER_IN_ARRAY:
                                    {
                                        var array = ((List<long>)item.Value).ToList();
                                        source = source.Where("@0.Contains(" + item.Property + ")", array);
                                        break;
                                    }
                                case FILTER_CON:
                                    {
                                        source = source.Where(item.Property + ".Contains(@0)", item.Value.ToString());
                                        break;
                                    }
                                case FILTER_NONE:
                                    {
                                        break;
                                    }
                                case FILTER_T:
                                    {
                                        if (item.Value.ToString() != "T") source = source.Where(item.Property + " = @0 ", item.Value);
                                        break;
                                    }
                                default:
                                    {
                                        source = source.Where(item.Property + " " + item.Condition.ToString() + " @0 ", item.Value);
                                        break;
                                    }
                            }

                        }
                    }
                }
                int total = source.Count();
                this.RecordCount = total;
                this.PageCount = total / PageSize;

                if (total % PageSize > 0) PageCount++;
            }

            if (Order != null)
            {
                ////Aplicar Orden
                if (Order.Count > 0)
                {
                    string orderBy = "";
                    foreach (var item in Order)
                    {
                        orderBy = orderBy + item.Property + " " + (item.Direction == "+" ? "asc" : "desc") + ",";
                    }
                    orderBy = orderBy.Substring(0, orderBy.Length - 1);
                    source = source.OrderBy(orderBy);
                }
            }

            List<T> SourceData = source.Skip((PageIndex.Value - 1) * PageSize.Value).Take(PageSize.Value).ToList();

            var data = new ArrayList();

            E items = new E();

            sourceData = items.makeDto(SourceData);

            data.AddRange((ICollection)sourceData);

            Data = data;
        }

        IList sourceData = null;
        public IList GetSourceData()
        {
            return sourceData;
        }
    }
}
