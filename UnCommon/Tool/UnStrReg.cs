using System.Text.RegularExpressions;

namespace UnCommon.Tool
{
    /// <summary>
    /// 正则表达式
    /// </summary>
    public class UnStrReg
    {
        /// <summary>
        /// 用户名(6~16位,[A-Z][a-z]_[0-9]组成,第一个字必须为字母)
        /// </summary>
        public static Regex regUserName = new Regex(@"^[a-zA-Z]\w{5,15}$");
        /// <summary>
        /// 密码(6-16位,[A-Z][a-z]_[0-9])
        /// </summary>
        public static Regex regPassWord = new Regex(@"^[a-zA-Z]\w{5,15}$");
        /// <summary>
        /// 强密码(必含字母数字,及特殊符号组成,8-16位)
        /// </summary>
        public static Regex regPassWord1 = new Regex(@"^(?=.*[a-zA-Z].*)(?=.*[0-9].*)[a-zA-Z\d_\~!@#\$%\^&\*()_\+]{8,16}$");
        /// <summary>
        /// 安全码(6位[A-Z][a-z]_[0-9])
        /// </summary>
        public static Regex regSecCode = new Regex(@"^([a-z]|\d|_){6}$");
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public static Regex regEmail = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        /// <summary>
        /// 手机号码
        /// </summary>
        public static Regex regCellPhoneNum = new Regex(@"^1[3,4,5,6,7,8,9]{1}\d{9}$");
        /// <summary>
        /// 时间格式
        /// </summary>
        public static Regex regDateTime = new Regex(@"^(\d{4,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$");
        /// <summary>
        /// 中文字符
        /// </summary>
        public static Regex regChinese = new Regex(@"^[\u4e00-\u9fa5]{0,}");
        /// <summary>
        /// 数字
        /// </summary>
        public static Regex regNumeric = new Regex(@"^\d+$");
        /// <summary>
        /// html标签
        /// </summary>
        public static Regex regHTML = new Regex(@"<[^>]*>");
        /// <summary>
        /// 电话只显示头尾
        /// </summary>
        public static Regex regPartPhoneNum = new Regex(@"(\d{3})\d{4}(\d{4})");
        /// <summary>
        /// 邮箱只显示部分
        /// </summary>
        public static Regex regPartEmail = new Regex(@"(.{2}).*(@{1})");
        /// <summary>
        /// 账户名只显示头尾
        /// </summary>
        public static Regex regPartUserName = new Regex(@"(.{2}).*(.{2})");
        /// <summary>
        /// 取出文件路径
        /// </summary>
        public static Regex regFilePath = new Regex(@"^.*/");
        /// <summary>
        /// URL
        /// </summary>
        public static Regex regUrl = new Regex(@"[a-zA-z]+://[^/s]*");
        /// <summary>
        /// 身份证
        /// </summary>
        public static Regex regIDCard = new Regex(@"/d{15}|/d{18}");
        /// <summary>
        /// IP地址
        /// </summary>
        public static Regex regIP = new Regex(@"/d+/./d+/./d+/./d+");
    }
}
