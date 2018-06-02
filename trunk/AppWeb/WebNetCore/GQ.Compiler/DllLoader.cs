using GQ.Core.utils;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace GQ.Compiler
{
    public static class DllLoader
    {
        public static DllAssembly LoadDll(string path)
        {
            return new DllAssembly(AssemblyLoadContext.Default.LoadFromAssemblyPath(path));
        }

        public static DllAssembly LoadDll(Stream stream)
        {
            return new DllAssembly(AssemblyLoadContext.Default.LoadFromStream(stream));
        }

        public class DllAssembly
        {
            public Assembly assembly { get; private set; }

            public DllAssembly(Assembly assembly)
            {
                this.assembly = assembly;
            }

            public Type[] GetTypeByInterface(string interfaceName)
            {
                return ClassUtils.getTypesByInterface(assembly, interfaceName);
            }

            public Type GetClass(string className)
            {
                if (assembly == null)
                {
                    return null;
                }
                return assembly.GetType(className);
            }

            public Type GetClassByInterface(string interfaceName)
            {
                if (assembly == null)
                {
                    return null;
                }
                return assembly.GetTypes().Where(x => x.GetInterface(interfaceName) != null).FirstOrDefault();
            }

            public MethodInfo GetMethod(string className, string methodName)
            {
                var type = GetClass(className);
                return type.GetMethod(methodName);
            }

            public object Invoke(string className, string methodName, params object[] parameters)
            {
                var type = GetClass(className);
                var method = type.GetMethod(methodName);

                object obj = Activator.CreateInstance(type);

                var result = method.Invoke(obj, parameters);

                return result;
            }
        }
    }
}
