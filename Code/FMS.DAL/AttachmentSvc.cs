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
            db.AddPare("@FileType", SqlDbType.NVarChar, 50, entity.FileType);
            db.AddPare("@FR_GUID", SqlDbType.NVarChar, 50, entity.FR_GUID);
            db.AddPare("@FlieData", SqlDbType.VarBinary, 2147483647, entity.FlieData);
            db.AddPare("@FileRemark", SqlDbType.NVarChar, 200, entity.FileRemark=null);
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
    }
}
