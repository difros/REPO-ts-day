using GQ.Core.encriptacion;

namespace WebNetCore.Data.constantes
{
    public static class Constantes
    {
        public const string CLAVE_ENCRIPTACION = "IOTG3m1nusQh0m";

        public const string ESTADO_ACTIVO = "A";
        public const string ESTADO_DESACTIVO = "D";
        public const string ESTADO_BORRADO = "B";

        public const string LANG_ES = "es";
        public const string LANG_EN = "en";

        public static string Encriptar(string value)
        {
            return Encriptacion.Encriptar(value, CLAVE_ENCRIPTACION);
        }

        public static string Desencriptar(string value)
        {
            return Encriptacion.Desencriptar(value, CLAVE_ENCRIPTACION);
        }

        
    }
}
