using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace BaseController
{
    public class UserController : BasicController
    {
        private string FunctionCode = "";
        
        protected string CompanyId()
        {
            return Session["CurrentCompanyGuid"].ToString();
        }

        public UserController(string functionCode)
        {
            this.FunctionCode = functionCode;
        }

        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            //filterContext.ActionDescriptor.ActionName
            //filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
            base.OnActionExecuting(filterContext);
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
            if (Session["GroupCode"] == null || Session["GroupCode"].ToString() != "GP001")
            {
                List<string> list = requestContext.HttpContext.Session["ModuleList"] as List<string>;
                if (list == null)
                {
                    requestContext.HttpContext.Response.Redirect("/Common/Login", true);
                    return;
                }
                if (list!=null && list.Any(i => i.Equals(this.FunctionCode)))
                {
                    return;
                }
               
            }
            if (Session["GroupCode"].ToString() == "GP010")
            {
                requestContext.HttpContext.Response.Redirect("/Common/Pending", true);
                return;
            }
            
            //List<string> nodes = requestContext.HttpContext.Session["Nodes"] as List<string>;
            //if (nodes == null)
            //{
            //    HttpContext.Response.Redirect("/Common/RedirectToLogin?Msg=" + General.Resource.Common.Login_in_Time_Out, true);
            //}
            //if (nodes.Any(i => i.Equals(this.FunctionCode)))
            //{
                
            //}
            //else
            //{
            //    requestContext.HttpContext.Response.Redirect("/Common/Pending", true);
            //}
        }
    }
}