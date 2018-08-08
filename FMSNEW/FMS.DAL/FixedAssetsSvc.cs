using System.Collections.Generic;
using System.Data;
using FMS.Model;

namespace FMS.DAL
{
    public class FixedAssetsSvc
    {
        /// <summary>
        /// 获取资产分类
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_AssetsGroup> GetAssetsGroups(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAssetsGroups";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_AssetsGroup>();
        }

        /// <summary>
        /// 更新资产分类
        /// </summary>
        /// <param name="item">资产分类对象</param>
        /// <returns></returns>
        public bool UpdAssetsGroup(T_AssetsGroup item)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdAssetsGroup";
            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, item.AG_GUID);
            dh.AddPare("@NAME", SqlDbType.NVarChar, 50, item.Name);
            dh.AddPare("@method", SqlDbType.Int, 0, item.DepreciationMethod);
            dh.AddPare("@SalvageRate", SqlDbType.Decimal, 7, item.SalvageRate);
            dh.AddPare("@life", SqlDbType.Int, 0, item.Life);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, item.C_GUID);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取资产列表
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="totalCount">纪录总数</param>
        /// <param name="flag">状态标志：0：所有，1：正在使用，2：可出售</param>
        /// <returns></returns>
        public List<T_Assets> GetAssetses(int pageSize, int pageIndex, out int totalCount, int flag,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAssetses";
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@TotalCount", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@flag", SqlDbType.Int, 0, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            List<T_Assets> result = new List<T_Assets>();
            try
            {
                result = dh.Reader<T_Assets>();
            }
            catch (System.Exception)
            {

                throw;
            }
            totalCount = dh.GetParaValue<int>("@TotalCount");
            return result;
        }

        /// <summary>
        /// 获取资产
        /// </summary>
        /// <param name="id">资产标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_Assets> GetAssets(string id,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAssetses";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            List<T_Assets> result = dh.Reader<T_Assets>();
            return result;
        }

        /// <summary>
        /// 更新资产
        /// </summary>
        /// <param name="item">资产对象</param>
        /// <returns></returns>
        public bool UpdAssets(T_Assets item)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdAssets";
            dh.AddPare("@ID", SqlDbType.NVarChar, 50, item.A_GUID);
            dh.AddPare("@No", SqlDbType.NVarChar, 100, item.No);
            dh.AddPare("@NAME", SqlDbType.NVarChar, 100, item.Name);
            dh.AddPare("@AG_Guid", SqlDbType.NVarChar, 50, item.AG_GUID);
            dh.AddPare("@PurchaseDate", SqlDbType.DateTime, 0, item.PurchaseDate);
            dh.AddPare("@RegDate", SqlDbType.DateTime, 0, item.RegisterDate);
            dh.AddPare("@AssetsCost", SqlDbType.Decimal, 0, item.AssetsCost);
            dh.AddPare("@Creator", SqlDbType.NVarChar, 50, item.Creator);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, item.C_GUID);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 更新资产状态
        /// </summary>
        /// <param name="id">资产标识</param>
        /// <param name="flag">状态标识</param>
        /// <returns></returns>
        public bool UpdAssetsStat(string id, string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdAssetsStat";
            dh.AddPare("@ID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@Stat", SqlDbType.NVarChar, 10, flag);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }
}
