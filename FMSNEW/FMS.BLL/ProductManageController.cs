using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web.Mvc;
using BaseController;
using Common.Models;
using FMS.DAL;
using FMS.Model.DTO;
using FMS.Model;
using Newtonsoft.Json;
using EF = FMS.Models;

namespace FMS.BLL
{

    public class ProductManageController : BasicController
    {

        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            ViewBag.PID = id;
            return View();
        }


        public string GetProductList()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();

            EF.FMS_DevelopEntities entity = new EF.FMS_DevelopEntities();
            var typeId = Request.QueryString["typeId"];
            var subTypeId = Request.QueryString["subTypeId"];
            var MaterielManage = Request.QueryString["MaterielManage"];
            var business_GUID = Request.QueryString["business_GUID"];
            var subBusiness_GUID = Request.QueryString["subBusiness_GUID"];
            var product = from p in entity.V_Prodcut_List
                      where p.C_GUID == C_GUID && p.TypeId == typeId && p.SubTypeId == subTypeId && p.MaterielManage_GUID == MaterielManage && p.Business_GUID == business_GUID && p.SubBusiness_GUID == subBusiness_GUID
                      select p;
            if (MaterielManage =="")
            {
                product = from p in entity.V_Prodcut_List
                          where p.C_GUID == C_GUID
                          select p;
            }
            return JsonConvert.SerializeObject(product);
        }

        /// <summary>
        /// 更新产品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string UpdateProduct(string id, string count)
        {
            ExceResult res = new ExceResult();
            if (string.IsNullOrEmpty(id))
            {
                res.success = false;
                res.msg = "更新数据不正确";
                return JsonConvert.SerializeObject(res);
            }

            int newCount;
            if (!int.TryParse(count, out newCount))
            {
                res.success = false;
                res.msg = "数量不正确";
                return JsonConvert.SerializeObject(res);
            }


            DataTable bom = ProductSvc.UpdateProductDetail(id, newCount);

            List<ProductNode> treeNode = GetProductNoeList(bom);

            if (treeNode != null)
            {
                res.success = true;
                res.msg = JsonConvert.SerializeObject(treeNode);

            }
            else
            {
                res.success = false;
                res.msg = "未找到产品明细！";
            }

            return JsonConvert.SerializeObject(res);

        }


        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="guid">要删除的guid</param>
        /// <returns></returns>
        public string DeleteProduct(string guid)
        {
            ExceResult res = new ExceResult();
            string C_GUID = Session["CurrentCompanyGuid"].ToString();

            var result = ProductSvc.DeleteProduct(guid, C_GUID);
            if (result == 1)
            {
                res.success = true;
                res.msg = "";
            }
            else
            {
                res.success = false;
                res.msg = "不可以删除";
            }

            return JsonConvert.SerializeObject(res);
        }


        /// <summary>
        /// 获取收入列表
        /// </summary>
        /// <returns></returns>
        public string GetIEList()
        {
            EF.FMS_DevelopEntities entity = new EF.FMS_DevelopEntities();
            string C_GUID = Session["CurrentCompanyGuid"].ToString();

            var ieList = entity.SP_QueryIEListForProductSales(C_GUID);

            return JsonConvert.SerializeObject(ieList);

        }


        ///   <summary>
        /// 产品核销
        /// </summary>

        /// <param name="productGuid">产品ID</param>
        /// <param name="saledCount">核销产品数量</param>
        /// <param name="ieGuidList">核销的收入ID</param>
        /// <param name="ieDetail">收入明细（新增收入时传入)</param>
        /// <returns></returns>
        public string ProductSales(string productGuid, string stockAmount,string saledCount, List<string> ieGuidList, string ieDetail)
        {
            ExceResult res = new ExceResult();
            string C_GUID = Session["CurrentCompanyGuid"].ToString();


            decimal count;
            if (!decimal.TryParse(saledCount, out count) || count <= 0)
            {
                res.success = false;
                res.msg = "核销数量异常";
                return JsonConvert.SerializeObject(res);
            }
            decimal stockcount;
            if (!decimal.TryParse(stockAmount, out stockcount) || stockcount <= 0)
            {
                res.success = false;
                res.msg = "核销数量异常";
                return JsonConvert.SerializeObject(res);
            }

            if (string.IsNullOrEmpty(productGuid))
            {
                res.success = false;
                res.msg = "核销产品异常";
                return JsonConvert.SerializeObject(res);
            }

            if ((ieGuidList == null || ieGuidList.Count == 0) && string.IsNullOrEmpty(ieDetail))
            {
                res.success = false;
                res.msg = "收入数据异常";
                return JsonConvert.SerializeObject(res);
            }
            
            var ieGuid = string.Empty;
            T_IERecord ieRecord = new T_IERecord();
            ieRecord.AffirmDate = DateTime.Now;
            ieRecord.Date = DateTime.Now;

            if ( ieGuidList != null && ieGuidList.Count > 0)
            {
                //核销现有收入，用逗号链接
               ieGuid=string.Join(",", ieGuidList.ToArray());
            }
            else
            {
                string[] temp = ieDetail.Split(new char[] { ',' });
                ////客户,收入确认日期,账期截止日期,货币,收入金额,税种企业所得税,税费金额,含税总收入,备注,Business_GUID,SubBusiness_GUID
                //传入参数必须包含以上9个项目
                if (temp.Count() !=11)
                {
                    res.success = false;
                    res.msg = "收入数据异常";
                    return JsonConvert.SerializeObject(res);
                }
                else
                {
                    DateTime affirmDate;
                    DateTime endDate;
                    if(!DateTime.TryParse(temp[1],out affirmDate) )
                    {
                        res.success = false;
                        res.msg = "收入确认日期异常";
                        return JsonConvert.SerializeObject(res);
                    }

                    if (!DateTime.TryParse(temp[2], out endDate))
                    {
                        res.success = false;
                        res.msg = "账期截止日期异常";
                        return JsonConvert.SerializeObject(res);
                    }
                    ieRecord.Creator = base.userData.LoginFullName;
                    
                    ieRecord.RPer = temp[0];

                    ieRecord.AffirmDate = affirmDate;
                    ieRecord.Date = endDate;
                    ieRecord.Currency = temp[3];
                    ieRecord.Amount = Convert.ToDecimal(temp[4]);
                    ieRecord.TaxationType = temp[5];
                    ieRecord.TaxationAmount =Convert.ToDecimal( temp[6]);
                    ieRecord.SumAmount = Convert.ToDecimal(temp[7]);
                    ieRecord.Remark = temp[8];
                    ieRecord.Business_GUID = temp[9];
                    ieRecord.SubBusiness_GUID = temp[10];
                }

            }


            bool result = ProductSvc.SalesProduct(productGuid, ieGuid, count, C_GUID, ieRecord, stockcount);
            if (result)
            {
                res.success = true;
                res.msg = "";
            }
            else
            {
                res.success = false;
                res.msg = "不是该公司产品不可核销";
            }

            return JsonConvert.SerializeObject(res);
        }


        public string GetSaledDetail()
        {
            var productId = Request.QueryString["pguid"];

            EF.FMS_DevelopEntities entity = new EF.FMS_DevelopEntities();
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            var detail = from p in entity.V_Product_Saled_Detail
                         where p.C_GUID == C_GUID && p.product_guid == productId
                         select p;

            return JsonConvert.SerializeObject(detail);
        }



        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 获取产品结构
        /// </summary>
        /// <returns></returns>
        public string GetProductDetail(string pId)
        {
            ExceResult rs = new ExceResult();

            DataTable bom = ProductSvc.ProductDetail(pId);

            List<ProductNode> treeNode = GetProductNoeList(bom);

            if (treeNode != null)
            {
                rs.success = true;
                rs.msg = JsonConvert.SerializeObject(treeNode);
            }
            else
            {
                rs.success = false;
                rs.msg = "未找到产品明细！";
            }

            return JsonConvert.SerializeObject(rs);
        }

        /// <summary>
        /// 创建产品
        /// </summary>
        /// <param name="typeId">产品大类</param>
        /// <param name="subId">产品子类</param>
        /// <param name="cnt">创建的产品数量</param>
        /// <returns>创建成功后的产品树JSON结构</returns>
        /// <remarks>2017/1/19  liujf   create</remarks>
        public string CreateProduct(string item_counts,string BusId,string SBusId,string typeId, string subId, string mmId,string cnt)
        {
            ExceResult rs = new ExceResult();

            int count;

            if (int.TryParse(cnt, out count))
            {
                if (count < 1)
                {
                    rs.success = false;
                    rs.msg = "产品数量不正确";
                    return JsonConvert.SerializeObject(rs);
                }

                string comanyId = Session["CurrentCompanyGuid"].ToString();
                string Currency = Session["Currency"].ToString();
                bool bom = ProductSvc.CreateProduct(Currency,comanyId, item_counts, BusId, SBusId, typeId, subId, mmId, count);

                if (bom)
                {
                    rs.success = true;
                    rs.msg = "产品创建成功";

                }
                else
                {
                    rs.success = false;
                    rs.msg = "该产品创建失败";
                }

            }
            else
            {
                rs.success = false;
                rs.msg = "产品数量不正确";
            }
            return JsonConvert.SerializeObject(rs);
        }
        /// <summary>
        /// 获取产品细节
        /// </summary>
        public string getProductDetails(string BusId, string SBusId, string typeId, string subId, string mmId, string cnt)
        {  
            ExceResult rs = new ExceResult();

            int count;

            if (int.TryParse(cnt, out count))
            {
                if (count < 1)
                {
                    rs.success = false;
                    rs.msg = "产品数量不正确";
                    return JsonConvert.SerializeObject(rs);
                }

                string comanyId = Session["CurrentCompanyGuid"].ToString();

                List<T_ProductBom> bom = ProductSvc.getProductDetail(comanyId, BusId, SBusId, typeId, subId, mmId, count);

                if (bom != null)
                {
                    rs.success = true;
                    rs.msg = JsonConvert.SerializeObject(bom);
                }
                else
                {
                    rs.success = false;
                    rs.msg = "该产品未设置产品结构，请先设置产品结构。";
                }
            }
            else
            {
                rs.success = false;
                rs.msg = "产品数量不正确";
            }
            return JsonConvert.SerializeObject(rs);
        }
        /// <summary>
        /// 获取产品数量
        /// </summary>
        public string getProductNum(string BusId, string SBusId, string typeId, string subId, string mmId)
        {
            ExceResult rs = new ExceResult();

                string comanyId = Session["CurrentCompanyGuid"].ToString();

                List<T_ProductBom> bom = ProductSvc.getProductNum(comanyId, BusId, SBusId, typeId, subId, mmId);

                if (bom != null)
                {
                    rs.success = true;
                    rs.msg = JsonConvert.SerializeObject(bom);
                }
                else
                {
                    rs.success = false;
                    rs.msg = "该产品未设置产品结构，请先设置产品结构。";
                }

            return JsonConvert.SerializeObject(rs);
        }

        /// <summary>
        /// 根据Bom的Table转换为结构树的格式
        /// </summary>
        /// <param name="bom">bom的Table</param>
        /// <returns>结构树</returns>
        public List<ProductNode> GetProductNoeList(DataTable bom)
        {
            if (bom != null && bom.Rows.Count > 0)
            {
                //将BOM结构转换为Tree的JSON数据格式
                ProductNode productNode = new ProductNode();

                productNode.id = bom.Rows[0]["id"].ToString();

                productNode.text = bom.Rows[0]["node_name"].ToString();
                //节点ID
                productNode.value = bom.Rows[0]["node"].ToString();
                //数量
                List<string> tag = new List<string>();
                tag.Add(bom.Rows[0]["item_counts"].ToString());
                productNode.tags = tag;

                //查询字节点
                //检查该节点是否有子节点
                DataRow[] children = bom.Select("parent_node='" + bom.Rows[0]["node"].ToString() + "'");

                ConvertToNode(productNode, bom, children);

                List<ProductNode> treeNode = new List<ProductNode>();
                treeNode.Add(productNode);

                return treeNode;
            }
            else
            {
                return null;
            }
        }

        public void ConvertToNode(ProductNode productNode, DataTable bom, DataRow[] childs)
        {
            List<ProductNode> nodeList = new List<ProductNode>();

            foreach (DataRow row in childs)
            {
                ProductNode node = new ProductNode();

                node.id = row["id"].ToString();

                //节点名称
                node.text = row["node_name"].ToString();
                //节点ID
                node.value = row["node"].ToString();
                //数量
                node.tags = new List<string>();
                node.tags.Add(row["item_counts"].ToString());

                //检查该节点是否有子节点

                DataRow[] children = bom.Select("parent_node='" + node.value + "'");
                if (children.Count() > 0)
                {
                    ConvertToNode(node, bom, children);
                }

                nodeList.Add(node);

                //node.nodes = nodeList;
            }
            productNode.nodes = nodeList;

        }



    }

}


