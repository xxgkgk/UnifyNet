using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UnEntity
{
    /// <summary>
    /// API处理结果通知
    /// </summary>
    public class ApiNote
    {
        /// <summary>
        /// 结果通知编号
        /// </summary>
        public int? NoteCode { get; set; }

     
        /// <summary>
        /// 结果通知信息
        /// </summary>
        public string NoteMsg { get; set; }


        public DateTime? dt { get; set; }
    }
}
