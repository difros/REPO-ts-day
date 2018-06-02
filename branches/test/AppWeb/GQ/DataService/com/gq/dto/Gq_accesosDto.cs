using GQDataService.com.gq.domain;
using GQDataService.com.gq.dto.codegen;
using GQService.com.gq.security;

namespace GQDataService.com.gq.dto
{
    public class Gq_accesosDto : _Gq_accesosDto
    {
        public SecurityDescription extra;

        public Gq_accesosDto():base()
        {
        }
       
        public Gq_accesosDto(Gq_accesos value):base(value)
        {
        }
    }
}
