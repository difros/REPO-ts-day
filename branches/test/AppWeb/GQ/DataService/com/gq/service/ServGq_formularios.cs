
using GQDataService.com.gq.service.codegen;
using NHibernate;

namespace GQDataService.com.gq.service
{
    public class ServGq_formularios : _ServGq_formularios
    {
    	#region Constructores

        public ServGq_formularios(ISession session): base(session){}
        public ServGq_formularios(IStatelessSession statelessSession): base(statelessSession){}
        public ServGq_formularios(ISession session, IStatelessSession statelessSession): base(session,statelessSession){}

        #endregion
    }
}
