using System.Web.Routing;
using Utilities;

namespace BaseController
{
    public class AdminController : BasicController
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Utilities.ResourceLoader.SetCurrentThreadCulture(Session);
            //判断如果不是超级管理员就直接跑错
            if (!((CustomPrincipal)HttpContext.User).UserData.IsSuperAdmin)
            {
                requestContext.HttpContext.Response.Redirect("/Common/RedirectToLogin?Msg=" + General.Resource.Common.Access_Denied, true);
            }
            else
            {

            }
        }

    }
}
