
using GQ.Security;
using WebNetCore.Data.sql.domain;
using WebNetCore.Data.sql.dto.codegen;

namespace WebNetCore.Data.sql.dto
{
    public class GQ_AccesosDto : _GQ_AccesosDto
    {
        public GQ_AccesosDto():base()
        {
        }
       
        public GQ_AccesosDto(GQ_Accesos value):base(value)
        {
        }

        public SecurityDescription extra { get; set; }
    }
}
