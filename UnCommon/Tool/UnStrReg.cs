using System.Text.RegularExpressions;

namespace UnCommon.Tool
{
    /// <summary>
    /// 正则表达式
    /// </summary>
    public class UnStrReg
    {
        //用户名(6~16位,[A-Z][a-z]_[0-9]组成,第一个字必须为字母)
        public static Regex regUserName = new Regex(@"^[a-zA-Z]\w{5,15}$");
        //密码(6-16位,[A-Z][a-z]_[0-9])
        public static Regex regPassWord = new Regex(@"^[a-zA-Z]\w{5,15}$");
        //强密码(必含字母数字,及特殊符号组成,8-16位)
        public static Regex regPassWord1 = new Regex(@"^(?=.*[a-zA-Z].*)(?=.*[0-9].*)[a-zA-Z\d_\~!@#\$%\^&\*()_\+]{8,16}$");
        //安全码(6位[A-Z][a-z]_[0-9])
        public static Regex regSecCode = new Regex(@"^([a-z]|\d|_){6}$");
        //电子邮箱
        public static Regex regEmail = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        //手机号码
        public static Regex regCellPhoneNum = new Regex(@"^1[3,4,5,8]{1}\d{9}$");
        //时间格式
        public static Regex regDateTime = new Regex(@"^(\d{4,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$");
        //中文字符
        public static Regex regChinese = new Regex(@"^[\u4e00-\u9fa5]{0,}");
        //数字
        public static Regex regNumeric = new Regex(@"^\d+$");
        //html标签
        public static Regex regHTML = new Regex(@"<[^>]*>");
        //电话只显示头尾
        public static Regex regPartPhoneNum = new Regex(@"(\d{3})\d{4}(\d{4})");
        //邮箱只显示部分
        public static Regex regPartEmail = new Regex(@"(.{2}).*(@{1})");
        //账户名只显示头尾
        public static Regex regPartUserName = new Regex(@"(.{2}).*(.{2})");
        //取出文件路径
        public static Regex regFilePath = new Regex(@"^.*/");
        // URL
        public static Regex regUrl = new Regex(@"[a-zA-z]+://[^/s]*");
        // 身份证
        public static Regex regIDCard = new Regex(@"/d{15}|/d{18}");
        // IP地址
        public static Regex regIP = new Regex(@"/d+/./d+/./d+/./d+");
    }
}
