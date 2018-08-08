using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model.DTO
{
    public class ProductUseDTO
    {
        /// <summary>
        /// 使用来源GUID
        /// </summary>
        public string fromGuid
        {
            set;
            get;
        }
        /// <summary>
        /// 使用来源的产品类别ID
        /// </summary>
        public string typeFrom
        {
            set;
            get;
        }

        /// <summary>
        /// 使用来源的产品子类别ID
        /// </summary>
        public string subTypeFrom
        {
            set;
            get;
        }


        /// <summary>
        /// 使用金额
        /// </summary>
        public string useAmount
        {
            set;
            get;
        }

        /// <summary>
        /// 使用到的产品类别ID
        /// </summary>
        public string typeTo
        {
            set;
            get;
        }

        /// <summary>
        /// 使用到的产品子类别ID
        /// </summary>
        public string subTypeTo
        {
            set;
            get;
        }
    }
}
