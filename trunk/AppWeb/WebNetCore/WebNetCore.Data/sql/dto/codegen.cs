using GQ.Data.dto;
using WebNetCore.Data.sql.domain;

namespace WebNetCore.Data.sql.dto.codegen
{
    public class _GQ_AccesosDto : Dto<GQ_Accesos, GQ_AccesosDto>
    {
        public _GQ_AccesosDto() : base()
        {
        }

        public _GQ_AccesosDto(GQ_Accesos value) : base(value)
        {
        }

        public virtual System.Int64? Id { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String ClassName { get; set; }


        public virtual System.String MethodName { get; set; }


    }


    public class _GQ_MenuesDto : Dto<GQ_Menues, GQ_MenuesDto>
    {
        public _GQ_MenuesDto() : base()
        {
        }

        public _GQ_MenuesDto(GQ_Menues value) : base(value)
        {
        }

        public virtual System.Int64? Id { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String MenuPosition { get; set; }


        public virtual System.String KeyName { get; set; }


        public virtual System.String MenuPadre { get; set; }


        public virtual System.String MenuUrl { get; set; }


        public virtual System.String MenuIcono { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime? Creado { get; set; }


        public virtual System.Int64? CreadoPor { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }


    public class _GQ_PerfilesDto : Dto<GQ_Perfiles, GQ_PerfilesDto>
    {
        public _GQ_PerfilesDto() : base()
        {
        }

        public _GQ_PerfilesDto(GQ_Perfiles value) : base(value)
        {
        }

        public virtual System.Int64? Id { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String KeyName { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime? Creado { get; set; }


        public virtual System.Int64? CreadoPor { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }


    public class _GQ_Perfiles_AccesosDto : Dto<GQ_Perfiles_Accesos, GQ_Perfiles_AccesosDto>
    {
        public _GQ_Perfiles_AccesosDto() : base()
        {
        }

        public _GQ_Perfiles_AccesosDto(GQ_Perfiles_Accesos value) : base(value)
        {
        }

        public virtual System.Int64? Id { get; set; }


        public virtual System.Int64 PerfilId { get; set; }


        public virtual System.Int64 AccesoId { get; set; }


        public virtual System.Boolean Permite { get; set; }


    }


    public class _GQ_UsuariosDto : Dto<GQ_Usuarios, GQ_UsuariosDto>
    {
        public _GQ_UsuariosDto() : base()
        {
        }

        public _GQ_UsuariosDto(GQ_Usuarios value) : base(value)
        {
        }

        public virtual System.Int64? Id { get; set; }


        public virtual System.String NombreUsuario { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String Apellido { get; set; }


        public virtual System.String Email { get; set; }


        public virtual System.String Password { get; set; }


        public virtual System.Boolean RequiereContraseña { get; set; }


        public virtual System.String EmpresaId { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime? Creado { get; set; }


        public virtual System.Int64? CreadoPor { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }


    public class _GQ_Usuarios_PerfilesDto : Dto<GQ_Usuarios_Perfiles, GQ_Usuarios_PerfilesDto>
    {
        public _GQ_Usuarios_PerfilesDto() : base()
        {
        }

        public _GQ_Usuarios_PerfilesDto(GQ_Usuarios_Perfiles value) : base(value)
        {
        }

        public virtual System.Int64? Id { get; set; }


        public virtual System.Int64 UsuarioId { get; set; }


        public virtual System.Int64 PerfilId { get; set; }


    }

}
