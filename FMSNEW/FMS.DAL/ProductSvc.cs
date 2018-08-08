
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FMS.Model;
namespace FMS.DAL
{
    public class ProductSvc
    {

        /// <summary>
        /// 使用产品
        /// </summary>
        /// <param name="fromGuid">使用来源</param>
        /// <param name="useAmount">使用金额</param>
        /// <param name="typeTo">用于产品类别</param>
        /// <param name="subTypeTo">用于产品子类</param>
        public static int UseProduct(string fromGuid, decimal useAmount, string typeTo, string subTypeTo)
        {
            DBHelper dh = new DBHelper();

            dh.strCmd = "SP_UseProduct";
            dh.AddPare("@from_guid", SqlDbType.VarChar, 40, fromGuid);
            dh.AddPare("@use_amount", SqlDbType.Decimal, 18, useAmount);
            dh.AddPare("@type_to", SqlDbType.VarChar, 40, typeTo);
            dh.AddPare("@sub_type_to", SqlDbType.VarChar, 100, subTypeTo);
            dh.AddPare("@result", SqlDbType.Int, ParameterDirection.Output, 0, null);
            
            dh.NonQuery();
            return dh.GetParaValue<int>("@result");
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="guid">要删除的产品GUID</param>
        /// <param name="companyId">当前公司GUID</param>
        /// <returns>1=删除成功，-1=不可删除</returns>
        public static int DeleteProduct(string guid,string companyId)
        {
            DBHelper dh = new DBHelper();

            dh.strCmd = "SP_DeleteProduct";
            dh.AddPare("@guid", SqlDbType.VarChar, 40, guid);
            dh.AddPare("@c_guid", SqlDbType.VarChar, 40, companyId);
            dh.AddPare("@result", SqlDbType.Int, ParameterDirection.Output, 0, null);

            dh.NonQuery();
            return dh.GetParaValue<int>("@result");
        }
       

        /// <summary>
        /// 核销产品
        /// </summary>
        /// <param name="guid">要核销的产品</param>
        /// <param name="ieGuid">收入GUID（多个用逗号)</param>
        /// <param name="saledAmount">核销金额</param>
        /// <param name="companyId">当前公司GUID</param>
        /// <param name="ieDetail">收入明细</param>
        /// <returns></returns>
        public static bool SalesProduct(string guid, string ieGuid, decimal saledAmount, string companyId, T_IERecord rec, decimal stockcount)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
            dh.strCmd = "SP_SalesProduct";
            dh.AddPare("@p_guid", SqlDbType.VarChar, 40, guid);
            dh.AddPare("@c_guid", SqlDbType.VarChar, 40, companyId);
            dh.AddPare("@saled_count", SqlDbType.Decimal, 18, saledAmount);
            dh.AddPare("@ie_guid", SqlDbType.VarChar, int.MaxValue, ieGuid); 
            
            dh.AddPare("@RPer", SqlDbType.VarChar, 40, rec.RPer);
            dh.AddPare("@Creator", SqlDbType.VarChar, 40, rec.Creator);
            dh.AddPare("@AffirmDate", SqlDbType.VarChar, 40, rec.AffirmDate);
            dh.AddPare("@end_date", SqlDbType.VarChar, 40, rec.Date);
            dh.AddPare("@Amount", SqlDbType.VarChar, 40, rec.Amount);
            dh.AddPare("@TaxationAmount", SqlDbType.VarChar, 40, rec.TaxationAmount);
            dh.AddPare("@TaxationType", SqlDbType.VarChar, 40, rec.TaxationType);
            dh.AddPare("@SumAmount", SqlDbType.VarChar, 40, rec.SumAmount);
            dh.AddPare("@Remark", SqlDbType.VarChar, 40, rec.Remark);
            dh.AddPare("@Currency", SqlDbType.VarChar, 40, rec.Currency);
            dh.AddPare("@Business_GUID", SqlDbType.VarChar, 40, rec.Business_GUID);
            dh.AddPare("@SubBusiness_GUID", SqlDbType.VarChar, 40, rec.SubBusiness_GUID);

            
            dh.AddPare("@result", SqlDbType.Int, ParameterDirection.Output, 0, null);

            dh.NonQuery();
            dh.CleanPara();

            dh.strCmd = "SP_GetProductBatch";
            dh.AddPare("@c_id", SqlDbType.VarChar, 40, companyId);
            dh.AddPare("@pid", SqlDbType.VarChar, 40, guid);
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            dh.CleanPara();

            for (int j = 0; j < result.Count; j++)
            {
                    dh.strCmd = "SP_UpdProductBatch";
                    dh.AddPare("@Amount", SqlDbType.VarChar, 40, (saledAmount / stockcount) * result[j].Amount);
                    dh.AddPare("@GUID", SqlDbType.VarChar, 40, result[j].GUID);
                    dh.NonQuery();
                    dh.CleanPara();


            }
            dh.CommitTran();
            return true;
            }
            catch (System.Exception e)
            {
                dh.RollBackTran();
                return false;
            }
        }

