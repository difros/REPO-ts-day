using GQService.com.gq.controller;
using GQService.com.gq.data;
using GQService.com.gq.exception;
using GQService.com.gq.menu;
using GQService.com.gq.paging;
using GQService.com.gq.security;
using GQService.com.gq.service;
using GQService.com.gq.validate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GQDataService.com.gq.constantes;
using GQDataService.com.gq.domain;
using GQDataService.com.gq.dto;
using GQDataService.com.gq.service;

namespace GQ.Controllers
{

    [SecurityDescription("MailTemplate", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
    public class MailTemplateController : BaseController, IABM<Gq_mailTemplateDto>
    {
        [MenuDescription("90-50-00", "MailTemplate", GQ.com.gq.security.Security.MENU_CONFIG_ID)]
        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public IActionResult Index()
        {
            return PartialView();
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public Paging Buscar([FromBody]Paging paging)
        {
            var query = Services.Get<ServGq_mailTemplate>().findBy(x => x.Estado != Constantes.ESTADO_BORRADO);
            paging.Apply<Gq_mailTemplate, Gq_mailTemplateDto>(query);
            return paging;
        }
        
        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        [Route("[controller]/[action]/{id}")]
        public Gq_mailTemplateDto GetMailTemplate(long id)
        {
            Gq_mailTemplateDto model = new Gq_mailTemplateDto().SetEntity(Services.Get<ServGq_mailTemplate>().findById(id));
            if (!string.IsNullOrWhiteSpace(model.Folder))
            {
                var dir = System.IO.Directory.GetCurrentDirectory();

                using (var csRead = System.IO.File.OpenText(dir + "\\wwwroot\\mailTemplate\\" + model.Folder + "\\mailTemplate.cs"))
                {
                    model.CodeSharp = csRead.ReadToEnd();
                }               

                using (var htmlRead = System.IO.File.OpenText(dir + "\\wwwroot\\mailTemplate\\" + model.Folder + "\\mailTemplate.html"))
                {
                    model.Template = htmlRead.ReadToEnd();
                }
            }

            return model;
        }

        [SecurityDescription("MailTemplate - Guardar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Guardar([FromBody]Gq_mailTemplateDto model)
        {
            ReturnData result = new ReturnData();
            try
            {
                var resultsValidation = new List<ValidationResult>();
                if (ValidateUtils.TryValidateModel(model, resultsValidation))
                {
                    using (var transaction = Services.session.BeginTransaction())
                    {
                        try
                        {
                            var entity = model.GetEntity();

                            if (!string.IsNullOrWhiteSpace(entity.Folder))
                            {
                                entity.CodeSharp = "";
                                entity.Template = "";
                            }

                            if (entity.Id == null || entity.Id.Equals(0))
                            {
                                entity.Estado = Constantes.ESTADO_ACTIVO;
                                entity.CreadoPor = (long)com.gq.security.Security.usuarioLogueado.UsuarioId;
                                entity.Creado = DateTime.Now;
                                entity = Services.Get<ServGq_mailTemplate>().Agregar(entity);
                            }
                            else
                            {
                                entity.Modificado = DateTime.Now;
                                entity.ModificadoPor = (long)com.gq.security.Security.usuarioLogueado.UsuarioId;
                                Services.Get<ServGq_mailTemplate>().Actualizar(entity);
                            }

                            if (!string.IsNullOrWhiteSpace(entity.Folder))
                            {
                                var dir = System.IO.Directory.GetCurrentDirectory();
                                if (!System.IO.Directory.Exists(dir + "\\wwwroot\\mailTemplate\\" + model.Folder))
                                {
                                    System.IO.Directory.CreateDirectory(dir + "\\wwwroot\\mailTemplate\\" + model.Folder);
                                }
                                System.IO.File.WriteAllText(dir + "\\wwwroot\\mailTemplate\\" + model.Folder + "\\mailTemplate.cs", model.CodeSharp);
                                System.IO.File.WriteAllText(dir + "\\wwwroot\\mailTemplate\\" + model.Folder + "\\mailTemplate.html", model.Template);                                                               
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            result.isError = true;
                            result.data = GenericError.Create(ex);
                            transaction.Rollback();
                        }
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

        [SecurityDescription("MailTemplate - Borrar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Borrar([FromBody]Gq_mailTemplateDto model)
        {
            ReturnData result = new ReturnData();

            using (var transaction = Services.session.BeginTransaction())
            {
                try
                {
                    var entity = Services.Get<ServGq_mailTemplate>().findById(model.Id);
                    entity.Estado = Constantes.ESTADO_BORRADO;
                    Services.Get<ServGq_mailTemplate>().Actualizar(entity);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.data = "Ocurrió el siguiente error al intentar borrar el Mail Template: " + ex.Message;
                    result.isError = true;
                }
            }
            return result;
        }
    }
}
