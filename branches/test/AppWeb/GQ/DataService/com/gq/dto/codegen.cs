
using GQDataService.com.gq.domain;
using System;
using System.Collections.Generic;
using GQService.com.gq.dto;

namespace GQDataService.com.gq.dto.codegen
{


    public class _Gq_accesosDto : Dto<Gq_accesos,Gq_accesosDto>
    {
    	public _Gq_accesosDto() : base()
    	{
    	}
    	
    	public _Gq_accesosDto( Gq_accesos value) : base(value)
    	{
    	}

        public virtual System.Int64? AccesoId { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String Descripcion { get; set; }


        public virtual System.Int64 Tipo { get; set; }


        public virtual System.String Clase { get; set; }


        public virtual System.String Metodo { get; set; }


        public virtual System.String Orden { get; set; }


    }


    public class _Gq_archivosDto : Dto<Gq_archivos,Gq_archivosDto>
    {
    	public _Gq_archivosDto() : base()
    	{
    	}
    	
    	public _Gq_archivosDto( Gq_archivos value) : base(value)
    	{
    	}

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


    public class _Gq_formulariosDto : Dto<Gq_formularios,Gq_formulariosDto>
    {
    	public _Gq_formulariosDto() : base()
    	{
    	}
    	
    	public _Gq_formulariosDto( Gq_formularios value) : base(value)
    	{
    	}

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


    public class _Gq_graficoDto : Dto<Gq_grafico,Gq_graficoDto>
    {
    	public _Gq_graficoDto() : base()
    	{
    	}
    	
    	public _Gq_graficoDto( Gq_grafico value) : base(value)
    	{
    	}

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


    public class _Gq_menuDto : Dto<Gq_menu,Gq_menuDto>
    {
    	public _Gq_menuDto() : base()
    	{
    	}
    	
    	public _Gq_menuDto( Gq_menu value) : base(value)
    	{
    	}

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


    public class _Gq_perfilesDto : Dto<Gq_perfiles,Gq_perfilesDto>
    {
    	public _Gq_perfilesDto() : base()
    	{
    	}
    	
    	public _Gq_perfilesDto( Gq_perfiles value) : base(value)
    	{
    	}

        public virtual System.Int64? PerfilId { get; set; }


        public virtual System.String Nombre { get; set; }


        public virtual System.String KeyName { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime? Creado { get; set; }


        public virtual System.DateTime? Modificado { get; set; }


        public virtual System.Int64? CreadoPor { get; set; }


        public virtual System.Int64? ModificadoPor { get; set; }


    }


    public class _Gq_perfiles_accesosDto : Dto<Gq_perfiles_accesos,Gq_perfiles_accesosDto>
    {
    	public _Gq_perfiles_accesosDto() : base()
    	{
    	}
    	
    	public _Gq_perfiles_accesosDto( Gq_perfiles_accesos value) : base(value)
    	{
    	}

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


    public class _Gq_smtp_configDto : Dto<Gq_smtp_config,Gq_smtp_configDto>
    {
    	public _Gq_smtp_configDto() : base()
    	{
    	}
    	
    	public _Gq_smtp_configDto( Gq_smtp_config value) : base(value)
    	{
    	}

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


    public class _Gq_usuariosDto : Dto<Gq_usuarios,Gq_usuariosDto>
    {
    	public _Gq_usuariosDto() : base()
    	{
    	}
    	
    	public _Gq_usuariosDto( Gq_usuarios value) : base(value)
    	{
    	}

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


    public class _Gq_mailTemplateDto : Dto<Gq_mailTemplate, Gq_mailTemplateDto>
    {
        public _Gq_mailTemplateDto() : base()
        {
        }

        public _Gq_mailTemplateDto(Gq_mailTemplate value) : base(value)
        {
        }

        public virtual System.Int64? Id { get; set; }


        public virtual System.String Nombre { get; set; }
        

        public virtual System.String Folder { get; set; }


        public virtual System.String Template { get; set; }


        public virtual System.String CodeSharp { get; set; }


        public virtual System.String Estado { get; set; }


        public virtual System.DateTime Creado { get; set; }


        public virtual System.Int64 CreadoPor { get; set; }


        public virtual System.DateTime Modificado { get; set; }


        public virtual System.Int64 ModificadoPor { get; set; }


    }
}
