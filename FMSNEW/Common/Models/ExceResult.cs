using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Models
{
    public class ExceResult
    {
        /// <summary>
        /// 执行成功
        /// </summary>
        public bool success { set; get; }

        /// <summary>
        /// 执行的结果消息
        /// </summary>
        public string msg { set; get; }
    }
}
