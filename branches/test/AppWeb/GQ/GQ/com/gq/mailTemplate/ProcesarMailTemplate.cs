using GQDataService.com.gq.service;
using GQService.com.gq.compiler;
using GQService.com.gq.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GQ.com.gq.Compiler
{
    public class ProcesarMailTemplate
    {
        public static object Ejecutar(EjecutarDto model)
        {
            object result = null;
            var g = Services.Get<ServGq_mailTemplate>().findBy(x => x.Folder == model.Id.ToString()).FirstOrDefault();
            if (g != null)
            {
                CompilerCSharp cs = new CompilerCSharp();

                cs.AddReferencia("System.dll");
                cs.AddReferencia("System.Data.dll");
                cs.AddReferencia("System.Core.dll");
                cs.AddReferencia("System.Runtime.dll");
                cs.AddReferencia("System.Runtime.Serialization.dll");

                var files = System.IO.Directory.GetFiles(cs.PathBase, "*.dll");

                foreach (var item in files)
                {
                    if (!item.Contains("libuv.dll"))
                        cs.AddReferencia(item);
                }

                if (!string.IsNullOrWhiteSpace(g.Folder))
                {
                    var dir = System.IO.Directory.GetCurrentDirectory();
                    cs.SourceType = CompilerCSharp.SourceTypeEnum.File;
                    cs.Source = dir + "\\wwwroot\\mailTemplate\\" + g.Folder + "\\mailTemplate.cs";
                }
                else
                {
                    cs.SourceType = CompilerCSharp.SourceTypeEnum.Text;
                    cs.Source = g.CodeSharp;
                }

                result = cs.Invoke("Main", model.Metodo, model.Parametros);

            }
            return result;
        }

        public class EjecutarDto
        {
            public object Id { get; set; }
            public string Metodo { get; set; }
            public object[] Parametros { get; set; }
        }
    }
}
