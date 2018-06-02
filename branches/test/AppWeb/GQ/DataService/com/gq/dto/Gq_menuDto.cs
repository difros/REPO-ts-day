using System.Collections.Generic;
using GQDataService.com.gq.domain;
using GQDataService.com.gq.dto.codegen;

namespace GQDataService.com.gq.dto
{
    public class Gq_menuDto : _Gq_menuDto
    {
        public Gq_menuDto():base()
        {
        }
       
        public Gq_menuDto(Gq_menu value):base(value)
        {
        }

        public List<Gq_menuDto> Child { get; set; } = new List<Gq_menuDto>();
    }
}
