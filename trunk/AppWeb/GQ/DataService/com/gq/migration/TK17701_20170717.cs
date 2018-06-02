using FluentMigrator;

namespace GQDataService.com.gq.migrations
{
    [Migration(1770120170717, "Renombrar columna Nombre de Gq_smtp_config")]
    public class TK17701_20170717 : Migration
    {
        public override void Up()
        {   
            Rename.Column("User").OnTable("Gq_smtp_config").To("UserName");
        }

        public override void Down()
        {
        }
    }
}
