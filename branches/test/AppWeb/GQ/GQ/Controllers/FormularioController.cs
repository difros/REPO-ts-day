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
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace GQ.Controllers
{
    [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
    public class FormularioController : BaseController/*, IABM<Gq_formulariosDto>*/
    {
        [MenuDescription("90-60-00", "Formularios", GQ.com.gq.security.Security.MENU_CONFIG_ID)]
        [SecurityDescription("Formularios", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public IActionResult Index()
        {
            return PartialView();
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public Paging Buscar([FromBody]Paging paging)
        {
            var query = Services.Get<ServGq_formularios>().findBy(x => x.Texto != string.Empty);
            paging.Apply<Gq_formularios, Gq_formulariosDto>(query);

            return paging;
        }

        [SecurityDescription("Formularios - Borrar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Borrar([FromBody]Gq_formulariosDto model)
        {
            ReturnData result = new ReturnData();
            using (var transaction = Services.session.BeginTransaction())
            {
                try
                {
                    var entity = Services.Get<ServGq_formularios>().findById(model.Id);
                    var update = entity.Estado = Constantes.ESTADO_BORRADO;
                    entity.Modificado = DateTime.Now;
                    entity.ModificadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                    Services.Get<ServGq_formularios>().Actualizar(entity);
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

        [SecurityDescription("Formularios - Guardar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        [Consumes("multipart/form-data")]
        public ReturnData Guardar()
        {
            ReturnData result = new ReturnData();
            try
            {
                var resultsValidation = new List<ValidationResult>();
                StringValues json = "";
                Request.Form.TryGetValue("JsonOject", out json);
                Gq_formulariosDto model = Newtonsoft.Json.JsonConvert.DeserializeObject<Gq_formulariosDto>(json.ToString());

                if (ValidateUtils.TryValidateModel(model, resultsValidation))
                {
                    using (var transaction = Services.session.BeginTransaction())
                    {
                        try
                        {
                            if (model.Hora != null)
                            {
                                model.Hora = model.Hora.Value.ToLocalTime();
                            }

                            var entity = model.GetEntity();

                            if (Request.Form.Files.Count > 0)
                            {
                                var PartesFile = Request.Form.Files[0].FileName.Split('.');
                                string FileExtension = PartesFile[PartesFile.Length - 1]; // me quedo con el ultimo punto que encuentro en el nombre
                                string[] values = { "jpg", "png", "gif","jpeg","bmp" };
                                if (!values.Contains(FileExtension))
                                {
                                    result.isError = true;
                                    result.data = "No se acepta el tipo de archivo. Únicamente imágenes con los siguientes formatos: jpg, png, bmp o gif";
                                    return result;
                                }

                                if (entity.IdArchivo != 0)
                                {
                                    eliminarArchivo(entity.IdArchivo);
                                }
                                var idArchivo = SaveFile(Request.Form.Files[0]);
                                entity.IdArchivo = idArchivo;
                            }
                            else
                            {
                                if (entity.IdArchivo != 0 && model.deletePicture)
                                {
                                    eliminarArchivo(entity.IdArchivo);
                                    entity.IdArchivo = 0;
                                }
                            }


                            if (entity.Id == null)
                            {
                                entity.Creado = DateTime.Now;
                                entity.CreadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                                entity.Modificado = DateTime.Now;
                                entity.ModificadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                                Services.Get<ServGq_formularios>().Agregar(entity);
                            }
                            else
                            {
                                entity.Modificado = DateTime.Now;
                                entity.ModificadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                                Services.Get<ServGq_formularios>().Actualizar(entity);
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

        private void eliminarArchivo(long pIdArchivo)
        {
            var oArchivo = Services.Get<ServGq_archivos>().findById(pIdArchivo);

            if (oArchivo != null)
            {
                //elimino el archivo fisico
                var path = Directory.GetCurrentDirectory() + "\\wwwroot\\upload\\imagen-formulario\\";
                System.IO.File.Delete(path + oArchivo.Nombre + "." + oArchivo.Extension);
                //elimino el arhivo de la BD
                Services.Get<ServGq_archivos>().Borrar(oArchivo);
            }
        }

        private long SaveFile(IFormFile file)
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + "\\wwwroot\\upload\\imagen-formulario\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var NewNamefile = Guid.NewGuid().ToString();
                var PartesFile = file.FileName.Split('.');
                var FileExtension = PartesFile[PartesFile.Length - 1]; // me quedo con el ultimo punto que encuentro en el nombre
                var NombreOriginal = file.FileName;

                using (var fw = System.IO.File.Create(path + NewNamefile + "." + FileExtension))
                {
                    file.CopyTo(fw);
                }

                //guardo los archivos en la base
                Gq_archivos oArchivo = new Gq_archivos();
                oArchivo.Extension = FileExtension;
                oArchivo.Nombre = NewNamefile;
                oArchivo.NombreOriginal = NombreOriginal;
                oArchivo.Estado = Constantes.ESTADO_ACTIVO;
                oArchivo.Creado = DateTime.Now;
                oArchivo.CreadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                //var intTipoArchivo = Constantes.DicTipoArchivo[f.Name];

                //if (intTipoArchivo != -1 && intTipoArchivo != 0)
                //    oArchivo.Tipo = (Int16)intTipoArchivo;


                Services.Get<ServGq_archivos>().Agregar(oArchivo);

                return oArchivo.Id.Value;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<long> SaveFiles(IFormFileCollection files)
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + "\\wwwroot\\upload\\imagen-formulario\\";
                List<long> lstIdArchivos = new List<long>();

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (var f in files)
                {
                    var NewNamefile = Guid.NewGuid().ToString();
                    var PartesFile = f.FileName.Split('.');
                    var FileExtension = PartesFile[PartesFile.Length - 1]; // me quedo con el ultimo punto que encuentro en el nombre
                    var NombreOriginal = f.FileName;

                    using (var fw = System.IO.File.Create(path + NewNamefile + "." + FileExtension))
                    {
                        f.CopyTo(fw);
                    }

                    //guardo los archivos en la base
                    Gq_archivos oArchivo = new Gq_archivos();
                    oArchivo.Extension = FileExtension;
                    oArchivo.Nombre = NewNamefile;
                    oArchivo.NombreOriginal = NombreOriginal;
                    oArchivo.Estado = Constantes.ESTADO_ACTIVO;
                    //var intTipoArchivo = Constantes.DicTipoArchivo[f.Name];

                    //if (intTipoArchivo != -1 && intTipoArchivo != 0)
                    //    oArchivo.Tipo = (Int16)intTipoArchivo;


                    Services.Get<ServGq_archivos>().Agregar(oArchivo);
                    lstIdArchivos.Add(oArchivo.Id.Value);

                }

                return lstIdArchivos;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}