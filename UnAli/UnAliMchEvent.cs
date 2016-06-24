using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aop.Api.Request;
using Aop.Api.Response;

namespace UnAli
{
    public enum UnAliMchEvent
    {
        预下单,
        查询订单,
        撤销订单,
        申请退款,
        条码支付
    }
}
