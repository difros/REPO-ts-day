using GQService.com.gq.controller;
using GQService.com.gq.security;
using GQService.com.gq.service;
using System.Collections.Generic;
using System.Linq;
using GQDataService.com.gq.constantes;
using GQDataService.com.gq.dto;
using GQDataService.com.gq.service;
using Microsoft.AspNetCore.Mvc.Localization;

namespace GQ.Controllers
{
    [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
    public class MenuController : BaseController
    {
        private IHtmlLocalizer<Idioma> Localizer;
        public MenuController(IHtmlLocalizer<Idioma> Localizer)
        {
            this.Localizer = Localizer;
        }

        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        public List<Gq_menuDto> Buscar()
        {
            List<Gq_menuDto> roots = new List<Gq_menuDto>();
            var query = new Gq_menuDto().SetEntity(Services.Get<ServGq_menu>().findBy(x => x.Estado == Constantes.ESTADO_ACTIVO).OrderBy(x => x.MenuPosition).ToList());
            foreach (var item in query)
            {
                if (string.IsNullOrWhiteSpace(((Gq_menuDto)item).MenuPadre))
                {
                    var items = query.Where(x => x.MenuPadre == ((Gq_menuDto)item).MenuPosition).OrderBy(x => x.MenuPosition).ToList();

                    foreach (var mi in items)
                    {
                        if (GQ.com.gq.security.Security.hasControllerPermission(mi.MenuUrl) == true)
                        {
                            var tras = Localizer[("menu_" + mi.MenuPosition).Replace("-", "_")].Value;
                            if (tras != ("menu_" + mi.MenuPosition).Replace("-", "_"))
                                mi.Nombre = tras;
                            ((Gq_menuDto)item).Child.Add(mi);
                        }
                    }
                    if (((Gq_menuDto)item).Child.Count > 0)
                    {
                        var tras = Localizer[("menu_" + item.MenuPosition).Replace("-", "_")].Value;
                        if (tras != ("menu_" + item.MenuPosition).Replace("-", "_"))
                            item.Nombre = tras;
                        roots.Add(((Gq_menuDto)item));
                    }
                }
            }
            return roots;
        }

    }
}
