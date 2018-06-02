using FluentMigrator;
using System;

namespace GQDataService.com.gq.migrations
{
    [Migration(1846820170905, "Gq_xylineas")]
    public class TK18468_20170905 : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("Gq_xylineas").Exists())
            {
                Create.Table("Gq_xylineas")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Linea").AsString(30).Nullable()
                .WithColumn("X1").AsString(15).Nullable()
                .WithColumn("Y1").AsString(15).Nullable()
                .WithColumn("X2").AsString(15).Nullable()
                .WithColumn("Y2").AsString(15).Nullable()
                .WithColumn("Tipo").AsString(15).Nullable()
                .WithColumn("NI").AsString(15).Nullable()
                .WithColumn("NF").AsString(15).Nullable()
                .WithColumn("Long1").AsString(15).Nullable()
                .WithColumn("Lat1").AsString(15).Nullable()
                .WithColumn("Long2").AsString(15).Nullable()
                .WithColumn("Lat2").AsString(15).Nullable();
            }

            Insert.IntoTable("Gq_xylineas").Row(new
            {
                Linea = "Codoba-Corrientes",
                X1 = -58.830634,
                Y1 = -27.4692131,
                X2 = -64.1887760,
                Y2 = -31.42008329,
                Tipo = "Vinculo",
                NI = "Corrientes",
                NF = "Cordoba",
                Long1 = -58.830634,
                Lat1 = -27.4692131,
                Long2 = -64.1887760,
                Lat2 = -31.42008329
            });

            Insert.IntoTable("Gq_xylineas").Row(new
            {
                Linea = "Cordoba-Mendoza",
                X1 = -64.1887760,
                Y1 = -31.42008329,
                X2 = -68.84583859,
                Y2 = -32.8894587,
                Tipo = "Vinculo",
                NI = "Cordoba",
                NF = "Mendoza",
                Long1 = -64.1887760,
                Lat1 = -31.42008329,
                Long2 = -68.84583859,
                Lat2 = -32.8894587
            });

        }

        public override void Down()
        {
        }
    }
}
