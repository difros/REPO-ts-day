using System.ComponentModel.DataAnnotations;
using GQDataService.com.gq.domain;
using GQDataService.com.gq.dto.codegen;

namespace GQDataService.com.gq.dto
{
    public class Gq_graficoDto : _Gq_graficoDto
    {
        public Gq_graficoDto():base()
        {
        }
       
        public Gq_graficoDto(Gq_grafico value):base(value)
        {
        }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Nombre { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Descripcion { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Folder { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Template { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Scritp { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string CodeSharp { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Estado { get; set; }
        
    }
}
