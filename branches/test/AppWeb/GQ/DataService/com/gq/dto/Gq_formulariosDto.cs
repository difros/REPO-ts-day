
using GQDataService.com.gq.domain;
using GQDataService.com.gq.dto.codegen;
using GQDataService.com.gq.service;
using GQService.com.gq.service;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GQDataService.com.gq.dto
{
    public class Gq_formulariosDto : _Gq_formulariosDto
    {
        public Gq_formulariosDto() : base()
        {
        }

        public Gq_formulariosDto(Gq_formularios value) : base(value)
        {
        }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Texto { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Opcion { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override DateTime? Fecha { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override DateTime? Hora { get; set; }

        public Boolean deletePicture { get; set; }

        public string ModificadoPorNombre
        {
            get
            {
                if (this.ModificadoPor.HasValue && this.ModificadoPor.Value > 0)
                {
                    return Services.Get<ServGq_usuarios>().findById(this.ModificadoPor.Value).Nombre;
                }
                else return "";

            }
        }

        public class ArchivoFoto
        {
            public string Url { get; set; }
            public long Id { get; set; }
            public string Extension { get; set; }
            public string NombreOriginal { get; set; }

        }

        public ArchivoFoto UrlArchivo
        {
            get
            {
                var oArchivo = Services.Get<ServGq_archivos>().findById(this.IdArchivo);
                //var oArchivo = Services.Get<ServGq_archivos>().findBy().Where(x => x.Id==this.IdArchivo && x.Tipo == constantes.Constantes.DicTipoArchivo[ptipoNombre]).FirstOrDefault();

                if (oArchivo != null)
                {
                    var path = "\\upload\\imagen-formulario\\";
                    return new ArchivoFoto { Url = path + oArchivo.Nombre + "." + oArchivo.Extension, Id = oArchivo.Id.Value, NombreOriginal = oArchivo.NombreOriginal, Extension = oArchivo.Extension };
                }
                else
                    return null;
            }
        }
    }
}