        /// <summary>
        /// 查询产品的物料结构信息
        /// </summary>
        /// <param name="subTypeId">产品小类ID</param>
        /// <param name="count">产品数量</param>
        /// <returns></returns>
        public static DataTable QueryProductBomView(string subTypeId,int count)
        {
            DBHelper dh = new DBHelper();

            dh.strCmd = "SP_ProductReview";
            dh.AddPare("@product_subtype_id", SqlDbType.VarChar, 40, subTypeId);
            dh.AddPare("@count", SqlDbType.VarChar, 40, count);

            DataSet ds=dh.Query();
            if(ds !=null && ds.Tables.Count>0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 查询产品的物料结构信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="typeId">产品大类</param>
        /// <param name="subTypeId">产品小类ID</param>
        /// <param name="count">产品数量</param>
        /// <returns></returns>
        public static bool CreateProduct(string Currency,string companyId, string item_counts, string BusId, string SBusId, string typeId, string subTypeId, string mmId, int count)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_CreateProduct";
                dh.AddPare("@c_id", SqlDbType.VarChar, 40, companyId);
                dh.AddPare("@Business_GUID", SqlDbType.VarChar, 40, BusId);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.VarChar, 40, SBusId);
                dh.AddPare("@product_subtype_id", SqlDbType.VarChar, 40, subTypeId);
                dh.AddPare("@product_type_id", SqlDbType.VarChar, 40, typeId);
                dh.AddPare("@Currency", SqlDbType.VarChar, 40, Currency);      
                dh.AddPare("@product_mm_id", SqlDbType.VarChar, 40, mmId);
                dh.AddPare("@product_count", SqlDbType.VarChar, 40, count);
                dh.AddPare("@pid", SqlDbType.VarChar, ParameterDirection.Output, 40, null);
                dh.NonQuery();
                string pid = dh.GetParaValue<string>("@pid");
                dh.CleanPara();
                

                string[] arrQueryString = item_counts.Split(';');
                for (int i = 0; i < arrQueryString.Length; i++)
                {
                    string[] arr = arrQueryString[i].Split(':');
                    dh.strCmd = "SP_CreateProducts";
                    dh.AddPare("@pid", SqlDbType.VarChar, 40, pid);
                    dh.AddPare("@c_id", SqlDbType.VarChar, 40, companyId);
                    dh.AddPare("@Business_GUID", SqlDbType.VarChar, 40, BusId);
                    dh.AddPare("@SubBusiness_GUID", SqlDbType.VarChar, 40, SBusId);
                    dh.AddPare("@product_mm_id", SqlDbType.VarChar, 40, arr[0]);
                    dh.AddPare("@item_counts", SqlDbType.VarChar, 40, arr[1]);
                    dh.AddPare("@item_num", SqlDbType.Int, 5,  Convert.ToInt32(arr[1]));
                    dh.AddPare("@mmId", SqlDbType.VarChar, 40, mmId);
                    dh.AddPare("@flag", SqlDbType.VarChar, 40, arr[2]); 
                dh.NonQuery();
                dh.CleanPara();
                }

                dh.strCmd = "SP_CreateProductsT";
                dh.AddPare("@pid", SqlDbType.VarChar, 40, pid);
                dh.AddPare("@c_id", SqlDbType.VarChar, 40, companyId);
                dh.AddPare("@Business_GUID", SqlDbType.VarChar, 40, BusId);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.VarChar, 40, SBusId);
                dh.AddPare("@product_mm_id", SqlDbType.VarChar, 40, mmId);
                dh.AddPare("@item_counts", SqlDbType.VarChar, 40, count);
                dh.AddPare("@item_num", SqlDbType.Int, 5, count);
                dh.AddPare("@mmId", SqlDbType.VarChar, 40, mmId);
                dh.AddPare("@flag", SqlDbType.VarChar, 40, "D"); 
                dh.NonQuery();
                dh.CleanPara();

