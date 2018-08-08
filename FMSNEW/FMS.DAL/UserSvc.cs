using System.Collections.Generic;
using FMS.Model;
using System.Data;

namespace FMS.DAL
{
    public class UserSvc
    {
        /// <summary>
        /// 改变默认语言
        /// </summary>
        /// <returns></returns>
        public  bool ChangeLanguage(string guid , string language)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_UpdLanguage";
            db.AddPare("@GUID", SqlDbType.NVarChar, 50,guid );
            db.AddPare("@LANGUAGE", SqlDbType.NVarChar, 40, language);
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
