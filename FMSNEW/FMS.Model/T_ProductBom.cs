using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 物料，资产对象
    /// </summary>
    public class T_ProductBom
    {
     
        public string nodesid { get; set; }

        public string nodes { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int tags { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int number { get; set; }


        public string subnodes { get; set; }

        /// <summary>
        /// 当前bom层次结构
        /// </summary>
        public int nodelevel { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public string MaterielManage_GUID { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public string stock_num { set; get; }

        /// <summary>
        /// 名称
        /// </summary>
        public string node_name { set; get; }
        /// <summary>
        /// 制造总数
        /// </summary>
        public int item_counts { set; get; }

    }
} 
