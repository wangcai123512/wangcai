using System;

namespace FMS.Model
{
    /// <summary>
    /// 附件对象
    /// </summary>
    public class T_Attachment
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string A_GUID { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// 记录ID
        /// </summary>
        public string FR_GUID { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] FlieData { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileRemark { get; set; }
        public string Number { get; set; }
    }
}
