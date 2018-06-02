using GQ.Core.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GQ.Compiler
{
    public abstract class CompilerCSharp
    {
        public enum SourceTypeEnum
        {
            File,
            Text
        }

        public List<string> Reference { get; set; }

        public SourceTypeEnum SourceType { get; set; }

        public string Source { get; set; }

        public Assembly CompiledAssembly { get; set; }

        public CompilerCSharp()
        {
            Reference = new List<string>();
        }

        public void AddReferencia(string referencia)
        {
            Reference.Add(referencia);
        }

        public Type[] GetTypeByInterface(string interfaceName)
        {
            return ClassUtils.getTypesByInterface(CompiledAssembly, interfaceName);
        }

        public Type GetClass(string className)
        {
            if (CompiledAssembly == null)
            {
                GenerateCode(this);
            }
            return CompiledAssembly.GetType(className);
        }

        public Type GetClassByInterface(string interfaceName)
        {
            if (CompiledAssembly == null)
            {
                GenerateCode(this);
            }
            return CompiledAssembly.GetTypes().Where(x => x.GetInterface(interfaceName) != null).FirstOrDefault();
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

        public virtual void GenerateCode(CompilerCSharp code)
        {
            
        }
    }
}
