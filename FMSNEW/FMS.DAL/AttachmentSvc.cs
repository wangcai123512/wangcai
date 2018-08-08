using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMS.Model;
using System.Data;

namespace FMS.DAL
{
    public class AttachmentSvc
    {
        /// <summary>
        /// 查看 图片
        /// </summary>
        /// <param name="id">FR_GUID 记录标识</param>
        /// <returns></returns>
        public List<T_Attachment> GetAttachment(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAttachment";
            dh.AddPare("@FR_GUID", SqlDbType.NVarChar, 50, id);
            return dh.Reader<T_Attachment>();
        }
        /// <summary>
        /// 查看 图片
        /// </summary>
        /// <param name="id">FR_GUID 记录标识</param>
        /// <returns></returns>
        public T_Attachment GetAttachmentById(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAttachmentByID";
            dh.AddPare("@File_GUID", SqlDbType.NVarChar, 50, id);
            return dh.Reader<T_Attachment>().FirstOrDefault() ?? new T_Attachment();

        }
        /// 新增/编辑 图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddAttachment(T_Attachment entity)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_AddAttachment";
            db.AddPare("@A_GUID", SqlDbType.NVarChar, 50, entity.A_GUID);
            db.AddPare("@FileName", SqlDbType.NVarChar, 200, entity.FileName);
            db.AddPare("@FileType", SqlDbType.NVarChar, 500, entity.FileType);
            db.AddPare("@FR_GUID", SqlDbType.NVarChar, 50, entity.FR_GUID);
            db.AddPare("@FlieData", SqlDbType.VarBinary, 2147483647, entity.FlieData);
            db.AddPare("@FileRemark", SqlDbType.NVarChar, 200, entity.FileRemark = null);
            db.AddPare("@Number", SqlDbType.NVarChar, 200, entity.Number);
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
        public bool UpdHaveFileUpload(T_Attachment form)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdAttachmentName";

                dh.AddPare("@A_GUID", SqlDbType.NVarChar, 40, form.A_GUID);
                dh.AddPare("@FileName", SqlDbType.NVarChar, 200, form.FileName);
                dh.NonQuery();
                dh.CleanPara();
                dh.CommitTran();
                return true;
            }
            catch
            {
                dh.RollBackTran();
                return false;
            }
        }
        /// <summary>
        /// 修改备注
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        /// </summary>
        public bool UpdAttachment(string id,string name,string remark)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_UpdAttachment";
            db.AddPare("@A_GUID", SqlDbType.NVarChar, 50, id);
            db.AddPare("@FileName", SqlDbType.NVarChar, 200, name);
            db.AddPare("@FileRemark", SqlDbType.NVarChar, 500, remark);
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
        /// 删除财务记录下的附件
        /// </summary>
        /// <param name="id">FR_GUID 财务记录标识</param>
        /// <returns></returns>
        public bool DelAttachment(string id)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_DelAttachment";
            db.AddPare("@FR_ID", SqlDbType.NVarChar, 50, id);
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
        /// 一一删除记录下的每个附件
        /// </summary>
        /// <param name="id">A_GUID 记录标识</param>
        /// <returns></returns>
        public bool DelEveryAttachment(string id)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_DelEveryAttachment";
            db.AddPare("@A_GUID", SqlDbType.NVarChar, 50, id);
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

        public List<T_Attachment> ShowTime(out int count, string FR_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetMoreAttachment";
            dh.AddPare("@FR_GUID", SqlDbType.NVarChar, 50, FR_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_Attachment> result = new List<T_Attachment>();
            result= dh.Reader<T_Attachment>();
            count = dh.GetParaValue<int>("@Count");
            return result;       
        }
        /// <summary>
        /// 获取已转售的列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <param name="customer"></param>
        /// <param name="grp"></param>
        /// <param name="flag"></param>
        /// <param name="state"></param>    
        /// <returns></returns>
        public List<T_Attachment> ShowUploadFile(string id, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAttachmentList";
            dh.AddPare("@FR_GUID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@DateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@DateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            List<T_Attachment> result = new List<T_Attachment>();
            result = dh.Reader<T_Attachment>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 删除采购记录
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public bool DelUploadAttachment(string id)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_DelUploadAttachment";
            db.AddPare("@A_GUID", SqlDbType.NVarChar, 50, id);
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
