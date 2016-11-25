var UnStrReg =
{
    regUserName: /^[a-zA-Z]\w{5,15}$/,
    regPassWord: /^[a-zA-Z]\w{5,15}$/,
    regPassWord1: /^(?=.*[a-zA-Z].*)(?=.*[0-9].*)[a-zA-Z\d_\~!@#\$%\^&\*()_\+]{8,16}$/,
    regSecCode: /^([a-z]|\d|_){6}$/,
    regEmail: /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/,
    regCellPhoneNum: /^1\d{10}$/,
    regDateTime: /^(\d{4,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/,
    regChinese: /^[\u4e00-\u9fa5]{0,}/,
    regNumeric: /^\d+$/,
    regHTML: /<[^>]*>/,
    regPartPhoneNum: /(\d{3})\d{4}(\d{4})/,
    regPartEmail: /(.{2}).*(@{1})/,
    regPartUserName: /(.{2}).*(.{2})/,
    regFilePath: /^.*/,
    regRate: /^[1-9]+[0-9]*]*$/,
    getByteLength: function (input) {
        return input.replace(/[^\u0000-\u00ff]/g, "aa").length;
    }
}