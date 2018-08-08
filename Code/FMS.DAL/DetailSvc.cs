using System.Collections.Generic;
using System.Data;
using FMS.Model;

namespace FMS.DAL
{
    public class DetailSvc
    {
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="detail">类别对象</param>
        /// <returns></returns>
        public bool UpdDetail(T_DetailedCategories detail)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdDetail";
            dh.AddPare("@GUID", SqlDbType.NVarChar, 40, detail.GUID);
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, detail.Name);
            dh.AddPare("@State", SqlDbType.NVarChar,40, detail.State);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新类别启用状态
        /// </summary>
        /// <param name="guid">记录标识</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public bool UpdDetailstate(string guid, string state)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_UpdDetailstate";
            db.AddPare("@GUID", SqlDbType.NVarChar, 40, guid);
            db.AddPare("@State", SqlDbType.NVarChar, 40, state);
            try
            {
                db.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="guid">记录标识</param>
        /// <returns></returns>
        public bool DelDetails(string guid)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_DelDetails";
            db.AddPare("@GUID", SqlDbType.NVarChar, 40, guid);
            try
            {
                db.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取类别列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_DetailedCategories> GetDetailList(int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailList";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_DetailedCategories> result = new List<T_DetailedCategories>();
            result = dh.Reader<T_DetailedCategories>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_DetailedCategories> GetAllDetail()
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAllDetail";
            List<T_DetailedCategories> result = new List<T_DetailedCategories>();
            result = dh.Reader<T_DetailedCategories>();
            return result;
        }
    }
}
