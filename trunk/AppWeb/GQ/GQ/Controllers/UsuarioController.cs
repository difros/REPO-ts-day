using GQService.com.gq.controller;
using GQService.com.gq.data;
using GQService.com.gq.encriptation;
using GQService.com.gq.menu;
using GQService.com.gq.paging;
using GQService.com.gq.security;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using GQService.com.gq.service;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GQService.com.gq.validate;
using GQService.com.gq.exception;
using GQService.com.gq.utils;
using GQ.Helper;
using GQDataService.com.gq.constantes;
using GQDataService.com.gq.domain;
using GQDataService.com.gq.dto;
using GQDataService.com.gq.service;

namespace GQ.Controllers
{

    [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
    public class UsuarioController : BaseController, IABM<Gq_usuariosDto>
    {

        // GET: /<controller>/
        [MenuDescription("90-10-00", "Usuarios", GQ.com.gq.security.Security.MENU_CONFIG_ID)]
        [SecurityDescription("Usuarios", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public IActionResult Index()
        {
            return PartialView();
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        [Route("[controller]/[action]/{done}")]
        public IActionResult MisDatos(string done)
        {
            ViewData["done"] = done;
            return View("Index");
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public Paging Buscar([FromBody]Paging paging)
        {
            var query = Services.Get<ServGq_usuarios>().findBy(x => x.Estado != Constantes.ESTADO_BORRADO);
            paging.Apply<Gq_usuarios, Gq_usuariosDto>(query);
            return paging;
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public IEnumerable<Gq_usuariosDto> GetUsuarios()
        {
            var query = Services.Get<ServGq_usuarios>().findBy(x => x.Estado != Constantes.ESTADO_BORRADO);
            return new Gq_usuariosDto().SetEntity(query);

        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public IEnumerable<Gq_perfilesDto> GetPerfiles()
        {
            var perfiles = Services.Get<ServGq_perfiles>().findBy(x => x.Estado != Constantes.ESTADO_BORRADO);
            Gq_perfilesDto perfilLog = com.gq.security.Security.getPerfilUserLogueado();
            return new Gq_perfilesDto().SetEntity(perfiles.ToList());
        }

        [SecurityDescription("Usuarios - Guardar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Guardar([FromBody]Gq_usuariosDto model)
        {
            ReturnData result = new ReturnData();
            string claveSinEncrip = "";
            try
            {
                var resultsValidation = new List<ValidationResult>();
                if (ValidateUtils.TryValidateModel(model, resultsValidation))
                {
                    using (var transaction = Services.session.BeginTransaction())
                    {
                        try
                        {
                            if (model.Clave.Equals(model.ClaveChequed))
                            {
                                claveSinEncrip = model.Clave;
                                model.Clave = Encriptacion.Encriptar(model.Clave, Constantes.CLAVE_ENCRIPTACION);
                            }

                            var entity = model.GetEntity();
                            //temporal
                            if (entity.PerfilId == 0) entity.PerfilId = 1;

                            if (entity.UsuarioId == null)
                            {
                                /*if (EsAdminOConfig() == false)
                                {
                                    result.data = "Usted no tiene permiso para agregar usuarios.";
                                    result.isError = true;
                                }
                                else*/
                                if (IsUniqueUser(entity.Usuario) == false)
                                {
                                    result.data = "El usuario <strong>" + entity.Usuario + "</strong> ya existe";
                                    result.isError = true;
                                }
                                else if (IsUniqueMail(entity.Email) == false)
                                {
                                    result.data = "El mail <strong>" + entity.Email + "</strong> ya existe";
                                    result.isError = true;
                                }
                                else
                                {
                                    entity.Creado = DateTime.Now;
                                    entity.CreadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                                    entity.Modificado = DateTime.Now;
                                    entity.ModificadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                                    Services.Get<ServGq_usuarios>().Agregar(entity);
                                }
                            }
                            else
                            {
                                entity.Modificado = DateTime.Now;
                                entity.ModificadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                                Services.Get<ServGq_usuarios>().Actualizar(entity);
                            }
                        }
                        catch (Exception e)
                        {
                            result.isError = true;
                            result.data = GenericError.Create(e);
                        }

                        if (!result.isError)
                            transaction.Commit();
                        else
                            transaction.Rollback();

                    }
                }
                else
                {
                    result.data = resultsValidation;
                    result.isError = true;
                }
            }
            catch (Exception e)
            {
                result.isError = true;
                result.data = GenericError.Create(e);
            }
            return result;
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public Boolean IsUniqueUser([FromBody]string user)
        {
            try
            {
                return !Services.Get<ServGq_usuarios>().findBy(x => x.Usuario == user && x.Estado != Constantes.ESTADO_BORRADO).Any();
            }
            catch
            {
                return false;
            }
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public Boolean IsUniqueMail([FromBody]string mail)
        {
            try
            {
                return !Services.Get<ServGq_usuarios>().findBy(x => x.Email == mail && x.Estado != Constantes.ESTADO_BORRADO).Any();
            }
            catch
            {
                return false;
            }
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public Boolean RecuperarClave([FromBody]string value)
        {
            try
            {
                var usr = Services.Get<ServGq_usuarios>().findBy(x => x.Email.ToLower() == value.ToLower()).FirstOrDefault();
                if (usr != null)
                {
                    return RecoverClave(usr);
                }
                else
                {
                    usr = Services.Get<ServGq_usuarios>().findBy(x => x.Usuario.ToLower() == value.ToLower()).FirstOrDefault();
                    if (usr != null)
                    {
                        return RecoverClave(usr);
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public ReturnData ClaveRecuperada([FromBody]Gq_usuariosDto model)
        {
            ReturnData result = new ReturnData();
            try
            {
                if (model != null)
                {
                    if (model.Clave.Equals(model.ClaveChequed))
                    {
                        model.Clave = Encriptacion.Encriptar(model.Clave, Constantes.CLAVE_ENCRIPTACION);
                        var entity = model.GetEntity();

                        List<string> emails = new List<string> { entity.Email };

                        entity.Clave = model.Clave;
                        entity.RequiereClave = "N";

                        using (var transaction = Services.session.BeginTransaction())
                        {
                            try
                            {
                                Services.Get<ServGq_usuarios>().Actualizar(entity);
                            }
                            catch
                            {

                            }
                            transaction.Commit();
                        }

                        result.data = new Gq_usuariosDto().makeDto(entity);

                        /*if (MailHelper.Enviar_AvisoDeMOdClaveOK(entity))
                        {
                            return result;
                        }
                        else
                        {
                            //ToDo: avisar que no se envio el mail
                        }*/
                    }
                    else
                    {
                        result.data = null;
                        result.isError = true;
                    }
                }
                else
                {
                    result.data = null;
                    result.isError = true;
                }
            }
            catch (Exception e)
            {
                result.isError = true;
                result.data = GenericError.Create(e);
            }
            return result;
        }

        private bool RecoverClave(Gq_usuarios entity)
        {
            try
            {
                string newPass = UtilsFunctions.CreateRandomCode(6);
                if (entity != null)
                {
                    
                    List<string> emails = new List<string> { entity.Email };

                    entity.RequiereClave = "S";
                    entity.Clave = Encriptacion.Encriptar(newPass, Constantes.CLAVE_ENCRIPTACION);
                    using (var transaction = Services.session.BeginTransaction())
                    {
                        try
                        {
                            Services.Get<ServGq_usuarios>().Actualizar(entity);
                        }
                        catch
                        {

                        }
                        transaction.Commit();
                    }
                }

                return MailHelper.Enviar_AvisoDeRecClave(entity, newPass);
            }
            catch (Exception)
            {
                return false;
            }
        }

        [SecurityDescription("Usuarios - Borrar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Borrar([FromBody]Gq_usuariosDto model)
        {
            ReturnData result = new ReturnData();
            using (var transaction = Services.session.BeginTransaction())
            {
                try
                {
                    var entity = Services.Get<ServGq_usuarios>().findById(model.UsuarioId);
                    var update = entity.Estado = Constantes.ESTADO_BORRADO;
                    entity.Modificado = DateTime.Now;
                    entity.ModificadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                    Services.Get<ServGq_usuarios>().Actualizar(entity);
                }
                catch (Exception e)
                {
                    result.isError = true;
                    result.data = GenericError.Create(e);
                }

                if (!result.isError)
                    transaction.Commit();
                else
                    transaction.Rollback();
            }

            return result;
        }
    }
}