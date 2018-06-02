using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GQDataService.com.gq.migration
{
    [Migration(1832920170822, "Creacion de tabla para representar archivos")]
    public class TK18329_20170822 : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            if (!Schema.Table("GQ_Archivos").Exists())
            {
                Create.Table("GQ_Archivos")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Nombre").AsString(255).Nullable()
                .WithColumn("NombreOriginal").AsString(255).Nullable()
                .WithColumn("Extension").AsString(4).Nullable()
                .WithColumn("Tipo").AsInt16().Nullable()
                .WithColumn("Estado").AsString(1).WithDefaultValue("A")
                .WithColumn("Creado").AsDateTime().Nullable()
                .WithColumn("Modificado").AsDateTime().Nullable()
                .WithColumn("CreadoPor").AsInt64().Nullable()
                .WithColumn("ModificadoPor").AsInt64().Nullable();
            }
        }
    }
}
