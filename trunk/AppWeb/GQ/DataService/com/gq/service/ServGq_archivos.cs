
using GQDataService.com.gq.service.codegen;
using NHibernate;

namespace GQDataService.com.gq.service
{
    public class ServGq_archivos : _ServGq_archivos
    {
    	#region Constructores

        public ServGq_archivos(ISession session): base(session){}
        public ServGq_archivos(IStatelessSession statelessSession): base(statelessSession){}
        public ServGq_archivos(ISession session, IStatelessSession statelessSession): base(session,statelessSession){}

        #endregion
    }
}
