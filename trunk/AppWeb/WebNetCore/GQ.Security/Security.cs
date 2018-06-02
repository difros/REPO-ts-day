using GQ.Core.service;
using GQ.Core.utils;
using System;

namespace GQ.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class Security
    {
        private static SecurityConfigure sc = null;

        /// <summary>
        /// 
        /// </summary>
        public static SecurityConfigure GetSecurityConfigure
        {
            get
            {
                if (sc == null)
                    sc = ServicesContainer.GetService<SecurityConfigure>();
                return sc;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static T UsuarioLogueado<T>() where T : class, new()
        {
            return (T) GetSecurityConfigure.DelegateUsuarioLogueado.DynamicInvoke();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="method"></param>
        /// <param name="returnException"></param>
        /// <returns></returns>
        public delegate bool DelegateHasPermission(object value, string method = null, bool returnException = true, params object[] parameters);

        private static DelegateHasPermission delegateHasPermission = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="method"></param>
        /// <param name="returnException"></param>
        /// <returns></returns>
        public static bool hasPermission(object value, string method = null, bool returnException = true, params object[] parameters)
        {
            if (delegateHasPermission == null)
                delegateHasPermission = GetSecurityConfigure.DelegateHasPermission;
            if (delegateHasPermission != null)
                return delegateHasPermission(value, method, returnException, parameters);
            return true;
        }

        private static Type[] excludeControllerSecurity = null;
        /// <summary>
        /// 
        /// </summary>
        public static Type[] ExcludeControllerSecurity
        {
            get
            {
                if (excludeControllerSecurity == null)
                {
                    excludeControllerSecurity = GetSecurityConfigure.ExcludeControllerSecurity;
                }
                return excludeControllerSecurity;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsExcludeController(Type type)
        {
            return IsExcludeController(ClassUtils.getNameObject(type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsExcludeController(string name)
        {
            foreach (Type item in ExcludeControllerSecurity)
            {
                if (name.Equals(ClassUtils.getNameObject(item)))
                    return true;
            }
            return false;
        }
    }
}