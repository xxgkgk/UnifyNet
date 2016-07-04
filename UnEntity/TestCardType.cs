using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;

namespace UnCommon
{
    public class TestCardType
    {
        // 自增ID
        [UnAttrSql(fieldType = "bigint IDENTITY(1,1)", fieldNULL = false)]
        public long? TTestCardTypeID { get; set; }

        // 编码
        [UnAttrSql(fieldType = "varchar(32)", fieldNULL = false)]
        public string TestCardTypeUID { get; set; }

        // 唯一编号,主键
        [UnAttrSql(isPrimaryKey = true, fieldNULL = false)]
        public Guid? TestCardTypeGUID { get; set; }

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

        // 类型名
        [UnAttrSql(fieldType = "varchar(16)")]
        public string Name { get; set; }
    }
}
