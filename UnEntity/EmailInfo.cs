using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnEntity
{
    /// <summary>
    /// 邮件信息
    /// </summary>
    public class EmailInfo
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public long EmID { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string EmContent { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 预设时间
        /// </summary>
        public DateTime PresetTime { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 发送状态
        /// </summary>
        public bool IsSend { get; set; }
        /// <summary>
        /// 发送结果备注
        /// </summary>
        public string BackRemark { get; set; }
        /// <summary>
        /// 发送类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 发送类型备注
        /// </summary>
        public string TypeRemark { get; set; }
    }
}
