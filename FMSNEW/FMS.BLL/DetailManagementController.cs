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
    public class DetailManagementController : UserController
    {
        public DetailManagementController()
            : base("DetailRecord")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailRecord()
        {
            ViewData["GUID"] = Guid.NewGuid().ToString();
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="guid">纪录标识</param>
        /// <returns></returns>
        public string DelDetails(string guid)
        {
            bool result = new DetailSvc().DelDetails(guid);
            string msg = string.Empty;
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

        /// <summary>
        /// 更新类别启用状态
        /// </summary>
        /// <param name="guid">纪录标识</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public string UpdDetailstate(string guid, string state)
        {
            bool result = new DetailSvc().UpdDetailstate(guid, state);
            string msg = string.Empty;
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

        /// <summary>
        /// 新增类别
        /// </summary>
        /// <returns></returns>
        public string UpdDetail(string guid,string name)
        {
            T_DetailedCategories detail=new T_DetailedCategories();
            detail.GUID = guid;
            detail.Name = name;
            detail.State = "启用";
            bool result = new DetailSvc().UpdDetail(detail);
            string msg = string.Empty;
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

        /// <summary>
        /// 获取费用列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetDetailList(string rows, string page)
        {
            int count = 0;
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DetailedCategories> IERcord = new List<T_DetailedCategories>();
            IERcord = new DetailSvc().GetDetailList(int.Parse(page), int.Parse(rows), out count);
             strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(IERcord));
             return strJson.ToString();
        }

        public string GetExpenseTypeList()
        {
            int count = 0;
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_ExpenseType> List = new List<T_ExpenseType>();
            List = new DetailSvc().GetExpenseTypeList(Session["CurrentCompanyGuid"].ToString(), out count);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string GetExpenseTypeRecord(string id)
        {
            //string C_GUID = Session["CurrentCompanyGuid"].ToString();
            T_ExpenseType rec = new DetailSvc().GetExpenseTypeRecord(id);
            string json = new JavaScriptSerializer().Serialize(rec);
            return json;
        }

        public string UpdExpenseTypeRecord(string id, string etguid, string expensetype, string expenseflag, string saleflag, string manageflag, string financeflag, string otherFlag, string taxflag)
        {
            bool result = false;
            string msg = string.Empty;
            T_ExpenseType form = new T_ExpenseType();
            form.ET_GUID = etguid;
            form.ExpenseType = expensetype;
            form.ExpenseFlag = expenseflag;
            form.SaleFlag = saleflag;
            form.ManageFlag = manageflag;
            form.FinanceFlag = financeflag;
            form.OtherFlag = otherFlag;
            form.TaxFlag = taxflag;
            result = new DetailSvc().UpdExpenseTypeRecord(form,id);
            if (result)
            {
               
                return "success";
            }
            else
            {
                msg = General.Resource.Common.Failed;
                return "failed";
            }
            //return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //    , result.ToString().ToLower(), msg);
        }

        public string UpExpenseTypeRecord()
        {
           string jsonData = (Request.Form["jsonData"]).ToString();
           List<T_ExpenseType> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T_ExpenseType>>(jsonData);
            string msg = string.Empty;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                //dh.strCmd = "SP_DelAllExpenseTypeRecord";
                //dh.NonQuery();
                if (list.Count > 0)
                {
                    for(int i=0;i<list.Count;i++)
                    {
                       

                        dh.strCmd = "SP_UpdExpenseTypeRecord";
                        //dh.AddPare("@GUID", SqlDbType.NVarChar, 40, list[i].ET_GUID);
                        dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
                        dh.AddPare("@ET_GUID", SqlDbType.NVarChar, 40, list[i].ET_GUID);
                        dh.AddPare("@ExpenseType", SqlDbType.NVarChar, 40, list[i].ExpenseType);
                        dh.AddPare("@ExpenseFlag", SqlDbType.NVarChar, 1, list[i].ExpenseFlag);
                        dh.AddPare("@SaleFlag", SqlDbType.NVarChar, 1, list[i].SaleFlag);
                        dh.AddPare("@ManageFlag", SqlDbType.NVarChar, 1, list[i].ManageFlag);
                        dh.AddPare("@FinanceFlag", SqlDbType.NVarChar, 1, list[i].FinanceFlag);
                        dh.AddPare("@OtherFlag", SqlDbType.NVarChar, 1, list[i].OtherFlag);
                        dh.AddPare("@TaxFlag", SqlDbType.NVarChar, 1, list[i].TaxFlag);

                        dh.NonQuery();
                        dh.CleanPara();
                    }
                }
               
                dh.CommitTran();
                msg = General.Resource.Common.Success;
                return "success";    
            }
            catch
            {
                dh.RollBackTran();
                msg = General.Resource.Common.Failed;
                return "failed";
            }
        }

        public string DelExpenseTypeRecord(string id)
        {
            bool result = new DetailSvc().DelExpenseTypeRecord(id);
            string msg = string.Empty;
            if (result)
            {
                return "success";
            }
            else
            {
                return "failed";
              
            }
           
        }
    }
}
