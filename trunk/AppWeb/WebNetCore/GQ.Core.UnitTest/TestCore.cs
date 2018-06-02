using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GQ.Core.UnitTest
{
    [TestClass]
    public class TestCore
    {
        [TestMethod]
        public void Encriptacion()
        {
            var key = "IOTG3m1nusQh0m";
            var text = "N9YaG0JSE0";
            var resultado = encriptacion.Encriptacion.Encriptar(text, key);
            resultado = encriptacion.Encriptacion.Desencriptar(resultado, key);

            if (!text.Equals(resultado))
                throw new System.Exception("Fail");
        }
    }
}
