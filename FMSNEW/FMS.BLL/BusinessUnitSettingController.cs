using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.Model;
using FMS.BLL;
using FMS.DAL;
using System.Web.Script.Serialization;

namespace FMS.BLL
{
    public class BusinessUnitSettingController : UserController
    {
        public BusinessUnitSettingController()
            : base("BusinessUnitSetting")
        { }
        public ActionResult Index() 
        {
            return View();
        }
        public string GetBusinessType(string TypeName)
        {
            string result = "";
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_BusinessType> types = new List<T_BusinessType>();
            types = new BusinessTypeSvc().GetBusinessType(TypeName,C_GUID);
            if(types.Count ==0){
                result = "true";
            }else{
                result = "false";
            }
            return result;
        }
        public string GetBusinessChildTypeRecord(string GUID, string SubBusinessName) 
        {
            string result = "";
            List<T_BusinessType> types = new List<T_BusinessType>();
            types = new BusinessTypeSvc().GetBusinessChildTypeRecord(GUID, SubBusinessName);
            if (types.Count == 0)
            {
                result = "true";
            }
            else
            {
                result = "false";
            }
            return result;
        }
        public string CheckTypeUsed(string GUID, string Parent_GUID)
        {
            string res = null;
           List<T_BusinessType> types = new List<T_BusinessType>();
           if (Parent_GUID == GUID)
           {
               types = new BusinessTypeSvc().CheckTypeUsed(GUID);
           } else { 
           types = new BusinessTypeSvc().CheckTypeUsed(GUID, Parent_GUID);
           }
           if (types.Count != 0)
           {
               res = "flase";
           }
           else {
               res = "true";
           }
            return res;
            
        }

        public string DelType(string GUID, string Parent_GUID)
        {
            string res=null;
            if (Parent_GUID == GUID)
            {
               bool result = new BusinessTypeSvc().DelType(GUID);
               if (result)
               {
                   res = "true";
               }
               else
               {
                   res = "false";
               }
            }
            else { 
            bool result = new BusinessTypeSvc().DelType(GUID, Parent_GUID);
            if (result)
            {
                res = "true";
            }
            else
            {
                res = "false";
            }
            }
            
            return res;
        }

        public string GetBusinessChildTpyList(string GUID)
        {
           
            var subTypeList = new BusinessTypeSvc().GetBusinessChildTypeList(GUID);
           var strJson = ConvertToSelectJson(subTypeList, "SubBusinessName", "Sub_GUID");
            return strJson.ToString();
        }

        public string UpdBusinessChildTypeRecord(string SubBusinessName, string GUID, string Remark) 
        {
            string rs = "";
            string Sub_GUID = Guid.NewGuid().ToString();
            bool result = new BusinessTypeSvc().UpdBusinessChildTypeRecord(Sub_GUID, SubBusinessName, GUID, Remark);
            if (result)
            {
                rs = "true";
            }
            else
            {
                rs = "false";
            }
            return rs;
        }
        
        public string UpdBusinessType(string TypeName) 
        {
            string rs = "";
            string GUID = Guid.NewGuid().ToString();
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            bool result = new BusinessTypeSvc().UpdBusinessType(GUID, TypeName, C_GUID);
            if (result)
            {
                rs = "true";
            }
            else
            {
                rs = "false";
            }
            return rs;
        }

        public string GetBusinessUnionTypeList(int pageSize, int pageIndex = 1) 
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_BusinessType> types = new List<T_BusinessType>();
            types = new BusinessTypeSvc().GetBusinessUnionTypeList(pageIndex, -1, out count, C_GUID);
            return new JavaScriptSerializer().Serialize(types);
        }
        public string GetBusinessTypeList() {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            var subTypeList = new BusinessTypeSvc().GetBusinessTypeList(C_GUID);
            var strJson = ConvertToSelectJson(subTypeList, "BusinessName", "GUID");

           
            return strJson.ToString();
        }
    }
}
