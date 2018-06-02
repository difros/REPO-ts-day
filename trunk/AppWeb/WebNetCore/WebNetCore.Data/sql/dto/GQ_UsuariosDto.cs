using GQ.Sql.MySQL;
using WebNetCore.Data.sql.domain;
using WebNetCore.Data.sql.dto.codegen;
using System.Collections.Generic;
using System.Linq;

namespace WebNetCore.Data.sql.dto
{
    public class GQ_UsuariosDto : _GQ_UsuariosDto
    {
        public GQ_UsuariosDto() : base()
        {
        }

        public GQ_UsuariosDto(GQ_Usuarios value) : base(value)
        {
        }

        public override GQ_UsuariosDto SetEntity(GQ_Usuarios value)
        {
            var dto = base.SetEntity(value);

            dto.Perfiles = MySQLService.Instance.GetSession<GQ_Usuarios_Perfiles>().findBy(x => x.UsuarioId == dto.Id.Value).Select(x => (long?)x.PerfilId).ToList();

            return dto;
        }

        public string token { get; set; }
        public List<long?> Perfiles { get; set; }
        public string PasswordChequed { get; set; }
    }
}
