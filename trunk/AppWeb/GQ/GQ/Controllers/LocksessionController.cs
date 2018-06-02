using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GQService.com.gq.controller;
using GQService.com.gq.security;
using GQService.com.gq.data;
using GQDataService.com.gq.dto;
using GQDataService.com.gq.service;
using GQService.com.gq.jwt;
using GQService.com.gq.service;
using GQDataService.com.gq.constantes;
using GQService.com.gq.encriptation;

namespace GQ.Controllers
{
    [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
    public class LocksessionController : BaseController
    {

        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public IActionResult Index()
        {
            var jwt = JWTUtil.GetPayloadSinControl<Gq_usuariosDto>(Request.Cookies["jwt"], Security.SecuritySecretKey);

            if (jwt != null && jwt.Usuario != null)
                ViewData["UsuarioNombre"] = jwt.Usuario;

            return PartialView();
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public ReturnData Login([FromBody]Gq_usuariosDto data)
        {
            var result = new ReturnData();
            var Usuario = JWTUtil.GetPayloadSinControl<Gq_usuariosDto>(Request.Cookies["jwt"], Security.SecuritySecretKey).Usuario;


            var user = Services.Get<ServGq_usuarios>().findBy(x => (x.Usuario == Usuario || x.Email == Usuario) && (x.Clave == Encriptacion.Encriptar(data.Clave, Constantes.CLAVE_ENCRIPTACION) || x.Clave == data.Clave)).FirstOrDefault(); 
            if (user != null)
            {
                Response.Cookies.Delete("jwt");
                Response.Cookies.Append("jwt", JWTUtil.GenerateToken(user, Security.SecuritySecretKey));
            }

            result.data = new Gq_usuariosDto().SetEntity(user);
            result.isError = result.data == null;

            return result;
        }
    }
}