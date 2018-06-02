using GQ.Data;
using GQ.Data.dto;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GQ.NoSql.Mongo
{
    /// <summary>
    /// Permite paginar una busqueda
    /// </summary>
    public class Paging : IPaging
    {
        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_NOT_IN = "!in";

        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_IN = "in";

        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_NOT_CON = "!con";

        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_CON = "con";

        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_NOT_MATCH = "!match";

        /// <summary>
        /// 
        /// </summary>
        public const string FILTER_MATCH = "match";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public delegate object CreateFilterdelegate();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public delegate object CreateProjectiondelegate();

        /// <summary>
        /// 
        /// </summary>
        public Type IntanceMongoDb { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public int? PageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? PageCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long? RecordCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAggregate { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public List<PagingFilter> Filter { get; set; } = new List<PagingFilter>();

        /// <summary>
        /// 
        /// </summary>
        public PagingOperator GroupBy { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public List<PagingOrder> Order { get; set; } = new List<PagingOrder>();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CreateFilterdelegate CreateFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CreateFilterdelegate CreateProjection { get; set; }

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
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="E"></typeparam>
        public void Apply<T, E>()
            where T : class, IDto<E, T>, new()
            where E : class, IDocumentEntity, IEntity, new()
        {
            FilterDefinition<E> filter = (CreateFilter == null) ? null : (FilterDefinition<E>)CreateFilter?.Invoke();
            ProjectionDefinition<E> projection = (CreateProjection == null) ? null : (ProjectionDefinition<E>)CreateProjection?.Invoke();

            filter = CreateFilters<E>(Filter, filter);

            if (IsAggregate)
            {
                var query = MongoDbServices.Instance(IntanceMongoDb).GetCollection<E>().Aggregate().Match(filter).Group(projection);
            }
            else
            {
                IFindFluent<E, E> query = null;

                if (filter != null)
                    query = MongoDbServices.Instance(IntanceMongoDb).GetCollection<E>().Find(filter);
                else
                    query = MongoDbServices.Instance(IntanceMongoDb).GetCollection<E>().Find(x => true);

                if (projection != null)
                {
                    query.Project(projection);
                }


                var sortB = Builders<E>.Sort;
                SortDefinition<E> sort = null;

                foreach (var item in Order)
                {
                    switch (item.Direction.ToString())
                    {
                        case "-":
                            {
                                if (sort == null)
                                    sort = sortB.Descending(item.Property);
                                else
                                    sort = sort.Descending(item.Property);
                                break;
                            }
                        case "+":
                            {
                                if (sort == null)
                                    sort = sortB.Ascending(item.Property);
                                else
                                    sort = sort.Ascending(item.Property);
                                break;
                            }
                    }
                }

                if (sort != null)
                    query.Sort(sort);

                PageSize = PageSize ?? 1;
                RecordCount = query.Count();
                PageCount = (RecordCount / PageSize);

                PageCount += (PageCount < ((float)RecordCount / (float)PageSize)) ? 1 : 0;
                Data = new T().SetEntity(query.Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList<E>());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CollectionName"></param>
        public void Apply<T>(string CollectionName)
        {
            var collection = ((MongoDbManager)MongoDbServices.Instance().GetDB()).GetDataBase().GetCollection<T>(CollectionName);
            var filterB = Builders<T>.Filter;

            FilterDefinition<T> filter = (CreateFilter == null) ? null : (FilterDefinition<T>)CreateFilter?.Invoke();
            ProjectionDefinition<T> projection = (CreateProjection == null) ? null : (ProjectionDefinition<T>)CreateProjection?.Invoke();

            Apply<T>(collection, filter, projection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="filter"></param>
        /// <param name="projection"></param>
        private void Apply<T>(IMongoCollection<T> collection, FilterDefinition<T> filter, ProjectionDefinition<T> projection)
        {
            filter = CreateFilters<T>(Filter, filter);

            if (IsAggregate)
            {

                var sortB = Builders<T>.Sort;
                SortDefinition<T> sort = null;

                foreach (var item in Order)
                {
                    switch (item.Direction.ToString())
                    {
                        case "-":
                            {
                                if (sort == null)
                                    sort = sortB.Descending(item.Property);
                                else
                                    sort = sort.Descending(item.Property);
                                break;
                            }
                        case "+":
                            {
                                if (sort == null)
                                    sort = sortB.Ascending(item.Property);
                                else
                                    sort = sort.Ascending(item.Property);
                                break;
                            }
                    }
                }

                var Group = BsonDocumentWrapper.Parse(JsonConvert.SerializeObject(GroupBy));

                IAggregateFluent<T> query = null;

                var options = new AggregateOptions();

                if (sort != null)
                    query = collection.Aggregate().Match(filter).Sort(sort).Group<T>(Group).Project<T>(BsonDocumentWrapper.Parse("{'id':'$_id','count':'$count'}"));
                else
                    query = collection.Aggregate().Match(filter).Group<T>(Group).Project<T>(BsonDocumentWrapper.Parse("{'id':'$_id','count':'$count'}"));

                Data = query.ToList<T>();

            }
            else
            {
                IFindFluent<T, T> query = null;

                if (filter != null)
                    query = collection.Find(filter);
                else
                    query = collection.Find(x => true);

                if (projection != null)
                {
                    query = query.Project<T>(projection);
                }

                var sortB = Builders<T>.Sort;
                SortDefinition<T> sort = null;

                foreach (var item in Order)
                {
                    switch (item.Direction.ToString())
                    {
                        case "-":
                            {
                                if (sort == null)
                                    sort = sortB.Descending(item.Property);
                                else
                                    sort = sort.Descending(item.Property);
                                break;
                            }
                        case "+":
                            {
                                if (sort == null)
                                    sort = sortB.Ascending(item.Property);
                                else
                                    sort = sort.Ascending(item.Property);
                                break;
                            }
                    }
                }

                if (sort != null)
                    query.Sort(sort);

                PageSize = PageSize ?? 1;
                RecordCount = query.Count();
                if (PageSize != int.MaxValue)
                {
                    PageCount = (RecordCount / PageSize);
                    PageCount += (PageCount < ((float)RecordCount / (float)PageSize)) ? 1 : 0;
                    Data = query.Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList<T>();
                }
                else
                {
                    PageCount = 1;
                    Data = query.ToList<T>();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pagingFilter"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private FilterDefinition<T> CreateFilters<T>(List<PagingFilter> pagingFilter, FilterDefinition<T> filter = null)
        {
            var filterB = Builders<T>.Filter;

            foreach (var item in pagingFilter)
            {
                var isObjectId = getIsObjectId<T>(item);
                Type typeProperty = null;
                try
                {
                    typeProperty = item.GetValueType();
                }
                catch { }

                switch (item.Condition.ToString())
                {
                    case "!=":
                        {
                            if (filter == null)
                                filter = filterB.Not(filterB.Eq(item.Property, isObjectId ? new ObjectId(item.GetValue().ToString()) : typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue()));
                            else
                                filter = filter & filterB.Not(filterB.Eq(item.Property, isObjectId ? new ObjectId(item.GetValue().ToString()) : typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue()));
                            break;
                        }
                    case "=":
                        {
                            if (filter == null)
                                filter = filterB.Eq(item.Property, isObjectId ? new ObjectId(item.GetValue().ToString()) : typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue());
                            else
                                filter = filter & filterB.Eq(item.Property, isObjectId ? new ObjectId(item.GetValue().ToString()) : typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue());
                            break;
                        }
                    case FILTER_IN:
                        {
                            if (filter == null)
                                filter = filterB.AnyIn(item.Property, (IEnumerable<object>)item.GetValue());
                            else
                                filter = filter & filterB.AnyIn(item.Property, (IEnumerable<object>)item.GetValue());
                            break;
                        }
                    case FILTER_NOT_IN:
                        {
                            if (filter == null)
                                filter = filterB.Not(filterB.AnyIn(item.Property, (IEnumerable<object>)item.GetValue()));
                            else
                                filter = filter & filterB.Not(filterB.AnyIn(item.Property, (IEnumerable<object>)item.GetValue()));
                            break;
                        }
                    case FILTER_CON:
                        {
                            if (filter == null)
                                filter = filterB.Regex(item.Property, new BsonRegularExpression(item.GetValue().ToString(), "i"));
                            else
                                filter = filter & filterB.Regex(item.Property, new BsonRegularExpression(item.GetValue().ToString(), "i"));
                            break;
                        }
                    case FILTER_NOT_CON:
                        {
                            if (filter == null)
                                filter = filterB.Not(filterB.Regex(item.Property, new BsonRegularExpression(item.GetValue().ToString(), "i")));
                            else
                                filter = filter & filterB.Not(filterB.Regex(item.Property, new BsonRegularExpression(item.GetValue().ToString(), "i")));
                            break;
                        }
                    case "<": //<
                        {
                            if (filter == null)
                                filter = filterB.Lt(item.Property, typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue());
                            else
                                filter = filter & filterB.Lt(item.Property, typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue());
                            break;
                        }
                    case "<="://<=
                        {
                            if (filter == null)
                                filter = filterB.Lte(item.Property, typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue());
                            else
                                filter = filter & filterB.Lte(item.Property, typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue());
                            break;
                        }
                    case ">"://>
                        {
                            if (filter == null)
                                filter = filterB.Gt(item.Property, typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue());
                            else
                                filter = filter & filterB.Gt(item.Property, typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue());
                            break;
                        }
                    case ">="://>=
                        {
                            if (filter == null)
                                filter = filterB.Gte(item.Property, typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue());
                            else
                                filter = filter & filterB.Gte(item.Property, typeProperty != null ? Convert.ChangeType(item.GetValue(), typeProperty) : item.GetValue());
                            break;
                        }
                    case FILTER_MATCH:
                        {
                            var value = item.GetValue();
                            if (value is PagingFilterData)
                            {
                                var subFilter = CreateFilters<T>(((PagingFilterData)value).Filter);
                                var f = filterB.ElemMatch(item.Property, subFilter);
                                if (filter == null)
                                    filter = f;
                                else
                                    filter = filter & f;
                                break;
                            }

                            break;
                        }
                    case FILTER_NOT_MATCH:
                        {
                            var value = item.GetValue();
                            if (value is PagingFilterData)
                            {
                                var subFilter = CreateFilters<T>(((PagingFilterData)value).Filter);
                                var f = filterB.Not(filterB.ElemMatch(item.Property, subFilter));
                                if (filter == null)
                                    filter = f;
                                else
                                    filter = filter & f;
                                break;
                            }

                            break;
                        }
                }
            }
            return filter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool getIsObjectId<E>(PagingFilter item)
        {
            var result = false;
            try
            {
                var type = typeof(E);
                var properties = type.GetProperties();
                foreach (var prop in properties)
                {
                    if (prop.Name == item.Property)
                    {
                        var atributes = prop.CustomAttributes.Where(x => x.AttributeType.Name == "BsonIdAttribute").FirstOrDefault();
                        if (atributes != null)
                        {
                            result = true;
                        }
                        break;
                    }
                }
            }
            catch
            {

            }
            return result;
        }


        public class AggregationPipelineResponse
        {
            public virtual int ok { get; set; }
            public virtual long waitedMS { get; set; }
            public virtual IEnumerable result { get; set; }
        }
    }
}
