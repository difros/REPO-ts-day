using FluentMigrator;
using System;

namespace GQDataService.com.gq.migrations
{
    [Migration(1834120170824, "Gráfico Área ejemplo")]
    public class TK18341_20170824 : Migration
    {
        public override void Up()
        {           
            Insert.IntoTable("Gq_grafico").Row(new
            {
                Nombre = "Gráfico Área",
                Descripcion = "Gráfico Área MEM Nodo Ejemplo",
                Folder = "equilibrioNodo",
                Template = "",
                Scritp = "",
                CodeSharp = "",
                Estado = "A",
                Creado = DateTime.Now,
                CreadoPor = "1",
                Modificado = DateTime.Now,
                ModificadoPor = 1,
            });
        }

        public override void Down()
        {
        }
    }
}
