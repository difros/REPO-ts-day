
using System.Collections.Generic;
using WebNetCore.Data.sql.domain;
using WebNetCore.Data.sql.dto.codegen;

namespace WebNetCore.Data.sql.dto
{
    public class GQ_MenuesDto : _GQ_MenuesDto
    {
        public GQ_MenuesDto():base()
        {
        }
       
        public GQ_MenuesDto(GQ_Menues value):base(value)
        {
        }

        public List<GQ_MenuesDto> Child { get; set; }
    }
}
