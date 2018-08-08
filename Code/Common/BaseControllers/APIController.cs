using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common.BaseControllers
{
    public class APIController : Controller
    {
        protected string SubSystemID = Convert.ToString(ConfigurationManager.AppSettings["SubSystemID"]);
        protected string AccountID = Convert.ToString(ConfigurationManager.AppSettings["AccountID"]);
        protected string SystemName = Convert.ToString(ConfigurationManager.AppSettings["SystemName"]);
        protected string RMUrl = string.Empty;
        protected bool SSO = bool.Parse(ConfigurationManager.AppSettings["SSO"]);

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
    }
}
