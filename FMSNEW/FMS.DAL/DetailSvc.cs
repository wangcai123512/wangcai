using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        /// <summary>
        /// 获取费用类别列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_ExpenseType> GetExpenseTypeList(string c_guid,out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetExpenseTypeList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, c_guid);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_ExpenseType> result = new List<T_ExpenseType>();
            result = dh.Reader<T_ExpenseType>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public T_ExpenseType GetExpenseTypeRecord(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetExpenseTypeList";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            //dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            return dh.Reader<T_ExpenseType>().FirstOrDefault();
        }

        public bool UpdExpenseTypeRecord(T_ExpenseType form,string id)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_UpdExpenseTypeRecord";
            db.AddPare("@C_GUID", SqlDbType.NVarChar, 40, id);
            db.AddPare("@ET_GUID", SqlDbType.NVarChar, 40, form.ET_GUID);
            db.AddPare("@ExpenseType", SqlDbType.NVarChar, 40, form.ExpenseType);
            db.AddPare("@ExpenseFlag", SqlDbType.NVarChar, 1, form.ExpenseFlag);
            db.AddPare("@SaleFlag", SqlDbType.NVarChar, 1, form.SaleFlag);
            db.AddPare("@ManageFlag", SqlDbType.NVarChar, 1, form.ManageFlag);
            db.AddPare("@FinanceFlag", SqlDbType.NVarChar, 1, form.FinanceFlag);
            db.AddPare("@OtherFlag", SqlDbType.NVarChar, 1, form.OtherFlag);
            db.AddPare("@TaxFlag", SqlDbType.NVarChar, 1, form.TaxFlag);
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

        public bool DelExpenseTypeRecord(string guid)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_DelExpenseTypeRecord";
            db.AddPare("@ET_GUID", SqlDbType.NVarChar, 40, guid);
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
    }
}
