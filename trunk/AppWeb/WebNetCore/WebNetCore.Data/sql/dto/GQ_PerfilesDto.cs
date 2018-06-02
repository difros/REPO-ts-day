
using System.Collections.Generic;
using WebNetCore.Data.sql.domain;
using WebNetCore.Data.sql.dto.codegen;
using GQ.Sql.MySQL;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebNetCore.Data.sql.dto
{
    public class GQ_PerfilesDto : _GQ_PerfilesDto
    {
        public GQ_PerfilesDto() : base()
        {
        }

        public GQ_PerfilesDto(GQ_Perfiles value) : base(value)
        {
        }

        public override GQ_PerfilesDto SetEntity(GQ_Perfiles value)
        {
            var dto = base.SetEntity(value);

            dto.PerfilesAccesos = (List<GQ_Perfiles_AccesosDto>)new GQ_Perfiles_AccesosDto().SetEntity(MySQLService.Instance.GetSession<GQ_Perfiles_Accesos>().findBy(x => x.PerfilId == dto.Id).AsNoTracking().ToList<GQ_Perfiles_Accesos>());

            return dto;
        }

        public List<GQ_Perfiles_AccesosDto> PerfilesAccesos { get; set; }
    }
}
