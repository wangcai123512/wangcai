using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Text;
using System.Web;
using System.IO;
using Aspose.Cells;
using Common.Models;
using Newtonsoft.Json;


namespace FMS.BLL
{
    /// <summary>
    //供应商的成本费用查询
    /// </summary>
    public class CostsAndExpensesSupplierRecordController : UserController
    {
        public CostsAndExpensesSupplierRecordController()
            : base("CostsAndExpensesSupplierRecord")
        { }
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //获取当前统计货币
            ViewData["Code"] = (new CompanySvc().GetCompanyCurrceny(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault()).Code;
            //获取公司全称
            ViewData["ChineseFullName"] = (new CompanySvc().GetCompanyInformation(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault()).ChineseFullName;
            return View();
        }
        /// <summary>
        /// 查询供应商成本与费用汇总
        /// </summary>
        /// <returns></returns>
        public string GetSupplierTotalCollectList(int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetSupplierTotalCollectList(C_GUID, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 查询供应商成本与费用列表
        /// </summary>
        /// <returns></returns>
        public string GetSupplierTotalList(string RPers, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string[] RPerSA = RPers.Split(',');
            List<T_IERecord> RecordCount = new List<T_IERecord>();
            for (int i = 0; i < RPerSA.Length; i++)
            {
                string RPer = RPerSA[i].ToString();
                List<T_IERecord> Record = new List<T_IERecord>();
                Record = new IESvc().GetSupplierTotalList(RPer, C_GUID, dateBegin, dateEnd,pageIndex, -1, out count);
                if (Record.Count > 0)
                {
                    for (int a = 0; a < Record.Count; a++)
                    {
                        RecordCount.Add(Record[a]);
                    }
                }
            }
            return new JavaScriptSerializer().Serialize(RecordCount);
        }
    }
}
