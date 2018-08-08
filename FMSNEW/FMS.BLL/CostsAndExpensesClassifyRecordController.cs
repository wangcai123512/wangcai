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
    /// 查询付款
    /// </summary>
    public class CostsAndExpensesClassifyRecordController : UserController
    {
        public CostsAndExpensesClassifyRecordController()
            : base("CostsAndExpensesClassifyRecord")
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
        /// 获取一级费用科目分类的成本与费用汇总行
        /// </summary>
        /// <returns></returns>
        public string GetOnceClassifyTotalCollectList(string InvType, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetOnceClassifyTotalCollectList(InvType,dateBegin,dateEnd, C_GUID, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 获取一级费用科目分类的成本与费用列表
        /// </summary>
        /// <returns></returns>
        public string GetOnceClassifyTotalList(string InvType, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetOnceClassifyTotalList(InvType, dateBegin, dateEnd, C_GUID, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 获取一级费用科目分类的成本与费用比较
        /// </summary>
        /// <returns></returns>
        /// 

        public string GetOnceClassifyCompareList(string InvType,string dateBegin, string dateEnd)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetOnceClassifyCompareList(InvType,C_GUID, dateBegin, dateEnd);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 获取二级费用科目分类的成本与费用汇总行
        /// </summary>
        /// <returns></returns>
        public string GetSecondClassifyTotalCollectList(string IEGroup, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetSecondClassifyTotalCollectList(IEGroup, dateBegin, dateEnd,C_GUID, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 获取二级费用科目分类的成本与费用列表
        /// </summary>
        /// <returns></returns>
        public string GetSecondClassifyTotalList(string IEGroup, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetSecondClassifyTotalList(IEGroup, dateBegin, dateEnd, C_GUID, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 获取二级费用科目分类的成本与费用比较
        /// </summary>
        /// <returns></returns>
        /// 
        public string GetSecondClassifyCompareList(string IEGroup, string dateBegin, string dateEnd)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetSecondClassifyCompareList(IEGroup, C_GUID, dateBegin, dateEnd);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 获取一级分类下面二级费用科目分类的成本与费用汇总行
        /// </summary>
        /// <returns></returns>
        public string GetOnceSonClassifyTotalCollectList(string InvType, string IEGroup, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetOnceSonClassifyTotalCollectList(InvType, IEGroup, dateBegin, dateEnd, C_GUID, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 获取一级分类下面二级费用科目分类的成本与费用列表
        /// </summary>
        /// <returns></returns>
        public string GetOnceSonClassifyTotalList(string InvType, string IEGroup, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetOnceSonClassifyTotalList(InvType, IEGroup, dateBegin, dateEnd, C_GUID, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 获取二级费用科目分类的成本与费用比较
        /// </summary>
        /// <returns></returns>
        /// 
        public string GetOnceSonClassifyCompareList(string InvType ,string IEGroup, string dateBegin, string dateEnd)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetOnceSonClassifyCompareList(InvType, IEGroup, C_GUID, dateBegin, dateEnd);
            return new JavaScriptSerializer().Serialize(Record);
        } 
    }
}
