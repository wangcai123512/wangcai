
using System;
using System.Web;
using System.Web.Mvc;

namespace Utilities
{
    /// <summary>
    /// 错误日志
    /// Controller发生异常时会执行这里
    /// </summary>
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            Exception Error = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            string msg = Error.Message.Replace("\r\n", string.Empty);
            var CusEx = new
            {
                Message = msg,
                Url = HttpContext.Current.Request.RawUrl,
                Method = Error.TargetSite.Name,
                //RowNum = Error.StackTrace.Substring(Error.StackTrace.IndexOf("行号"), 10)
            };
            filterContext.Result = new RedirectResult("/Exception/CatchException/"
                 + CusEx.Method);
        }
    }
}
