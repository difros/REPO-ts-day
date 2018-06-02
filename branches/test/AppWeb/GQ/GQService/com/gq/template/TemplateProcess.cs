using System.IO;
using System.Text.RegularExpressions;

namespace GQService.com.gq.template
{
    public static class TemplateProcess
    {
        public static string ExecuteFile(string fileName, params dynamic[] parametros)
        {
            string source = "";
            source = File.ReadAllText(fileName);
            return Execute(source, parametros);
        }

        public static string Execute(string source, params object[] parametros)
        {
            string result = source.ToString();

            foreach (var item in parametros)
            {
                var type = item.GetType();
                var properties = type.GetProperties();
                foreach (var prop in properties)
                {
                    if (prop.CanRead)
                    {
                        var replace = "{" + type.Name + "." + prop.Name + "}";
                        result = Regex.Replace(result, replace, prop.GetValue(item).ToString(), RegexOptions.IgnoreCase);// result.Replace(replace, prop.GetValue(item).ToString());
                    }
                }
            }
            return result;
        }
    }
}
