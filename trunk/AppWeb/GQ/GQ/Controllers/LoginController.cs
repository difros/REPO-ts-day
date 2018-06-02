using GQDataService.com.gq.constantes;
using GQDataService.com.gq.dto;
using GQDataService.com.gq.service;
using GQService.com.gq.controller;
using GQService.com.gq.data;
using GQService.com.gq.encriptation;
using GQService.com.gq.jwt;
using GQService.com.gq.security;
using GQService.com.gq.service;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;

namespace GQ.Controllers
{
    [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
    public class LoginController : BaseController
    {

        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public IActionResult Index()
        {
            Response.Cookies.Delete("jwt");
            return View();
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public ReturnData Login([FromBody]Gq_usuariosDto data)
        {
            var result = new ReturnData();

            var user = Services.Get<ServGq_usuarios>().findBy(x => (x.Usuario == data.Usuario || x.Email == data.Usuario) && (x.Clave == Encriptacion.Encriptar(data.Clave, Constantes.CLAVE_ENCRIPTACION) || x.Clave == data.Clave)).FirstOrDefault(); // Services.GetCollection<Iot_usuarios>().Find(x => (x.NombreUsuario == data.NombreUsuario || x.EMail == data.NombreUsuario) && x.Clave == Constantes.Encriptar(data.Clave)).FirstOrDefault();
            if (user != null)
            {
                Response.Cookies.Append("jwt", JWTUtil.GenerateToken(user, Security.SecuritySecretKey));
            }

            result.data = new Gq_usuariosDto().SetEntity(user);
            result.isError = result.data == null;

            return result;
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public bool Logout()
        {
            Response.Cookies.Delete("jwt");
            return true;
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        [Route("[controller]/[action]/{value}")]
        public bool RecuperarClave(string value)
        {
            var controller = new UsuarioController();
            return controller.RecuperarClave(value);
        }
    }
}