                decimal amount = 0;
                for (int i = 0; i < arrQueryString.Length; i++)
                {
                    string[] arr = arrQueryString[i].Split(':');

                        dh.strCmd = "SP_GetProductsstock";
                        dh.AddPare("@c_id", SqlDbType.VarChar, 40, companyId);
                        dh.AddPare("@Business_GUID", SqlDbType.VarChar, 40, BusId);
                        dh.AddPare("@SubBusiness_GUID", SqlDbType.VarChar, 40, SBusId);
                        dh.AddPare("@product_mm_id", SqlDbType.VarChar, 40, arr[0]);
                        List<T_AIDRecord> result = new List<T_AIDRecord>();
                        result = dh.Reader<T_AIDRecord>();
                        dh.CleanPara();
                        int item_row = Convert.ToInt32(arr[1]);
                        for (int j = 0; j < result.Count; j++)
                        {
                            if (item_row <= result[j].Inventory_Number && item_row > 0)
                            {
                                dh.strCmd = "SP_UpdProductstocks";
                                dh.AddPare("@Inventory_Number", SqlDbType.Int, 5, result[j].Inventory_Number - item_row);
                                dh.AddPare("@GUID", SqlDbType.VarChar, 40, result[j].GUID);
                                dh.NonQuery();
                                dh.CleanPara();

                                dh.strCmd = "SP_CreateAIDBatch";
                                dh.AddPare("@pid", SqlDbType.VarChar, 40, pid);
                                dh.AddPare("@AID_GUID", SqlDbType.VarChar, 40, result[j].GUID);
                                dh.AddPare("@item_code", SqlDbType.VarChar, 40, arr[0]);
                                dh.AddPare("@Inventory_Number", SqlDbType.Int, 5, item_row);
                                dh.AddPare("@flag", SqlDbType.VarChar, 40, arr[2]);
                                dh.AddPare("@Amount", SqlDbType.VarChar, 40, (item_row * result[j].Amount) / result[j].Number);
                                dh.NonQuery();
                                dh.CleanPara();

                                amount = amount + (item_row * result[j].Amount) / result[j].Number;
                                break;
                            }
                            else if (item_row > result[j].Inventory_Number && result[j].Inventory_Number > 0)
                            {
                                dh.strCmd = "SP_UpdProductstocks";
                                dh.AddPare("@Inventory_Number", SqlDbType.Int, 5, 0);
                                dh.AddPare("@GUID", SqlDbType.VarChar, 40, result[j].GUID);
                                dh.NonQuery();
                                dh.CleanPara();

                                dh.strCmd = "SP_CreateAIDBatch";
                                dh.AddPare("@pid", SqlDbType.VarChar, 40, pid);
                                dh.AddPare("@item_code", SqlDbType.VarChar, 40, arr[0]);
                                dh.AddPare("@AID_GUID", SqlDbType.VarChar, 40, result[j].GUID);
                                dh.AddPare("@Inventory_Number", SqlDbType.Int, 5, result[j].Inventory_Number);
                                dh.AddPare("@flag", SqlDbType.VarChar, 40, arr[2]);
                                dh.AddPare("@Amount", SqlDbType.VarChar, 40, (result[j].Inventory_Number * result[j].Amount) / result[j].Number);
                                dh.NonQuery();
                                dh.CleanPara();

                                amount = amount + (result[j].Inventory_Number * result[j].Amount) / result[j].Number;
                                item_row = item_row - result[j].Inventory_Number;
                            }
                        }
                }

                dh.strCmd = "SP_UpdProduct";
                dh.AddPare("@GUID", SqlDbType.VarChar, 40, pid);
                dh.AddPare("@Amount", SqlDbType.VarChar, 40, amount);
                dh.NonQuery();
                dh.CleanPara();

                dh.CommitTran();
                return true;
            }
            catch (System.Exception e)
            {
                dh.RollBackTran();
                return false;
            }
        }

        /// <summary>
        /// 查询产品的明细信息
        /// </summary>
        /// <param name="productId">产品ID</param> 
        /// <returns></returns>
        public static DataTable ProductDetail(string productId)
        {
            DBHelper dh = new DBHelper();

            dh.strCmd = "SP_ProductDetail";
            dh.AddPare("@product_id", SqlDbType.VarChar, 50, productId); 

            DataSet ds = dh.Query();
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// 查询产品的明细信息库存
        /// </summary>
        /// <returns></returns>
        public static List<T_ProductBom> getProductDetail(string companyId, string BusId, string SBusId, string typeId, string subTypeId, string mmId, int count)
        {
            DBHelper dh = new DBHelper();

            dh.strCmd = "SP_ProductStock";
            dh.AddPare("@Business_GUID", SqlDbType.VarChar, 50, BusId);
            dh.AddPare("@SubBusiness_GUID", SqlDbType.VarChar, 50, SBusId);
            dh.AddPare("@product_mm_id", SqlDbType.VarChar, 50, mmId);
            dh.AddPare("@product_num", SqlDbType.Int, 5, count);
            return dh.Reader<T_ProductBom>();
        }
        /// <summary>
        /// 查询产品的明细信息库存2
        /// </summary>
        /// <returns></returns>
        public static List<T_ProductBom> getProductNum(string companyId, string BusId, string SBusId, string typeId, string subTypeId, string mmId)
        {
            DBHelper dh = new DBHelper();

            dh.strCmd = "SP_ProductStockNum";
            dh.AddPare("@Business_GUID", SqlDbType.VarChar, 50, BusId);
            dh.AddPare("@SubBusiness_GUID", SqlDbType.VarChar, 50, SBusId);
            dh.AddPare("@product_mm_id", SqlDbType.VarChar, 50, mmId);
            return dh.Reader<T_ProductBom>();
        }
        /// <summary>
        /// 查询产品的明细信息
        /// </summary>
        /// <param name="id">产品明细GUID</param> 
        /// <param name="newCount">最新的数量</param>
        /// <returns>
        /// 更新成功返回最新明细
        /// </returns>
        public static DataTable UpdateProductDetail(string id, int newCount)
        {
            DBHelper dh = new DBHelper();

            dh.strCmd = "SP_UpdateProductDetail";
            dh.AddPare("@id", SqlDbType.VarChar, 50, id);
            dh.AddPare("@new_count", SqlDbType.Int, 5, newCount);

            DataSet ds = dh.Query();
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        } 
        
    }

}
