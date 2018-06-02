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

    [SecurityDescription("Perfiles", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
    public class PerfilController : BaseController, IABM<Gq_perfilesDto>
    {
        [MenuDescription("90-20-00", "Perfiles", GQ.com.gq.security.Security.MENU_CONFIG_ID)]
        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public IActionResult Index()
        {
            return PartialView();
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public Paging Buscar([FromBody]Paging paging)
        {
            var query = Services.Get<ServGq_perfiles>().findBy(x => x.Estado != Constantes.ESTADO_BORRADO);
            paging.Apply<Gq_perfiles, Gq_perfilesDto>(query);
            return paging;
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public IEnumerable<Gq_accesosDto> GetAccesos()
        {
            return new Gq_accesosDto().SetEntity(Services.Get<ServGq_accesos>().findBy().OrderBy(acceso => acceso.Nombre));
        }

        [SecurityDescription("Perfiles - Guardar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Guardar([FromBody]Gq_perfilesDto model)
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
                            if (entity.PerfilId == null || entity.PerfilId.Equals(0))
                            {
                                entity.CreadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                                entity.Creado = DateTime.Now;
                                entity = Services.Get<ServGq_perfiles>().Agregar(entity);
                            }
                            else
                            {
                                entity.Modificado = DateTime.Now;
                                entity.ModificadoPor = com.gq.security.Security.usuarioLogueado.UsuarioId;
                                Services.Get<ServGq_perfiles>().Actualizar(entity);
                            }

                            #region Búsqueda y eliminación y creación de PerfilesAcesos

                            var accesosDePerfil = Services.Get<ServGq_perfiles_accesos>().findBy(x => x.PerfilId == entity.PerfilId).ToList();
                            Services.Get<ServGq_perfiles_accesos>().Borrar(accesosDePerfil);

                            foreach (var item in model.Accesos)
                            {
                                item.PerfilId = entity.PerfilId.Value;
                                item.Estado = Constantes.ESTADO_ACTIVO;
                                item.CreadoPor = entity.CreadoPor;
                                item.Creado = entity.Creado;
                                item.Modificado = entity.Modificado;
                                item.ModificadoPor = entity.ModificadoPor;
                            }

                            List<Gq_perfiles_accesos> perfilesAccesos = new Gq_perfiles_accesosDto().GetEntity(model.Accesos).ToList();
                            Services.Get<ServGq_perfiles_accesos>().Agregar(perfilesAccesos);

                            #endregion

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

        [SecurityDescription("Perfiles - Borrar", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public ReturnData Borrar([FromBody]Gq_perfilesDto model)
        {
            ReturnData result = new ReturnData();

            using (var transaction = Services.session.BeginTransaction())
            {
                try
                {
                    var entity = Services.Get<ServGq_perfiles>().findById(model.PerfilId);
                    entity.Estado = Constantes.ESTADO_BORRADO;
                    Services.Get<ServGq_perfiles>().Actualizar(entity);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.data = "Ocurrió el siguiente error al intentar borrar el perfil: " + ex.Message;
                    result.isError = true;
                }
            }
            return result;
        }
    }
}
