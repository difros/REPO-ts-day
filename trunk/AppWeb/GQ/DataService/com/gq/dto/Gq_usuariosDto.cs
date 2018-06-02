using System.ComponentModel.DataAnnotations;
using GQDataService.com.gq.constantes;
using GQDataService.com.gq.domain;
using GQDataService.com.gq.service;
using GQService.com.gq.encriptation;
using GQService.com.gq.service;
using GQService.com.gq.validate;
using GQDataService.com.gq.dto.codegen;

namespace GQDataService.com.gq.dto
{
    public class Gq_usuariosDto : _Gq_usuariosDto
    {
        public Gq_usuariosDto():base()
        {
        }
       
        public Gq_usuariosDto(Gq_usuarios value):base(value)
        {
        }
        
        [Required(ErrorMessage = "Campo requerido")]
        public override string Usuario { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Nombre { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Apellido { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Email { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public override string Clave { get; set; }

        [FunctionValidator(typeof(Gq_usuariosDto), "PasswordValidar")]
        public object ClaveChequed { get; set; }

        /// <summary>
        /// Validate Function
        /// </summary>
        /// <param name="value"></param>
        /// <param name="ObjectInstance"></param>
        /// <returns></returns>
        public static bool PasswordValidar(object value, object ObjectInstance)
        {
            Gq_usuariosDto data = (Gq_usuariosDto)ObjectInstance;
            if (string.IsNullOrWhiteSpace(data.Clave) == false)
            {
                if (data.Clave.Equals(data.ClaveChequed))
                {
                    return true;
                }
                else if (Encriptacion.Desencriptar(data.Clave, Constantes.CLAVE_ENCRIPTACION).Contains("Wrong Input. ") == false)
                {
                    return true;
                }
            }

            return false;
        }

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
    }
}
