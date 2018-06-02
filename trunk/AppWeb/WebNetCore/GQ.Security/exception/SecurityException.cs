using GQ.Core.utils;
using System;

namespace GQ.Security.exception
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityException : Exception
    {
        private string message = "No tiene permiso para acceder a este metodo";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Method"></param>
        public SecurityException(object value, string Method)
        {
            ObjectName = ClassUtils.getNameObject(value);
            MethodName = Method;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Message { get { return message; } }

        /// <summary>
        /// 
        /// </summary>
        public virtual string ObjectName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string MethodName { get; private set; }
    }
}
