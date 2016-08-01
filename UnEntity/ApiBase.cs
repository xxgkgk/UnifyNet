using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnEntity
{
    /// <summary>
    /// API基础信息
    /// </summary>
    public class ApiBase
    {
        /// <summary>
        /// 模块
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; set; }

        public Guid? guid{ get; set; }

        public List<Test> ArrayOfTest { get; set; }

        public string IsTest { get; set; }

    }
}
