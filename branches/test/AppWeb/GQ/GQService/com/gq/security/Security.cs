using GQService.com.gq.jwt;
using GQService.com.gq.structureMap;
using System;

namespace GQService.com.gq.security
{
    /// <summary>
    /// 
    /// </summary>
    public static class Security
    {
        public static string SecuritySecretKey { get; set; } = "GeminusQhom-Aplications!#$%&/()=";

        private static SecurityConfigure sc = null;
        public static SecurityConfigure GetSecurityConfigure
        {
            get
            {
                if (sc == null)
                    sc = ObjectFactory.GetNamedInstance<SecurityConfigure>("SecurityConfigure");
                return sc;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static T usuarioLogueado<T>() where T : class, new()
        {
            T result = null;
            if (System.Web.HttpContext.Current.Request.Cookies.ContainsKey("jwt"))
            {
                result = JWTUtil.GetPayload<T>(System.Web.HttpContext.Current.Request.Cookies["jwt"], SecuritySecretKey);
            }

            return result;
        }

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
                delegateHasPermission = ObjectFactory.GetNamedInstance<DelegateHasPermission>("delegateHasPermission");
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
                    excludeControllerSecurity = ObjectFactory.GetNamedInstance<Type[]>("excludeControllerSecurity");
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
            return IsExcludeController(Security.getNameObject(type));
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
                if (name.Equals(Security.getNameObject(item)))
                    return true;
            }
            return false;
        }
    }
}