using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnEntity
{
    /// <summary>
    /// 短信信息
    /// </summary>
    public class SMSInfo
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public long SMSID { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNum { get; set; }
        /// <summary>
        /// 短信内容
        /// </summary>
        public string SendContent { get; set; }
        /// <summary>
        /// 添加时间(默认当前时间)
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 预计发送时间(默认当前时间)
        /// </summary>
        public DateTime PresetTime { get; set; }
        /// <summary>
        /// 实际发送时间(系统生成)
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 是否发送
        /// </summary>
        public bool IsSend { get; set; }
        /// <summary>
        /// 返回结果备注
        /// </summary>
        public string BackRemark { get; set; }
        /// <summary>
        /// 发送类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 类型备注
        /// </summary>
        public string TypeRemark { get; set; }
    }
}
