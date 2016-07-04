using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;

namespace UnCommon
{
    public class TestCard
    {
        // 自增ID
        [UnAttrSql(fieldType = "bigint IDENTITY(1,1)", fieldNULL = false)]
        public long? TestCardID { get; set; }

        // 编码
        [UnAttrSql(fieldType = "varchar(32)", fieldNULL = false,fieldDefault = "123")]
        public string TestCardUID { get; set; }

        [UnAttrSql(isPrimaryKey = true, fieldNULL = false)]
        // 唯一编号,主键
        public Guid? TestCardGUID { get; set; }

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

        // 卡类型GUID,外键
        [UnAttrSql(fieldNULL = false, isForeignKey = true, foreignKeyValue = new string[] { "TestCardType", "TestCardTypeGUID" })]
        public Guid? TestCardTypeGUID { get; set; }

        // 用户GUID,外键
        [UnAttrSql(fieldNULL = false, isForeignKey = true, foreignKeyValue = new string[] { "TestUser", "TestUserGUID" })]
        public Guid? TestUserID { get; set; }

        // 余额
        [UnAttrSql(fieldDefault = "0", fieldNULL = false)]
        public decimal? Money { get; set; }
    }
}
