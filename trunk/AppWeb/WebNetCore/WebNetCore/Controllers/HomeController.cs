using GQ.Compiler;
using GQ.Core.service;
using GQ.Data;
using GQ.Security;
using GQ.Security.MCV.controller;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace WebNetCore.Controllers
{
    [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
    public class HomeController : BaseController
    {
        public async Task<ReturnData<int>> Index()
        {
            /*
            var compilation = CSharpCompilation.Create("a")
    .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
    .AddReferences(
        MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location))
    .AddSyntaxTrees(CSharpSyntaxTree.ParseText(
        @"
using System;

public static class C
{
    public static void M()
    {
        Console.WriteLine(""Hello Roslyn."");
    }
}"));

            var fileName = "a.dll";

            compilation.Emit(fileName);

            var a = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(fileName));

            a.GetType("C").GetMethod("M").Invoke(null, null);*/

            CompilerCSharpRoslyn cs = new CompilerCSharpRoslyn();
            //cs.AddReferencia("System.dll");
            //cs.AddReferencia("System.Data.dll");
            //cs.AddReferencia("System.Core.dll");
            //cs.AddReferencia("System.Runtime.dll");
            //cs.AddReferencia("System.Runtime.Serialization.dll");

            var files = System.IO.Directory.GetFiles(ServicesContainer.ContentRootPath(), "*.dll");

            var excludeDlls = new string[] { };

            foreach (var item in files)
            {
                var fileName1 = item.Substring(item.LastIndexOf('\\') + 1).ToLower();
                if (excludeDlls.Where(x => x.Equals(fileName1)).Count() == 0)
                    cs.AddReferencia(item);
            }

            cs.SourceType = CompilerCSharpRoslyn.SourceTypeEnum.Text;
            cs.Source = @"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebNetCore.Controllers
{
    public class test
    {
        public string Say(string name)
        {
            return ""Hello "" + name;
        }
    }
}
";

            var classType = cs.Invoke("WebNetCore.Controllers.test", "Say", new object[] { "Esteban" });

            //var a = DllLoader.LoadDll(@"D:\PROYECTOS\GQBase\code\web\net\src\trunk\AppWeb\WebNetCore\WebNetCore\wwwroot\TestDllLoader.dll");

            //var classType = a.Invoke("WebNetCore.Controllers.test", "Say",new object[] { "Esteban" });

            /*
            PlataformaIOT rest = new PlataformaIOT();
            var result = await rest.Login("58989260e6fb8410d0293bbd");
            result = await rest.GetDevices();
            result = await rest.GetSchedule(2, 500);

            var s = new MySQLService();
            var m = s.GetSession<Gq_usuarios>().findById(2);
            */

            //Paging page = new Paging();
            //page.PageSize = 10;
            //page.PageIndex = 1;
            //page.CreateProjection = () =>
            //{
            //    var project = Builders<BsonDocument>.Projection;
            //    ProjectionDefinition<BsonDocument> p = null;

            //    p = project.Exclude("EstadoActual");
            //    p = p.Exclude("Comandos");

            //    return p;
            //};
            //page.Filter.Add(new PagingFilter { Property = "Usuarios", Condition = "match", Value = new PagingFilterData { Filter = { new PagingFilter { Property = "UsuarioId", Condition = "=", Value = "58989260e6fb8410d0293bbd" } } } });
            //page.Populate<BsonDocument>("IOT_Equipos");


            return new ReturnData<int>(); // View();
        }
    }
}
