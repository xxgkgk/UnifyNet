using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnCommon.Entity;

namespace UnCommon
{
    public class TestUser
    {
        // 自增ID
        [UnAttrSql(fieldType = "bigint IDENTITY(1,1)", fieldNULL = false,oldFieldName = "TestUserIDD")]
        public long? TestUserID { get; set; }

        // 编码
        [UnAttrSql(fieldType = "varchar(32)", fieldNULL = false, fieldDefault = "1234")]
        public string TestUserUID { get; set; }

        // GUID,主键
        [UnAttrSql(isPrimaryKey = true, fieldNULL = false)]
        public Guid? TestUserGUID { get; set; }

        // 用户名
        [UnAttrSql(fieldType = "varchar(32)", fieldNULL = false, indexModel = IndexModel.Unique)]
        public string Name { get; set; }

        // 用户密码,MD5*2
        [UnAttrSql(fieldType = "varchar(32)", fieldNULL = false)]
        public string Pass { get; set; }

        // 是否删除
        //[UnAttrSql(fieldNULL = false, fieldDefault = "0")]
        //public bool? IsDelete { get; set; }

        // 单列索引A
        [UnAttrSql(fieldType = "varchar(32)")]
        public string NonclusteredA { get; set; }

        // 单列索引
        [UnAttrSql(fieldType = "varchar(32)")]
        public string NonclusteredB { get; set; }

        // 联合索引A
        [UnAttrSql(fieldType = "varchar(32)", indexModel = IndexModel.UnionClustered)]
        public string UnionClusteredA { get; set; }

        // 联合索引B
        [UnAttrSql(fieldType = "varchar(32)", indexModel = IndexModel.UnionClustered)]
        public string UnionClusteredB { get; set; }

        // 联合非聚集索引A
        [UnAttrSql(fieldType = "varchar(32)", indexModel = IndexModel.UnionNonclustered)]
        public string UnionNonclusteredA { get; set; }

        // 联合非聚集索引B
        [UnAttrSql(fieldType = "varchar(32)", indexModel = IndexModel.UnionNonclustered)]
        public string UnionNonclusteredB { get; set; }

        [UnAttrSql(fieldType = "varchar(65)",fieldDefault = "333", oldFieldName = "a1")]
        public string a { get; set; }

        [UnAttrSql(fieldDefault = "0")]
        public int? b { get; set; }


        [UnAttrSql(fieldType = "varchar(20)", fieldDefault = "'李二罗'")]
        public string c { get; set; }


        //public Guid? c { get; set; }

        //public float? d { get; set; }

        //public Int16? e { get; set; }

        //public decimal? f { get; set; }

        // public Int64? g { get; set; }

        //public Int16? h { get; set; }

        //public Byte? i { get; set; }
        //public byte? j { get; set; }

        //public string k { get; set; }

        [UnAttrSql(fieldType = "varchar(20)", fieldDefault = "'ddA人'",fieldNULL = false)]
        public string l { get; set; }
    }
}
