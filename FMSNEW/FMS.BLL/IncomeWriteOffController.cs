using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.Model;
using FMS.DAL;
using System.Web.Script.Serialization;

namespace FMS.BLL
{
    /// <summary>
    /// 核销收入
    /// </summary>
    public class IncomeWriteOffController:UserController
    {
        public IncomeWriteOffController()
            : base("IncomeWriteOff")
        {}

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取应收账款列表数据
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetReceivablesList(string rows, string page)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_Receivables> Receivables = new List<T_Receivables>();
            Receivables = new WriteOffSvc().GetReceivablesList(C_GUID, int.Parse(page), int.Parse(rows), out count);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(Receivables));
            return strJson.ToString();
        }

        /// <summary>
        /// 获取已销列表数据
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetIEWriteOffList(string rows, string page)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_IEWriteOff> Receivables = new List<T_IEWriteOff>();
            Receivables = new WriteOffSvc().GetIEWriteOffList(C_GUID, int.Parse(page), int.Parse(rows), out count,"I");
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(Receivables));
            return strJson.ToString();
        }

        /// <summary>
        /// 应收信息页
        /// </summary>
        /// <param name="id">应收纪录标识</param>
        /// <returns></returns>
        public ActionResult ReceivablesRecord(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return View(new WriteOffSvc().GetRecord(id, C_GUID,"I"));
        }

        /// <summary>
        /// 更新应收纪录
        /// </summary>
        /// <param name="rec">应收纪录对象</param>
        /// <returns></returns>
        public string UpdReceivablesRecord(T_Receivables rec)
        {
            bool result = false;
            string msg = string.Empty;
            rec.IE_Flag = "I";
            rec.Creator = base.userData.LoginFullName;
            rec.C_GUID = Session["CurrentCompanyGuid"].ToString();
            DateTime now = DateTime.Now;
            if (rec.Date <= now)
            {
                result = new WriteOffSvc().UpdWriteOffRecord(rec);
                if (result)
                {
                    msg = General.Resource.Common.Success;
                }
                else
                {
                    msg = General.Resource.Common.Failed;
                }
            }
            else
            {
                 result = false;
                msg = FMS.Resource.Finance.Finance.DateError;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
               , result.ToString().ToLower(), msg);
        }
    }
}
