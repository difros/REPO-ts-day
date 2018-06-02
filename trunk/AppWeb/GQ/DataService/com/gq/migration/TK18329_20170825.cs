using FluentMigrator;

namespace GQDataService.com.gq.migrations
{
    [Migration(1832920170825, "Columna IdUsuario en tabla Formularios")]
    public class TK18329_20170825 : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("Gq_formularios").Column("IdUsuario").Exists())
            {
                Alter.Table("Gq_formularios").AddColumn("IdUsuario").AsInt64().Nullable();
            }
        }

        public override void Down()
        {
        }
    }
}
