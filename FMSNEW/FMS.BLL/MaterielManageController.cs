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
    /// 
    /// </summary>
    public class MaterielManageController : UserController
    {
        public MaterielManageController()
            : base("MaterielManage")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterielManage()
        {
            //ViewData["GUID"] = Guid.NewGuid().ToString();
            return View();
        }
        public ActionResult Index() {
            return View();
        }
        /// <summary>
        /// 物料类别页
        /// </summary>
        public ActionResult PurchasingTypeRecord()
        {
            return View();
        }
        /// <summary>
        /// 类别表格数据
        /// </summary>
        /// <returns></returns>
        public string GetMaterielManageList(int pageSize, string AID_FLAG, int pageIndex = 1)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_PTR> Record = new List<T_PTR>();
            Record = new AIDSvc().GetMaterielManageList(pageIndex, -1, out count, C_GUID, AID_FLAG);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 更新父类别前查询
        /// </summary>
        /// <param name="partner">更新父类别前查询</param>
        /// <returns></returns>
        public string GetUpdPurchasingTypeRecord(T_PTR ptr)
        {
            string rs = "";
            int count = 0;
            ptr.C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_AIDRecord> Record = new List<T_AIDRecord>();
            Record = new AIDSvc().GetUpdPurchasingTypeRecord(out count, ptr.AidTypeName, ptr.AID_FLAG, ptr.C_GUID);
            if (Record.Count == 0)
            {
                rs = "true";
            }
            else
            {
                rs = "false";
            }
            return rs;

            //return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //    , result.ToString().ToLower(), msg);
        }
        /// <summary>
        /// 更新父类别
        /// </summary>
        /// <param name="partner">更新父类别</param>
        /// <returns></returns>
        public string UpdPurchasingTypeRecord(T_PTR ptr)
        {
            string rs = "";
            ptr.C_GUID = Session["CurrentCompanyGuid"].ToString();
            if (string.IsNullOrEmpty(ptr.AT_GUID))
            {
                ptr.AT_GUID = Guid.NewGuid().ToString();
            }

            bool result = new AIDSvc().UpdPurchasingTypeRecord(ptr);
            if (result)
            {
                rs = "true";
            }
            else
            {
                rs = "false";
            }
            return rs;

            //return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //    , result.ToString().ToLower(), msg);
        }
        /// <summary>
        /// 更新子类别前查询
        /// </summary>
        /// <param name="partner">更新父类别前查询</param>
        /// <returns></returns>
        public string GetUpdMaterielManage(T_PTR ptr)
        {
            string rs = "";
            int count = 0;
            ptr.C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_AIDRecord> Record = new List<T_AIDRecord>();
            Record = new AIDSvc().GetUpdMaterielManage(out count, ptr.MM_FLAG, ptr.C_GUID, ptr.MM_Name);
            if (Record.Count == 0)
            {
                rs = "true";
            }
            else
            {
                rs = "false";
            }
            return rs;

            //return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //    , result.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 更新子类别
        /// </summary>
        /// <param name="partner">更新子类别</param>
        /// <returns></returns>
        public string UpdPurchasingSubTypeRecord(T_PTR ptr)
        {
            string rs = "";

            ptr.C_GUID = Session["CurrentCompanyGuid"].ToString();
            if (string.IsNullOrEmpty(ptr.AST_GUID))
            {
                ptr.AST_GUID = Guid.NewGuid().ToString();
            }

            bool result = new AIDSvc().UpdPurchasingSubTypeRecord(ptr);
            if (result)
            {
                rs = "true";
            }
            else
            {
                rs = "false";
            }
            return rs;

            //return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //    , result.ToString().ToLower(), msg);
        }
        /// <summary>
        /// 更新物料
        /// </summary>
        /// <param name="partner">更新物料</param>
        /// <returns></returns>
        public string UpdMaterielManage(T_PTR ptr)
        {
            string rs = "";

            ptr.C_GUID = Session["CurrentCompanyGuid"].ToString();
            if (string.IsNullOrEmpty(ptr.MM_GUID))
            {
                ptr.MM_GUID = Guid.NewGuid().ToString();
            }

            bool result = new AIDSvc().UpdMaterielManage(ptr);
            if (result)
            {
                rs = "true";
            }
            else
            {
                rs = "false";
            }
            return rs;

            //return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //    , result.ToString().ToLower(), msg);
        }
        /// <summary>
        /// 删除类别前查询
        /// </summary>
        /// <param name="partner">删除类别</param>
        /// <returns></returns>
        public string GetDelMaterielManage(T_PTR ptr)
        {
            string rs = "";
            int count = 0;
            ptr.C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_AIDRecord> Record = new List<T_AIDRecord>();
            Record = new AIDSvc().GetDelMaterielManage(out count, ptr.C_GUID, ptr.MM_GUID,ptr.MM_FLAG);
            if (Record.Count == 0)
            {
                rs = "true";
            }
            else
            {
                rs = "false";
            }
            return rs;

            //return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //    , result.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 删除物料
        /// </summary>
        /// <param name="partner">删除物料</param>
        /// <returns></returns>
        public string DelMaterielManage(T_PTR ptr)
        {
            string rs = "";
            ptr.C_GUID = Session["CurrentCompanyGuid"].ToString();
            bool result = new AIDSvc().DelMaterielManage(ptr);
            if (result)
            {
                rs = "true";
            }
            else
            {
                rs = "false";
            }
            return rs;

            //return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //    , result.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 获取父类
        /// </summary>
        /// <returns></returns>
        public string GetSubAidType(T_PTR ptr)
        {
            ptr.C_GUID = Session["CurrentCompanyGuid"].ToString();

            var subTypeList = new AIDSvc().GetSubAidType(ptr);

            var strJson = ConvertToSelectJson(subTypeList, "ASTTypeName", "AST_GUID");

            return strJson.ToString();
        }

        ///// <summary>
        ///// 获取子类
        ///// </summary>
        ///// <returns></returns>
        public string GetSonAidType(string parentId)
        {
            var subTypeList = new AIDSvc().GetSonAidType(parentId); 

            var strJson = ConvertToSelectJson(subTypeList, "ASTTypeName", "AST_GUID");

            return strJson.ToString();
        }


        ///// <summary>
        ///// 通过获取子类得到折旧周期
        ///// </summary>
        ///// <returns></returns>
        public string GetPeriodAidType(string parentId)
        {
            //var subTypeList = new AIDSvc().GetPeriodAidType(parentId);

            //var strJson = ConvertToSelectJson(subTypeList, "Depreciation_year", "Depreciation_year");

            //return strJson.ToString();


            List<T_PTR> Record = new List<T_PTR>();
            Record = new AIDSvc().GetPeriodAidType(parentId);
            return new JavaScriptSerializer().Serialize(Record);

        }



        ///// <summary>
        ///// 获取资产分类
        ///// </summary>
        ///// <returns></returns>
        public string GetAssetsAidType(string assetType)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            var subTypeList = new AIDSvc().GetAssetsAidType(assetType, C_GUID);

            var strJson = ConvertToSelectJson(subTypeList, "AidTypeName", "AT_GUID");

            return strJson.ToString();
        }
        ///// <summary>
        ///// 获取bom结构
        ///// </summary>
        ///// <returns></returns>
        public string GetProductBom(string subId)
        {
            ExceResult rs = new ExceResult();
            DataTable bom = AIDSvc.GetProductBomList(subId);
            List<ProductNode> treeNode = new FMS.BLL.ProductManageController().GetProductNoeList(bom);
            if (treeNode != null)
            {
                rs.success = true;
                rs.msg = JsonConvert.SerializeObject(treeNode);
            }
            else
            {
                rs.success = false;
                rs.msg = "该产品未设置结构，请先设置结构。";
            }
            return JsonConvert.SerializeObject(rs);
        }
        ///// <summary>
        ///// 通过类别子类获取父类Id
        ///// </summary>
        ///// <returns></returns>
        public string GetParentnodesId(string nodes)
        {
            List<T_PTR> Record = new List<T_PTR>();
            Record = new AIDSvc().GetParentnodesId(nodes);
            return new JavaScriptSerializer().Serialize(Record);
        }
        ///// <summary>
        ///// 更新物料数量
        ///// </summary>
        ///// <returns></returns>
        public string EditProductBomRecord(T_ProductBom form)
        {
            ExceResult rs = new ExceResult();
            bool result = false;
            string msg = string.Empty;
            result = new AIDSvc().UpdProductBomRecord(form);
            if (result)
            {
                rs.success = true;
                rs.msg = "修改成功";
            }
            else
            {
                rs.success = true;
                rs.msg = "修改失败";
            }
            return JsonConvert.SerializeObject(rs);
        }
        /// <summary>
        /// 添加bom物料前的查询
        /// </summary>
        /// <param name="partner">更新父类别前查询</param>
        /// <returns></returns>
        public string GetUpdPurchasingBomTypeRecord(T_ProductBom pb)
        {
            string rs = "";
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_ProductBom> Record = new List<T_ProductBom>();
            Record = new AIDSvc().GetUpdPurchasingBomTypeRecord(out count, C_GUID, pb.nodes, pb.subnodes, pb.nodelevel, pb.nodesid,pb.MaterielManage_GUID);
            if (Record.Count == 0)
            {
                rs = "true";
            }
            else
            {
                rs = "false";
            }
            return rs;

            //return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //    , result.ToString().ToLower(), msg);
        }
        /// <summary>
        /// 添加物料
        /// </summary>
        /// <returns></returns>
        public string SubmitProductBomRecord(T_ProductBom form)
        {
            ExceResult rs = new ExceResult();
            bool result = false;
            string msg = string.Empty;
            result = new AIDSvc().SubmitProductBomRecord(form);
            if (result)
            {
                rs.success = true;
                rs.msg = "提交成功";
            }
            else
            {
                rs.success = true;
                rs.msg = "提交失败";
            }
            return JsonConvert.SerializeObject(rs);
        }
        /// <summary>
        /// 获取公共父类
        /// </summary>
        /// <returns></returns>
        public string GetComParentAidType(T_PTR ptr)
        {
            ptr.C_GUID = Session["CurrentCompanyGuid"].ToString();

            var subTypeList = new AIDSvc().GetComParentAidType(ptr);

            var strJson = ConvertToSelectJson(subTypeList, "AidTypeName", "AT_GUID");

            return strJson.ToString();
        }
        /// <summary>
        /// 查询bom表记录
        /// </summary>
        /// <returns></returns>
        public string GetQueryBom(string nodesid)
        {
            StringBuilder strJson = new StringBuilder();
            T_ProductBom record = new T_ProductBom();
            record = new AIDSvc().GetQueryBom(nodesid);
            strJson.Append(new JavaScriptSerializer().Serialize(record));
            return strJson.ToString();
        }

        /// <summary>
        /// 删除采购记录
        /// </summary>
        /// <param name="id">productbom纪录标识</param>
        /// <returns></returns>
        public string DelProductBomDetail(string id, int nodelevel)
        {
            bool result = new AIDSvc().DelProductBomDetail(id, nodelevel);
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
        ///// <summary>
        ///// 获取子类
        ///// </summary>
        ///// <returns></returns>
        public string GetMMType(string parentId)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            var subTypeList = new AIDSvc().GetMMType(C_GUID, parentId);

            var strJson = ConvertToSelectJson(subTypeList, "MM_Name", "MM_GUID");

            return strJson.ToString();
        }
        ///// <summary>
        ///// 获取子类A
        ///// </summary>
        ///// <returns></returns>
        public string GetMMTypeA(string parentId)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            var subTypeList = new AIDSvc().GetMMTypeA(C_GUID, parentId);

            var strJson = ConvertToSelectJson(subTypeList, "MM_Name", "MM_GUID");

            return strJson.ToString();
        }
        ///// <summary>
        ///// 获取子类NA
        ///// </summary>
        ///// <returns></returns>
        public string GetMMTypeNA(string parentId)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            var subTypeList = new AIDSvc().GetMMTypeNA(C_GUID, parentId);

            var strJson = ConvertToSelectJson(subTypeList, "MM_Name", "MM_GUID");

            return strJson.ToString();
        }
    }
}
