using GQ.Core.service;
using System;

namespace GQ.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityConfigure
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Configure(
            TimeSpan timeOut,
            Func<object> delegateUsuarioLogueado = null,
            Security.DelegateHasPermission delegateHasPermission = null,
            Type[] excludeControllerSecurity = null,
            string securitySecretKey = "GeminusQhom-Aplications!#$%&/()=")
        {
            SecurityConfigure sc = new SecurityConfigure();
            sc.TimeOut = timeOut;
            sc.DelegateHasPermission = delegateHasPermission;
            sc.ExcludeControllerSecurity = excludeControllerSecurity;
            sc.SecuritySecretKey = securitySecretKey;
            sc.DelegateUsuarioLogueado = delegateUsuarioLogueado;

            ServicesContainer.AddSingleton<SecurityConfigure>(sc);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual TimeSpan TimeOut { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Security.DelegateHasPermission DelegateHasPermission { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Delegate DelegateUsuarioLogueado { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Type[] ExcludeControllerSecurity { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string SecuritySecretKey { get; private set; }

       
    }
}
