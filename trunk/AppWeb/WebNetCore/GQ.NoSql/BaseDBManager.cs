using System;
using System.Linq;
using System.Reflection;

namespace GQ.NoSql
{
    public class BaseDBManager
    {
        protected string GetCollectionName(Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
                 BindingFlags.Static | BindingFlags.FlattenHierarchy);

            var result = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.Name.Equals("CollectionName")).ToList();
            var instance = Activator.CreateInstance(type);

            return result[0].GetValue(instance).ToString();
        }
    }

}
