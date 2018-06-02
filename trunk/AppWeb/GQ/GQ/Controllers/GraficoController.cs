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
using static GQ.com.gq.graficos.ProcesarGraficos;
using GQ.com.gq.graficos;

namespace GQ.Controllers
{
    [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
    public class GraficoController : BaseController, IABM<Gq_graficoDto>
    {
        [MenuDescription("90-30-00", "Graficos", GQ.com.gq.security.Security.MENU_CONFIG_ID)]
        [SecurityDescription("Graficos", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public IActionResult Index()
        {
            return PartialView();
        }
        
        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public Paging Buscar([FromBody]Paging paging)
        {
            var query = Services.Get<ServGq_grafico>().findBy(x => x.Estado != Constantes.ESTADO_BORRADO);
            paging.Apply<Gq_grafico, Gq_graficoDto>(query);
            return paging;
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        [Route("[controller]/[action]/{id}")]
        public Gq_graficoDto GetGrafico(long id)
        {
            Gq_graficoDto model = new Gq_graficoDto().SetEntity(Services.Get<ServGq_grafico>().findById(id));
            if (!string.IsNullOrWhiteSpace(model.Folder))
            {
                var dir = System.IO.Directory.GetCurrentDirectory();

                using (var csRead = System.IO.File.OpenText(dir + "\\wwwroot\\graficos\\" + model.Folder + "\\grafico.cs"))
                {
                    model.CodeSharp = csRead.ReadToEnd();
                }

                using (var jsRead = System.IO.File.OpenText(dir + "\\wwwroot\\graficos\\" + model.Folder + "\\grafico.js"))
                {
                    model.Scritp = jsRead.ReadToEnd();
                }

                using (var htmlRead = System.IO.File.OpenText(dir + "\\wwwroot\\graficos\\" + model.Folder + "\\grafico.html"))
                {
                    model.Template = htmlRead.ReadToEnd();
                }
            }

            return model;
        }

        [SecurityDescription("Grafico - Guardar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Guardar([FromBody]Gq_graficoDto model)
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
                                entity.Scritp = "";
                                entity.Template = "";
                            }

                            if (entity.Id == null)
                            {
                                entity.Estado = Constantes.ESTADO_ACTIVO;
                                entity.CreadoPor = (long)com.gq.security.Security.usuarioLogueado.UsuarioId;
                                entity.Creado = DateTime.Now;
                                entity.Modificado = DateTime.Now;
                                entity = Services.Get<ServGq_grafico>().Agregar(entity);
                            }
                            else
                            {
                                entity.Modificado = DateTime.Now;
                                entity.ModificadoPor = (long)com.gq.security.Security.usuarioLogueado.UsuarioId;
                                Services.Get<ServGq_grafico>().Actualizar(entity);
                            }

                            if (!string.IsNullOrWhiteSpace(entity.Folder))
                            {
                                var dir = System.IO.Directory.GetCurrentDirectory();
                                if (!System.IO.Directory.Exists(dir + "\\wwwroot\\graficos\\" + model.Folder))
                                {
                                    System.IO.Directory.CreateDirectory(dir + "\\wwwroot\\graficos\\" + model.Folder);
                                }
                                System.IO.File.WriteAllText(dir + "\\wwwroot\\graficos\\" + model.Folder + "\\grafico.cs", model.CodeSharp);
                                System.IO.File.WriteAllText(dir + "\\wwwroot\\graficos\\" + model.Folder + "\\grafico.js", model.Scritp);
                                System.IO.File.WriteAllText(dir + "\\wwwroot\\graficos\\" + model.Folder + "\\grafico.html", model.Template);
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

        [SecurityDescription("Grafico - Borrar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Borrar([FromBody]Gq_graficoDto model)
        {
            ReturnData result = new ReturnData();

            using (var transaction = Services.session.BeginTransaction())
            {
                try
                {
                    var entity = Services.Get<ServGq_grafico>().findById(model.Id);
                    entity.Estado = Constantes.ESTADO_BORRADO;
                    Services.Get<ServGq_grafico>().Actualizar(entity);
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

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public dynamic Ejecutar([FromBody]EjecutarDto model)
        {
            dynamic result = null;
            result = ProcesarGraficos.Ejecutar(model);
            return result;
        }
    }
}
