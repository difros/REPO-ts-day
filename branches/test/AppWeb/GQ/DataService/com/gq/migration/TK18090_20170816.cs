using FluentMigrator;

namespace GQDataService.com.gq.migration
{
    [Migration(1809020170816, "Creacion de tabla MailTemplate")]
    public class TK18090_20170816 : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("Gq_mailTemplate").Exists())
            {
                Create.Table("Gq_mailTemplate")
                    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                    .WithColumn("Nombre").AsString(50).NotNullable()
                    .WithColumn("Folder").AsString(250).Nullable().Unique()
                    .WithColumn("Template").AsString(int.MaxValue).NotNullable()
                    .WithColumn("CodeSharp").AsString(int.MaxValue).NotNullable()
                    .WithColumn("Estado").AsString(1).NotNullable()
                    .WithColumn("Creado").AsDateTime()
                    .WithColumn("CreadoPor").AsInt64()
                    .WithColumn("Modificado").AsDateTime().Nullable()
                    .WithColumn("ModificadoPor").AsInt64().Nullable();
            }
        }

        public override void Down()
        {
        }
    }
}
