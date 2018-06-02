using FluentMigrator;

namespace GQDataService.com.gq.migrations
{
    [Migration(1834120170823, "Columna Folder en tabla Graficos")]
    public class TK18341_20170823 : Migration
    {
        public override void Up()
        {
            if (!Schema.Table("Gq_grafico").Column("Folder").Exists())
            {
                Alter.Table("Gq_grafico").AddColumn("Folder").AsString(250).Nullable().Unique();
            }
        }

        public override void Down()
        {
        }
    }
}
