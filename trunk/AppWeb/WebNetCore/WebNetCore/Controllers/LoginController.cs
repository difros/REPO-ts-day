using GQ.Security;
using GQ.Security.MCV.controller;

namespace WebNetCore.Controllers
{
    [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
    public class LoginController : BaseController
    {
        [SecurityDescription(SecurityDescription.SeguridadEstado.Desactivo)]
        public bool Login(string userName, string password)
        {
            return true;
        }
    }
}
