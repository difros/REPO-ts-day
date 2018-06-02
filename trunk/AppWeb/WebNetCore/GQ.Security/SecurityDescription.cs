using System;
using System.Reflection;

namespace GQ.Security
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SecurityDescription : Attribute
    {
        public delegate bool DelegateHasPermission(object value, string method = null, bool returnException = true);

        /// <summary>
        /// 
        /// </summary>
        public enum SeguridadEstado
        {
            /// <summary>
            /// 
            /// </summary>
            Desactivo,
            /// <summary>
            /// 
            /// </summary>
            Activo,
            /// <summary>
            /// 
            /// </summary>
            ActivoFunction,
            /// <summary>
            /// 
            /// </summary>
            SoloLogueo
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public MethodInfo Method { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SeguridadEstado Estado { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string[] Perfiles { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public SecurityDescription(string name)
        {
            Name = name;
            Estado = SecurityDescription.SeguridadEstado.Activo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="perfiles"></param>
        public SecurityDescription(string name, string[] perfiles)
        {
            Name = name;
            Perfiles = perfiles;
            Estado = SecurityDescription.SeguridadEstado.Activo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="estado"></param>
        public SecurityDescription(SecurityDescription.SeguridadEstado estado)
        {
            Estado = estado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="delegateType"></param>
        /// <param name="delegateName"></param>
        public SecurityDescription(string name, Type delegateType , string delegateName)
        {
            Name = name;
            Method = delegateType.GetMethod(delegateName);
            Estado = SecurityDescription.SeguridadEstado.ActivoFunction;
        }
    }
}
