using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using UnCommon;
using UnCommon.Encrypt;
using Com.Alipay;

namespace UnAli
{
    public static class UnNotify
    {
        public static bool verify(SortedDictionary<string, string> sPara, string charset, string sign_type, string partner, string mapiUrl)
        {
            Notify aliNotify = new Notify(charset, sign_type, partner, mapiUrl);

            return aliNotify.Verify(sPara, sPara["notify_id"], sPara["sign"]);
        }
    }
}
