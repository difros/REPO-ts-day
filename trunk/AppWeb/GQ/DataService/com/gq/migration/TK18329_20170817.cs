using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GQDataService.com.gq.migration
{
    [Migration(1832920170817, "Creacion de tabla para la vista de Formulario")]
    public class TK18329_20170817 : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("GQ_Formularios").Exists())
            {
                Create.Table("GQ_Formularios")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Texto").AsString(255).Nullable()
                .WithColumn("Opcion").AsString(255).Nullable()
                .WithColumn("Fecha").AsDateTime().Nullable()
                .WithColumn("Hora").AsDateTime().Nullable()
                .WithColumn("IdArchivo").AsInt64()
                .WithColumn("CheckBox").AsBoolean()
                .WithColumn("Estado").AsString(1).WithDefaultValue("A")
                .WithColumn("Creado").AsDateTime().Nullable()
                .WithColumn("Modificado").AsDateTime().Nullable()
                .WithColumn("CreadoPor").AsInt64().Nullable()
                .WithColumn("ModificadoPor").AsInt64().Nullable();
            }
        }

        public override void Down()
        {
        }
    }
}
