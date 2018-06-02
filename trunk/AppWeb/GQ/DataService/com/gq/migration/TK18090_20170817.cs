using FluentMigrator;
using System;

namespace GQDataService.com.gq.migration
{
    [Migration(1809020170817, "Creacion de tabla MailTemplate")]
    public class TK18090_20170817 : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("Gq_mailTemplate").Row(new
            {
                Nombre = "Clave_modificada",
                Folder = "Clave_modificada",
                Template = "",
                CodeSharp = "",
                Estado = "A",
                Creado = DateTime.Now,
                CreadoPor = 1,
                Modificado = DateTime.Now,
                ModificadoPor = 1,
            });

            Insert.IntoTable("Gq_mailTemplate").Row(new
            {
                Nombre = "Clave_modificadaOK",
                Folder = "Clave_modificadaOK",
                Template = "",
                CodeSharp = "",
                Estado = "A",
                Creado = DateTime.Now,
                CreadoPor = 1,
                Modificado = DateTime.Now,
                ModificadoPor = 1,
            });

            Insert.IntoTable("Gq_mailTemplate").Row(new
            {
                Nombre = "Clave_recuperada",
                Folder = "Clave_recuperada",
                Template = "",
                CodeSharp = "",
                Estado = "A",
                Creado = DateTime.Now,
                CreadoPor = 1,
                Modificado = DateTime.Now,
                ModificadoPor = 1,
            });

            Insert.IntoTable("Gq_mailTemplate").Row(new
            {
                Nombre = "Creacion_usuario",
                Folder = "Creacion_usuario",
                Template = "",
                CodeSharp = "",
                Estado = "A",
                Creado = DateTime.Now,
                CreadoPor = 1,
                Modificado = DateTime.Now,
                ModificadoPor = 1,
            });
        }

        public override void Down()
        {
        }
    }
}
