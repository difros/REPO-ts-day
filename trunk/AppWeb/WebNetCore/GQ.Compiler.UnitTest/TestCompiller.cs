using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GQ.Compiler.UnitTest
{
    [TestClass]
    public class TestCompiller
    {
        [TestMethod]
        public void CompilerRoslyn()
        {

            CompilerCSharpRoslyn cs = new CompilerCSharpRoslyn();


            var files = System.IO.Directory.GetFiles(@"D:\PROYECTOS\GQBase\code\web\net\src\trunk\AppWeb\WebNetCore\GQ.Compiler.UnitTest\bin\Debug\netcoreapp2.0\", "*.dll");

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
        }
    }
}
