using GQ.Data.dto;
using System.ComponentModel.DataAnnotations;

namespace WebNetCore.Data.sql.domain.codegen
{
    public class _GQ_Accesos : IEntity
    {
        [Key]
        public virtual System.Int64? Id { get; set; }

        [MaxLength(45)]
        [Required]
        public virtual System.String Nombre { get; set; }

        [MaxLength(255)]
        [Required]
        public virtual System.String ClassName { get; set; }

        [MaxLength(255)]
        public virtual System.String MethodName { get; set; }


    }

    public class _GQ_Menues : IEntity
    {
        [Key]
        public virtual System.Int64? Id { get; set; }

        [MaxLength(128)]
        [Required]
        public virtual System.String Nombre { get; set; }

        [MaxLength(128)]
        [Required]
        public virtual System.String MenuPosition { get; set; }

        [MaxLength(128)]
        public virtual System.String KeyName { get; set; }

        [MaxLength(128)]
        public virtual System.String MenuPadre { get; set; }

        [MaxLength(128)]
        public virtual System.String MenuUrl { get; set; }

        [MaxLength(128)]
        public virtual System.String MenuIcono { get; set; }

        [MaxLength(1)]
        public virtual System.String Estado { get; set; }

        public virtual System.DateTime? Creado { get; set; }

        public virtual System.Int64? CreadoPor { get; set; }

        public virtual System.DateTime? Modificado { get; set; }

        public virtual System.Int64? ModificadoPor { get; set; }


    }

    public class _GQ_Perfiles : IEntity
    {
        [Key]
        public virtual System.Int64? Id { get; set; }

        [MaxLength(45)]
        [Required]
        public virtual System.String Nombre { get; set; }

        [MaxLength(50)]
        public virtual System.String KeyName { get; set; }

        [MaxLength(1)]
        public virtual System.String Estado { get; set; }

        public virtual System.DateTime? Creado { get; set; }

        public virtual System.Int64? CreadoPor { get; set; }

        public virtual System.DateTime? Modificado { get; set; }

        public virtual System.Int64? ModificadoPor { get; set; }


    }

    public class _GQ_Perfiles_Accesos : IEntity
    {
        [Key]
        public virtual System.Int64? Id { get; set; }

        [Required]
        public virtual System.Int64 PerfilId { get; set; }

        [Required]
        public virtual System.Int64 AccesoId { get; set; }

        [Required]
        public virtual System.Boolean Permite { get; set; }


    }

    public class _GQ_Usuarios : IEntity
    {
        [Key]
        public virtual System.Int64? Id { get; set; }

        [MaxLength(128)]
        [Required]
        public virtual System.String NombreUsuario { get; set; }

        [MaxLength(128)]
        [Required]
        public virtual System.String Nombre { get; set; }

        [MaxLength(128)]
        [Required]
        public virtual System.String Apellido { get; set; }

        [MaxLength(128)]
        [Required]
        public virtual System.String Email { get; set; }

        [MaxLength(128)]
        [Required]
        public virtual System.String Password { get; set; }

        [Required]
        public virtual System.Boolean RequiereContraseña { get; set; }

        [MaxLength(48)]
        public virtual System.String EmpresaId { get; set; }

        [MaxLength(1)]
        public virtual System.String Estado { get; set; }

        public virtual System.DateTime? Creado { get; set; }

        public virtual System.Int64? CreadoPor { get; set; }

        public virtual System.DateTime? Modificado { get; set; }

        public virtual System.Int64? ModificadoPor { get; set; }


    }
    
    public class _GQ_Usuarios_Perfiles : IEntity
    {
        [Key]
        public virtual System.Int64? Id { get; set; }

        [Required]
        public virtual System.Int64 UsuarioId { get; set; }

        [Required]
        public virtual System.Int64 PerfilId { get; set; }


    }

}
