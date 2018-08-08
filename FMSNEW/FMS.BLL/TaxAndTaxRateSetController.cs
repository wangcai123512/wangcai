using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Text;
using System.Web;
using System.IO;
using System.Data;

namespace FMS.BLL
{
    /// <summary>
    /// 记录费用
    /// </summary>
    public class TaxAndTaxRateSetController : UserController
    {
        public TaxAndTaxRateSetController()
            : base("TaxAndTaxRateSet")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["T_GUID"] = Guid.NewGuid().ToString();
            return View();
        }


        /// <summary>
        /// 获取税种列表
        /// </summary>
        /// <returns></returns>
        public string GetTaxAndTaxRateList()
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            
            StringBuilder strJson = new StringBuilder();
            List<T_Tax> List = new List<T_Tax>();
            List = new TaxSvc().GetTax(C_GUID);
            string json = new JavaScriptSerializer().Serialize(List);

            return json;
        }

        /// <summary>
        /// 修改税种和税率
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string UpdTaxAndTaxRate()
        {
            string jsonData = (Request.Form["jsonData"]).ToString();
            List<T_Tax> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T_Tax>>(jsonData);
            string msg = string.Empty;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_DelAllTax";
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
                dh.NonQuery();
				dh.CleanPara();
                if (list.Count > 0)
                {
                    for(int i=0;i<list.Count;i++)
                    {
                        dh.strCmd = "SP_UpdTax";
                        dh.AddPare("@T_GUID", SqlDbType.NVarChar, 40, list[i].T_GUID);
                        dh.AddPare("@Type", SqlDbType.NVarChar, 40, list[i].Type);
                        dh.AddPare("@Name", SqlDbType.NVarChar, 40, list[i].Name);
                        dh.AddPare("@Rate", SqlDbType.Decimal, 0, list[i].Rate);
                        dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, list[i].C_GUID);
                        dh.NonQuery();
                        dh.CleanPara();
                    }
                }
               
                dh.CommitTran();
                msg = General.Resource.Common.Success;
                return msg;    
            }
            catch
            {
                dh.RollBackTran();
                msg = General.Resource.Common.Failed;
                return msg;
            }
        }

        /// <summary>
        /// 删除税种
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string DelTax(string Tguid)
        {
            bool result = false;
            string msg = string.Empty;
            //C_GUID = Session["CurrentCompanyGuid"].ToString();
            result = new TaxSvc().DelTax(Tguid);
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }
    }
}
