using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GQ.Data
{
    public interface IPaging
    {
        int? PageIndex { get; set; }
        int? PageSize { get; set; }
        long? PageCount { get; set; }
        long? RecordCount { get; set; }
        List<PagingFilter> Filter { get; set; }
        List<PagingOrder> Order { get; set; }
        IEnumerable Data { get; set; }
    }

    public class PagingFilter
    {
        public string Property { get; set; }
        public string Condition { get; set; }
        private object _Value;
        public object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                if (_Value != null && string.IsNullOrEmpty(ValueType))
                {
                    ValueType = _Value.GetType().ToString();
                }
            }
        }
        public string ValueType { get; set; } = string.Empty;

        public object GetValue()
        {
            var valueType = GetValueType();
            if (valueType != null)
            {
                return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(Value), valueType);
            }
            else if (Value is JObject)
            {
                return JsonConvert.DeserializeObject<PagingFilterData>(JsonConvert.SerializeObject(Value));
            }
            return Value;
        }

        public Type GetValueType()
        {
            try
            {
                return Type.GetType(ValueType);
            }
            catch
            {
                return null;
            }

        }
    }

    public class PagingFilterData
    {
        public List<PagingFilter> Filter { get; set; } = new List<PagingFilter>();
    }

    public class PagingOrder
    {
        public string Property { get; set; }
        public string Direction { get; set; }
    }
}
