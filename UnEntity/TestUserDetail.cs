using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;

namespace UnCommon
{
    public class TestUserDetail
    {
        // 自增ID
        [UnAttrSql(fieldType = "bigint IDENTITY(1,1)", fieldNULL = false)]
        public long? TestUserDetailID { get; set; }

        // 编码
        [UnAttrSql(fieldType = "varchar(32)", fieldNULL = false)]
        public string TestUserDetailUID { get; set; }

        // 唯一编号,主键
        [UnAttrSql(isPrimaryKey = true, fieldNULL = false)]
        public Guid? TestUserDetailGUID { get; set; }

        // 添加时间
        [UnAttrSql(fieldType = "varchar(22)")]
        public string AddTime { get; set; }

        // 添加时间戳
        public long? AddTimeStamp { get; set; }

        // 添加时间
        [UnAttrSql(fieldType = "varchar(22)")]
        public string LastTime { get; set; }

        // 添加时间戳
        public long? LastTimeStamp { get; set; }

        // 是否删除
        [UnAttrSql(fieldNULL = false, fieldDefault = "0")]
        public bool? IsDelete { get; set; }

        // 用户GUID,外键
        [UnAttrSql(fieldNULL = false, isForeignKey = true, foreignKeyValue = new string[] { "TestUser", "TestUserGUID" })]
        public Guid? TestUserGUID { get; set; }

        // 国家
        [UnAttrSql(fieldType = "varchar(16)")]
        public string Country { get; set; }

        // 区号
        [UnAttrSql(fieldType = "varchar(16)")]
        public string TelAreaCode { get; set; }

        // 手机
        [UnAttrSql(fieldType = "varchar(16)")]
        public string TelMobile { get; set; }
    }
}
