using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace GQ.Core.utils
{
    public static class ClassUtils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string getNameObject(object value)
        {
            if (value != null)
            {
                if (value is Type)
                    return (value as Type).Namespace + "." + (value as Type).Name;
                return value.GetType().Namespace + "." + value.GetType().Name;
            }
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethod(Type type, string MethodName)
        {
            var method = (from iface in type.GetMethods()
                          where iface.Name.Equals(MethodName)
                          select iface).ToArray();
            return method;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type[] getTypes(Type type)
        {
            List<Type> result = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    result.AddRange(getTypes(assembly, type));
                }
                catch { }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type[] getTypes(Assembly assembly, Type type)
        {
            List<Type> result = new List<Type>();

            try
            {
                var typesSelect = (
                    from types in assembly.GetTypes()
                    where types.BaseType == type || (types.BaseType.IsGenericType == true && types.BaseType.GetGenericTypeDefinition() == type)
                    select types);

                result.AddRange(typesSelect);
            }
            catch { }


            return result.ToArray();
        }

        public static Type[] getTypesByInterface(Assembly assembly, string InterfaceName)
        {
            List<Type> result = new List<Type>();

            try
            {
                var typesSelect = (
                    from types in assembly.GetTypes()
                    where types.GetInterface(InterfaceName) != null
                    select types);

                result.AddRange(typesSelect);
            }
            catch { }


            return result.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object getNewInstance(Type value)
        {
            return Activator.CreateInstance(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Type getElementType(Type value)
        {
            var elementType = (from iface in value.GetInterfaces()
                               where iface.IsGenericType
                               where iface.GetGenericTypeDefinition() == typeof(IList<>)
                               select iface.GetGenericArguments()[0]);
            if (elementType.Count() > 0)
            {
                return elementType.First();
            }

            elementType = (from iface in new Type[] { value }
                           where iface.IsGenericType
                           where iface.GetGenericTypeDefinition() == typeof(IList<>)
                           select iface.GetGenericArguments()[0]);

            if (elementType.Count() > 0)
            {
                return elementType.First();
            }

            return null;
        }

        /*
        public static IEnumerable<Assembly> GetReferencingAssemblies(string assemblyName)
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateLibrary(library, assemblyName))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }

        private static bool IsCandidateLibrary(RuntimeLibrary library, string assemblyName)
        {
            return library.Name == (assemblyName)
                || library.Dependencies.Any(d => d.Name.StartsWith(assemblyName));
        }
        */

        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
