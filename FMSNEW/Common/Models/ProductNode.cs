using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Models
{
    /// <summary>
    /// 产品树的节点类型
    /// </summary>
    public class ProductNode
    {
        /// <summary>
        /// 节点guid
        /// </summary>
        public string id { set; get; }

        /// <summary>
        /// 节点中文名
        /// </summary>
        public string text { set; get; }

        /// <summary>
        /// 节点ID
        /// </summary>
        public string value { set; get; }
        
        /// <summary>
        /// 节点数量
        /// </summary>
        public List<string> tags { set; get; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<ProductNode> nodes { set; get; }


    }
}
