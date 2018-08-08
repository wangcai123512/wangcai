using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FMS.Model;

namespace FMS.DAL
{
    public class BusinessTypeSvc
    {
        public List<T_BusinessType> GetBusinessType(string TypeName,string C_GUID) 
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBusinessType";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@BusinessName", SqlDbType.NVarChar, 50, TypeName);
            List<T_BusinessType> result = new List<T_BusinessType>();
            result = dh.Reader<T_BusinessType>();
            return result;
        }

        public List<T_BusinessType> GetBusinessUnionTypeList(int pageIndex, int pageSize, out int count, string C_GUID) 
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBusinessTypeList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_BusinessType> result = new List<T_BusinessType>();
            result = dh.Reader<T_BusinessType>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_BusinessType> GetBusinessTypeList(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBusinessType";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            List<T_BusinessType> result = new List<T_BusinessType>();
            result = dh.Reader<T_BusinessType>();
            return result;
        }
        public List<T_BusinessType> GetBusinessChildTypeRecord(string GUID, string SubBusinessName) 
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBusinessChildTypeRecord";
            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, GUID);
            dh.AddPare("@SubBusinessName", SqlDbType.NVarChar, 50, SubBusinessName);
            List<T_BusinessType> result = new List<T_BusinessType>();
            result = dh.Reader<T_BusinessType>();
            return result;
        
        }

        public List<T_BusinessType> CheckTypeUsed(string GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_CheckTypeUsed";

            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, GUID);
            List<T_BusinessType> result = new List<T_BusinessType>();
            result = dh.Reader<T_BusinessType>();
            return result;
        }

        public List<T_BusinessType> CheckTypeUsed(string GUID, string Parent_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_CheckTypeUsed";
          
            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, GUID);
            dh.AddPare("@Parent_GUID", SqlDbType.NVarChar, 50, Parent_GUID);
            List<T_BusinessType> result = new List<T_BusinessType>();
            result = dh.Reader<T_BusinessType>();
            return result;
            
        }

        public bool DelType(string GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelType";

            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, GUID);
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

        public bool DelType(string GUID, string Parent_GUID) {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelType";

            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, GUID);
            dh.AddPare("@Parent_GUID", SqlDbType.NVarChar, 50, Parent_GUID);
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

        public List<T_BusinessType> GetBusinessChildTypeList(string GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBusinessChildTypeRecord";
            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, GUID);
            List<T_BusinessType> result = new List<T_BusinessType>();
            result = dh.Reader<T_BusinessType>();
            return result;
        }

        public bool UpdBusinessType(string GUID, string TypeName, string C_GUID) 
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdBusinessType";
            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@BusinessName", SqlDbType.NVarChar, 50,TypeName );
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

        public bool UpdBusinessChildTypeRecord(string Sub_GUID, string SubBusinessName,string GUID,string remark)
        { 
          DBHelper dh = new DBHelper();
          dh.strCmd = "SP_UpdBusinessChildTypeRecord";
          dh.AddPare("@Sub_GUID", SqlDbType.NVarChar, 50, Sub_GUID);
          dh.AddPare("@SubBusinessName", SqlDbType.NVarChar, 50, SubBusinessName);
          dh.AddPare("@GUID", SqlDbType.NVarChar, 50, GUID);
          dh.AddPare("@Remark", SqlDbType.NVarChar, 50, remark);
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

    }
}
