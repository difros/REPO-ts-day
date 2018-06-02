using System.Collections.Generic;

namespace GQ.Data.dto
{
    public interface IGenericDto
    {
        IEnumerable<IGenericDto> SetEntity(IEnumerable<IEntity> values);
        IGenericDto SetEntity(IEntity value);
        IEnumerable<IEntity> GetEntity(IEnumerable<IGenericDto> values);
        IEntity GetEntity();
    }
}
