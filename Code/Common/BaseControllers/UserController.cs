using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace BaseController
{
    public class UserController : BasicController
    {
        private string FunctionCode = "";

        public UserController(string functionCode)
        {
            this.FunctionCode = functionCode;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ValicateAccountModule(requestContext);
        }

        /// <summary>
        /// 验证该账户是否有该模块的访问权限
        /// </summary>
        /// <param name="requestContext"></param>
        private void ValicateAccountModule(RequestContext requestContext)
        {
            List<string> nodes = requestContext.HttpContext.Session["Nodes"] as List<string>;
            if (nodes == null)
            {
                HttpContext.Response.Redirect("/Common/RedirectToLogin?Msg=" + General.Resource.Common.Login_in_Time_Out, true);
            }
            if (nodes.Any(i => i.Equals(this.FunctionCode)))
            {

            }
            else
            {
                requestContext.HttpContext.Response.Redirect("/Common/Pending", true);
            }
        }
    }
}