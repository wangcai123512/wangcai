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
    /// 费用核销
    /// </summary>
    public class ExpenditureWriteOffController : UserController
    {
        public ExpenditureWriteOffController()
            : base("ExpenditureWriteOff")
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
        /// 获取应付数据
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetPayablesList(string rows, string page)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_Payables> Payables = new List<T_Payables>();
            Payables = new WriteOffSvc().GetPayablesList(C_GUID, int.Parse(page), int.Parse(rows), out count);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(Payables));
            return strJson.ToString();
        }

        /// <summary>
        /// 获取已销数据
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
            List<T_IEWriteOff> Payables = new List<T_IEWriteOff>();
            Payables = new WriteOffSvc().GetIEWriteOffList(C_GUID, int.Parse(page), int.Parse(rows), out count,"E");
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(Payables));
            return strJson.ToString();
        }

        /// <summary>
        /// 应付数据页
        /// </summary>
        /// <param name="id">应付数据标识</param>
        /// <returns></returns>
        public ActionResult PayablesRecord(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return View(new WriteOffSvc().GetRecord(id, C_GUID,"E"));
        }

        /// <summary>
        /// 更新应付记录
        /// </summary>
        /// <param name="rec">应付记录对象</param>
        /// <returns></returns>
        public string UpdPayablesRecord(T_Receivables rec)
        {
            bool result = false;
            string msg = string.Empty;
            rec.IE_Flag = "E";
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
