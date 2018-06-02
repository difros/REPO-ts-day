using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GQService.com.gq.menu;
using GQService.com.gq.security;
using GQService.com.gq.service;
using GQService.com.gq.dto;
using GQService.com.gq.utils;
using GQService.com.gq.controller;

namespace GQ.Controllers
{
    [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
    public class MapaController : BaseController
    {
        [MenuDescription("90-70-00", "Mapa", GQ.com.gq.security.Security.MENU_CONFIG_ID)]
        [SecurityDescription("Mapa", new string[] { GQ.com.gq.security.Security.ROL_ADMI })]
        public IActionResult Index()
        {
            return PartialView();
        }


        [SecurityDescription(SecurityDescription.SeguridadEstado.SoloLogueo)]
        [Route("[controller]/[action]")]
        public List<Object> BuscarMarcadores()
        {
            var sql = Services.session.CreateSQLQuery(string.Format(@"
            SELECT Linea, Tipo, NI, NF, REPLACE(Long1,'.',',') as Long1, REPLACE(Lat1,'.',',') as Lat1, REPLACE(Long2,'.',',') as Long2, REPLACE(Lat2,'.',',') as Lat2
            FROM gq_xylineas            
            ORDER BY NI"));

            sql.SetResultTransformer(new NHibernate.Transform.AliasToBeanResultTransformer(typeof(XYLineasDto)));

            var items = sql.List<XYLineasDto>();

            Dictionary<string, Object> Nodo = new Dictionary<string, Object>();
            Dictionary<string, Object> Linea = new Dictionary<string, Object>();

            foreach (var item in items)
            {
                if (!Nodo.ContainsKey(item.NI))
                {
                    var nodo = new Marker { Id = item.NI, Position = new double[] { NumberUtils.ConvertTo<Double>(item.Lat1), NumberUtils.ConvertTo<Double>(item.Long1) }, Label = item.NI, FillColor = "#0000FF" };
                    Nodo.Add(nodo.Id, nodo);
                }
                if (!Nodo.ContainsKey(item.NF))
                {
                    var nodo = new Marker { Id = item.NF, Position = new double[] { NumberUtils.ConvertTo<Double>(item.Lat2), NumberUtils.ConvertTo<Double>(item.Long2) }, Label = item.NF, FillColor = "#0000FF" };
                    Nodo.Add(nodo.Id, nodo);
                }
                if (!Linea.ContainsKey(item.Linea))
                {
                    var nodo = new MarcadorMapa { Id = item.Linea, Polyline = new List<double[]>() { new double[] { NumberUtils.ConvertTo<Double>(item.Lat1), NumberUtils.ConvertTo<Double>(item.Long1) }, new double[] { NumberUtils.ConvertTo<Double>(item.Lat2), NumberUtils.ConvertTo<Double>(item.Long2) } }, Type = MarcadorMapa.TYPE_POLYLINEA, StrokeColor = "#0000FF", StrokeOpacity = 1, StrokeWeight = 4 };
                    Linea.Add(nodo.Id, nodo);
                }
            }
            var list = new List<Object>();

            list.AddRange(Nodo.Values);
            list.AddRange(Linea.Values);

            return list;
        }
    }

    public class MarcadorMapa
    {
        public const string TYPE_CIRCLE = "circle";
        public const string TYPE_POLYLINEA = "polyline";
        public const string TYPE_POLYGON = "polygon";
        public const string TYPE_RECTANGLE = "rectangle";
        public const string TYPE_GROUNDOVERLAY = "groundOverlay";
        public const string TYPE_IMAGE = "image";

        public string Id { get; set; }
        public string Type { get; set; }

        public string FillColor { get; set; }
        public double? FillOpacity { get; set; }

        public string StrokeColor { get; set; }
        public double? StrokeOpacity { get; set; }
        public double? StrokeWeight { get; set; }

        public List<double[]> Polyline { get; set; }
        public List<double[]> Polygon { get; set; }
        public List<double[]> Rectangle { get; set; }
        public double[] Circle { get; set; }
        public double? Radius { get; set; }

    }

    public class Marker
    {
        public const string TYPE_MARKER = "marker";

        public string Id { get; set; }
        public string Type { get; set; } = TYPE_MARKER;
        public string Label { get; set; }
        public string Icon { get; set; }
        public double[] Position { get; set; }
        public string FillColor { get; set; }
    }
}