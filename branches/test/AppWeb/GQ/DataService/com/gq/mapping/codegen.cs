
using FluentNHibernate.Mapping;
using GQDataService.com.gq.domain;

namespace GQDataService.com.gq.mapping.codegen
{

    public class _MapGq_accesos : ClassMap<Gq_accesos>
    {
        public _MapGq_accesos()
        {
        	Table("gq_accesos");
        	
			Id(c => c.AccesoId).GeneratedBy.Identity();
			Map(c => c.Nombre).Length(45);

			Map(c => c.Descripcion).Length(255);

			Map(c => c.Tipo);

			Map(c => c.Clase).Length(255);

			Map(c => c.Metodo).Length(255);

			Map(c => c.Orden).Length(15);

		}
    }

    public class _MapGq_archivos : ClassMap<Gq_archivos>
    {
        public _MapGq_archivos()
        {
        	Table("gq_archivos");
        	
			Id(c => c.Id).GeneratedBy.Identity();
			Map(c => c.Nombre).Length(255);

			Map(c => c.NombreOriginal).Length(255);

			Map(c => c.Extension).Length(4);

			Map(c => c.Tipo);

			Map(c => c.Estado).Length(1);

			Map(c => c.Creado);

			Map(c => c.Modificado);

			Map(c => c.CreadoPor);

			Map(c => c.ModificadoPor);

		}
    }

    public class _MapGq_formularios : ClassMap<Gq_formularios>
    {
        public _MapGq_formularios()
        {
        	Table("gq_formularios");
        	
			Id(c => c.Id).GeneratedBy.Identity();
			Map(c => c.Texto).Length(255);

			Map(c => c.Opcion).Length(255);

			Map(c => c.Fecha);

			Map(c => c.Hora);

			Map(c => c.IdArchivo);

			Map(c => c.CheckBox);

			Map(c => c.Estado).Length(1);

			Map(c => c.Creado);

			Map(c => c.Modificado);

			Map(c => c.CreadoPor);

			Map(c => c.ModificadoPor);

            Map(c => c.IdUsuario);

        }
    }

    public class _MapGq_grafico : ClassMap<Gq_grafico>
    {
        public _MapGq_grafico()
        {
        	Table("gq_grafico");
        	
			Id(c => c.Id).GeneratedBy.Identity();
			Map(c => c.Nombre).Length(50);

			Map(c => c.Descripcion).Length(128);

			Map(c => c.Template).Length(int.MaxValue);

            Map(c => c.Folder).Length(int.MaxValue);

            Map(c => c.Scritp).Length(int.MaxValue);

			Map(c => c.CodeSharp).Length(int.MaxValue);

			Map(c => c.Estado).Length(1);

			Map(c => c.Creado);

			Map(c => c.CreadoPor);

			Map(c => c.Modificado);

			Map(c => c.ModificadoPor);

		}
    }

    public class _MapGq_menu : ClassMap<Gq_menu>
    {
        public _MapGq_menu()
        {
        	Table("gq_menu");
        	
			Id(c => c.MenuId).GeneratedBy.Identity();
			Map(c => c.MenuPosition).Length(50);

			Map(c => c.MenuUrl).Length(128);

			Map(c => c.MenuIcono).Length(255);

			Map(c => c.KeyName).Length(50);

			Map(c => c.Nombre).Length(50);

			Map(c => c.MenuPadre).Length(50);

			Map(c => c.Estado).Length(1);

			Map(c => c.Creado);

			Map(c => c.Modificado);

			Map(c => c.CreadoPor);

			Map(c => c.ModificadoPor);

		}
    }

    public class _MapGq_perfiles : ClassMap<Gq_perfiles>
    {
        public _MapGq_perfiles()
        {
        	Table("gq_perfiles");
        	
			Id(c => c.PerfilId).GeneratedBy.Identity();
			Map(c => c.Nombre).Length(50);

			Map(c => c.KeyName).Length(50);

			Map(c => c.Estado).Length(1);

			Map(c => c.Creado);

			Map(c => c.Modificado);

			Map(c => c.CreadoPor);

			Map(c => c.ModificadoPor);

		}
    }

    public class _MapGq_perfiles_accesos : ClassMap<Gq_perfiles_accesos>
    {
        public _MapGq_perfiles_accesos()
        {
        	Table("gq_perfiles_accesos");
        	
			Id(c => c.PerfilesAccesosId).GeneratedBy.Identity();
			Map(c => c.PerfilId);

			Map(c => c.AccesoId);

			Map(c => c.GrantPermition).Length(1);

			Map(c => c.Estado).Length(1);

			Map(c => c.Creado);

			Map(c => c.Modificado);

			Map(c => c.CreadoPor);

			Map(c => c.ModificadoPor);

		}
    }

    public class _MapGq_smtp_config : ClassMap<Gq_smtp_config>
    {
        public _MapGq_smtp_config()
        {
        	Table("gq_smtp_config");
        	
			Id(c => c.Id).GeneratedBy.Identity();
			Map(c => c.Nombre).Length(255);

			Map(c => c.NombreFrom).Length(255);

			Map(c => c.UserName).Length(255);

			Map(c => c.Pass).Length(255);

			Map(c => c.Host).Length(255);

			Map(c => c.Port);

			Map(c => c.UseDefaultCredentials);

			Map(c => c.EnableSsl);

			Map(c => c.EMailFrom).Length(150);

		}
    }

    public class _MapGq_usuarios : ClassMap<Gq_usuarios>
    {
        public _MapGq_usuarios()
        {
        	Table("gq_usuarios");
        	
			Id(c => c.UsuarioId).GeneratedBy.Identity();
			Map(c => c.Usuario).Length(255);

			Map(c => c.Nombre).Length(255);

			Map(c => c.Apellido).Length(255);

			Map(c => c.Email).Length(255);

			Map(c => c.Clave).Length(255);

			Map(c => c.PerfilId);

			Map(c => c.RequiereClave).Length(1);

			Map(c => c.Estado).Length(1);

			Map(c => c.Creado);

			Map(c => c.Modificado);

			Map(c => c.CreadoPor);

			Map(c => c.ModificadoPor);

		}
    }

    public class _MapGq_mailTemplate : ClassMap<Gq_mailTemplate>
    {
        public _MapGq_mailTemplate()
        {
            Table("gq_mailTemplate");

            Id(c => c.Id).GeneratedBy.Identity();

            Map(c => c.Nombre).Length(50);

            Map(c => c.Folder).Length(250);

            Map(c => c.Template).Length(int.MaxValue);

            Map(c => c.CodeSharp).Length(int.MaxValue);

            Map(c => c.Estado).Length(1);

            Map(c => c.Creado);

            Map(c => c.CreadoPor);

            Map(c => c.Modificado);

            Map(c => c.ModificadoPor);

        }
    }

}
