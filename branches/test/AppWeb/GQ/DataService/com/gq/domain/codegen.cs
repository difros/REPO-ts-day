using GQService.com.gq.dto;
using System;

namespace GQDataService.com.gq.domain.codegen
{

    public class _Gq_accesos : IEntity
    {

        public virtual System.Int64? AccesoId { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String Descripcion { get; set; }


        public virtual System.Int64 Tipo { get; set; }


        public virtual System.String Clase { get; set; }


        public virtual System.String Metodo { get; set; }


        public virtual System.String Orden { get; set; }


    }

    public class _Gq_archivos : IEntity
    {

        public virtual System.Int64? Id { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String NombreOriginal { get; set; }


        public virtual System.String Extension { get; set; }


        public virtual System.Int16? Tipo { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime? Creado { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? CreadoPor { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }

    public class _Gq_formularios : IEntity
    {

        public virtual System.Int64? Id { get; set; }


        public virtual System.String Texto { get; set; }


        public virtual System.String Opcion { get; set; }


        public virtual System.DateTime? Fecha { get; set; }


        public virtual System.DateTime? Hora { get; set; }


        public virtual System.Int64 IdArchivo { get; set; }


        public virtual System.Boolean CheckBox { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime? Creado { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? CreadoPor { get; set; }

        
        public virtual System.Int64? ModificadoPor { get; set; }


        public virtual System.Int64? IdUsuario { get; set; }

    }

    public class _Gq_grafico : IEntity
    {

        public virtual System.Int64? Id { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String Descripcion { get; set; }


        public virtual System.String Folder { get; set; }


        public virtual System.String Template { get; set; }


        public virtual System.String Scritp { get; set; }


        public virtual System.String CodeSharp { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime Creado { get; set; }


        public virtual System.Int64 CreadoPor { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }

    public class _Gq_menu : IEntity
    {

        public virtual System.Int64? MenuId { get; set; }


        public virtual System.String MenuPosition { get; set; }


        public virtual System.String MenuUrl { get; set; }


        public virtual System.String MenuIcono { get; set; }


        public virtual System.String KeyName { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String MenuPadre { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime? Creado { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? CreadoPor { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }

    public class _Gq_perfiles : IEntity
    {

        public virtual System.Int64? PerfilId { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String KeyName { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime? Creado { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? CreadoPor { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }

    public class _Gq_perfiles_accesos : IEntity
    {

        public virtual System.Int64? PerfilesAccesosId { get; set; }


        public virtual System.Int64 PerfilId { get; set; }


        public virtual System.Int64 AccesoId { get; set; }


        public virtual System.String GrantPermition { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime? Creado { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? CreadoPor { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }

    public class _Gq_smtp_config : IEntity
    {

        public virtual System.Int64? Id { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String NombreFrom { get; set; }


        public virtual System.String UserName { get; set; }


        public virtual System.String Pass { get; set; }


        public virtual System.String Host { get; set; }


        public virtual System.Int32 Port { get; set; }


        public virtual System.Boolean UseDefaultCredentials { get; set; }


        public virtual System.Boolean EnableSsl { get; set; }


        public virtual System.String EMailFrom { get; set; }


    }

    public class _Gq_usuarios : IEntity
    {

        public virtual System.Int64? UsuarioId { get; set; }


        public virtual System.String Usuario { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String Apellido { get; set; }


        public virtual System.String Email { get; set; }


        public virtual System.String Clave { get; set; }


        public virtual System.Int64 PerfilId { get; set; }


        public virtual System.String RequiereClave { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime? Creado { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? CreadoPor { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }

    public class _Gq_mailTemplate : IEntity
    {

        public virtual System.Int64? Id { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String Folder { get; set; }


        public virtual System.String Template { get; set; }


        public virtual System.String CodeSharp { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime Creado { get; set; }


        public virtual System.Int64 CreadoPor { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }
}
