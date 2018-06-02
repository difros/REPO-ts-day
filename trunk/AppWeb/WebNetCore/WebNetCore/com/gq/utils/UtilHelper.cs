using System.Reflection;

namespace WebNetCore.com.gq.utils
{
    public static class UtilHelper
    {
        public static string GetVersion()
        {
            return Assembly.GetAssembly(typeof(UtilHelper)).GetName().Version.ToString();
        }

        private static string pathBase = "";
        public static void SetBase(string value)
        {
            pathBase = value;
        }

        public static string GetPathBase(string value)
        {
            return pathBase + @"/";
        }
        public static string GetPathImagen(string value)
        {
            return GetPathBase(value) + @"images/";
        }
        public static string GetPathCss(string value)
        {
            return GetPathBase(value) + @"css/";
        }

        private static string physicPathBase = "";
        private static string physicPathBaseRoot = "";
        private static string physicPathBaseBin = "";

        public static void SetPhysicBase(string value)
        {
            if (value.ToLower().IndexOf("debug") > 0)
            {
                physicPathBaseRoot = @"../../../";
            }

            physicPathBase = value;

            string path = Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "").Replace(":/", ":\\").Replace("/", "\\");
            var idx = path.LastIndexOf('\\') + 1;

            physicPathBaseBin = path.Substring(0, idx);
        }

        public static string GetWWWRoot(string value)
        {
            return physicPathBase + "/" + physicPathBaseRoot + @"wwwroot/" + value + @"/";
        }

        public static string GetCompany(string value)
        {
            return GetWWWRoot("company") + value;
        }

        public static string GetBin()
        {
            return physicPathBaseBin;
        }

    }
}
