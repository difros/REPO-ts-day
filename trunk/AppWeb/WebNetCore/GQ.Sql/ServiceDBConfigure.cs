using System.Reflection;

namespace GQ.Sql
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceDBConfigure
    {

        public static IBaseService Service { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static void Configure(Assembly assembly, IServiceDBConfigure config)
        {


        }

        
    }
}