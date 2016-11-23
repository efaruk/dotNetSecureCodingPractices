using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SCP.Web
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ScpAuthorizeAttribute: AuthorizeAttribute
    {


        public override void OnAuthorization(HttpActionContext actionContext)
        {

            base.OnAuthorization(actionContext);
        }
    }
}