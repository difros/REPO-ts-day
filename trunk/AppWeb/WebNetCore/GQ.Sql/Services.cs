using System;

namespace GQ.Sql
{
    public class Services
    {
        public static T Get<T>() where T : BaseService
        {
            T oService = null;
            
            oService = (T)Activator.CreateInstance(typeof(T));

            return oService;
        }
    }
}
