using System;
using System.Collections.Generic;
using GQDataService.com.gq.domain;
using GQDataService.com.gq.service;
using GQService.com.gq.mail;
using GQService.com.gq.service;
using GQ.com.gq.Compiler;
using static GQ.com.gq.Compiler.ProcesarMailTemplate;

namespace GQ.Helper
{
    public static class MailHelper 
    {
        #region Configuración SMTP
        private static Gq_smtp_config getConfig()
        {
            return Services.Get<ServGq_smtp_config>().findByOne(x => x.Nombre.Contains("gmail"));
        }
        #endregion

        public static bool Enviar_AvisoDeUsuarioCreado(Gq_usuarios pUsuario, string pClave)
        {
            try
            {
                EjecutarDto ejecutar = new EjecutarDto();
                ejecutar.Metodo = "Enviar_Mail";
                ejecutar.Parametros = new object[] { pUsuario, pClave };
                ejecutar.Id = "Creacion_usuario";
                return (bool)ProcesarMailTemplate.Ejecutar(ejecutar);
                
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Enviar_AvisoDeMOdClave(Gq_usuarios pUsuario, string pClave)
        {
            try
            {
                EjecutarDto ejecutar = new EjecutarDto();
                ejecutar.Metodo = "Enviar_Mail";
                ejecutar.Parametros = new object[] { pUsuario, pClave };
                ejecutar.Id = "Clave_modificada";
                return (bool)ProcesarMailTemplate.Ejecutar(ejecutar);                
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Enviar_AvisoDeMOdClaveOK(Gq_usuarios pUsuario)
        {
            try
            {
                EjecutarDto ejecutar = new EjecutarDto();
                ejecutar.Metodo = "Enviar_Mail";
                ejecutar.Parametros = new object[] { pUsuario };
                ejecutar.Id = "Clave_modificadaOK";
                return (bool)ProcesarMailTemplate.Ejecutar(ejecutar);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Enviar_AvisoDeRecClave(Gq_usuarios pUsuario, string pClave)
        {
            try
            {
                EjecutarDto ejecutar = new EjecutarDto();
                ejecutar.Metodo = "Enviar_Mail";
                ejecutar.Parametros = new object[] { pUsuario , pClave };
                ejecutar.Id = "Clave_recuperada";
                return (bool)ProcesarMailTemplate.Ejecutar(ejecutar);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
