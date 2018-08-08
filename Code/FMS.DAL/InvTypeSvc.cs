using System.Collections.Generic;
using FMS.Model;

namespace FMS.DAL
{
    public class InvTypeSvc
    {
        /// <summary>
        /// 获取单据类型
        /// </summary>
        /// <returns></returns>
        public List<T_InvType> GetInvType()
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetInvType";
            return dh.Reader<T_InvType>();
        }
    }
}
