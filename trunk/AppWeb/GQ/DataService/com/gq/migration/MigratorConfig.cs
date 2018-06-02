using System.Reflection;
namespace GQDataService.com.gq.migrations
{
    public static class MigratorConfig
    {
        public static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
