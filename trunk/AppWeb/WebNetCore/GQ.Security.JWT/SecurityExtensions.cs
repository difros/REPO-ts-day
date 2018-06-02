namespace GQ.Security.JWT
{
    /// <summary>
    /// 
    /// </summary>
    public static class SecurityJWT
    {
        /// <summary>
        /// 
        /// </summary>
        public static T UsuarioLogueado<T>() where T : class, new()
        {
            T result = null;
            if (System.Web.HttpContext.Current.Request.Cookies.ContainsKey("jwt"))
            {
                result = JWTUtil.GetPayload<T>(System.Web.HttpContext.Current.Request.Cookies["jwt"], Security.GetSecurityConfigure.SecuritySecretKey);
            }
            return result;
        }
    }
}
