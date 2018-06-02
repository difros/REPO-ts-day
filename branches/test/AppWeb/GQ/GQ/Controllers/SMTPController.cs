using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GQDataService.com.gq.constantes;
using GQDataService.com.gq.domain;
using GQDataService.com.gq.dto;
using GQDataService.com.gq.service;
using GQService.com.gq.controller;
using GQService.com.gq.data;
using GQService.com.gq.exception;
using GQService.com.gq.menu;
using GQService.com.gq.paging;
using GQService.com.gq.security;
using GQService.com.gq.service;
using GQService.com.gq.validate;
using Microsoft.AspNetCore.Mvc;

namespace GQ.Controllers
{
    [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
    public class SMTPController : BaseController, IABM<Gq_smtp_configDto>
    {
        [MenuDescription("90-40-00", "SMTP", GQ.com.gq.security.Security.MENU_CONFIG_ID)]
        [SecurityDescription("SMTP", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public IActionResult Index()
        {
            return PartialView();
        }
        
        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public Paging Buscar([FromBody]Paging paging)
        {
            var query = Services.Get<ServGq_smtp_config>().findBy(x => x.Nombre != string.Empty);
            paging.Apply<Gq_smtp_config, Gq_smtp_configDto>(query);
            return paging;
        }
        
        [SecurityDescription("SMTP - Guardar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Guardar([FromBody]Gq_smtp_configDto model)
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
                            if (entity.Id == null)
                            {                                
                                entity = Services.Get<ServGq_smtp_config>().Agregar(entity);
                            }
                            else
                            {                                
                                Services.Get<ServGq_smtp_config>().Actualizar(entity);
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

        [SecurityDescription("SMTP - Borrar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Borrar([FromBody]Gq_smtp_configDto model)
        {
            ReturnData result = new ReturnData();

            using (var transaction = Services.session.BeginTransaction())
            {
                try
                {
                    var entity = Services.Get<ServGq_smtp_config>().findById(model.Id);
                    Services.Get<ServGq_smtp_config>().Actualizar(entity);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.data = "Ocurrió el siguiente error al intentar borrar el grafico: " + ex.Message;
                    result.isError = true;
                }
            }
            return result;
        }
        
    }
}
