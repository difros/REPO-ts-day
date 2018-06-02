using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace GQ.Data
{
    public class Paging : IPaging
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public long? PageCount { get; set; }
        public long? RecordCount { get; set; }
        public List<PagingFilter> Filter { get; set; } = new List<PagingFilter>();
        public List<PagingOrder> Order { get; set; } = new List<PagingOrder>();
        public IEnumerable Data { get; set; }

        public virtual void Apply(IQueryable source)
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
                                case "in":
                                    {
                                        source = source.Where(item.Property + " in (@0)", item.Value.ToString());
                                        break;
                                    }
                                case "inArray":
                                    {
                                        var array = ((List<long>)item.Value).ToList();
                                        source = source.Where("@0.Contains(" + item.Property + ")", array);
                                        break;
                                    }
                                case "con":
                                    {
                                        source = source.Where(item.Property + ".Contains(@0)", item.Value.ToString());
                                        break;
                                    }
                                case "x":
                                    {
                                        break;
                                    }
                                case "=|T":
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

            var SourceData = source.Skip((PageIndex.Value - 1) * PageSize.Value).Take(PageSize.Value).ToDynamicList();

            var data = new ArrayList();

            data.AddRange((ICollection)SourceData);

            Data = data;
        }
    }
}
