using FluentMigrator;
using System;

namespace GQDataService.com.gq.migrations
{
    [Migration(1834120170825, "Gráfico Área ejemplo")]
    public class TK18341_20170825 : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("Gq_grafico_valores").Exists())
            {
                Create.Table("Gq_grafico_valores")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Filtro").AsString(255).Nullable()
                .WithColumn("ID_Fecha").AsInt16()
                .WithColumn("Concepto").AsString(255).Nullable()
                .WithColumn("Valor").AsDecimal();
            }

            Insert.IntoTable("Gq_grafico_valores").Row(new
            {
                Filtro = "FiltroX",
                ID_Fecha = 1,
                Concepto = "ConceptoX",
                Valor = 144.907                
            });

            Insert.IntoTable("Gq_grafico_valores").Row(new
            {
                Filtro = "FiltroX",
                ID_Fecha = 2,
                Concepto = "ConceptoX",
                Valor = 145.824
            });

            Insert.IntoTable("Gq_grafico_valores").Row(new
            {
                Filtro = "FiltroX",
                ID_Fecha = 3,
                Concepto = "ConceptoX",
                Valor = 144.466
            });

            Insert.IntoTable("Gq_grafico_valores").Row(new
            {
                Filtro = "FiltroX",
                ID_Fecha = 4,
                Concepto = "ConceptoX",
                Valor = 144.638
            });


            Insert.IntoTable("Gq_grafico_valores").Row(new
            {
                Filtro = "FiltroX",
                ID_Fecha = 1,
                Concepto = "ConceptoY",
                Valor = 142.974
            });

            Insert.IntoTable("Gq_grafico_valores").Row(new
            {
                Filtro = "FiltroX",
                ID_Fecha = 2,
                Concepto = "ConceptoY",
                Valor = 139.096
            });

            Insert.IntoTable("Gq_grafico_valores").Row(new
            {
                Filtro = "FiltroX",
                ID_Fecha = 3,
                Concepto = "ConceptoY",
                Valor = 139.297
            });

            Insert.IntoTable("Gq_grafico_valores").Row(new
            {
                Filtro = "FiltroX",
                ID_Fecha = 4,
                Concepto = "ConceptoY",
                Valor = 139.053
            });


            Update.Table("Gq_grafico").Set(new
            {
                Folder = "graficoEjemplo"
            }).Where(new
            {
                Folder = "equilibrioNodo"
            });

        }

        public override void Down()
        {
        }
    }
}
