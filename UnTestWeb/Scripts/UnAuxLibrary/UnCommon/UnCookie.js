var UnCookie = {
    setCookie: function (name, value, exp, path) {
        if (path == null) {
            path = "/";
        }
        var date = new Date();
        date.setTime(date.getTime() + exp * 1000);
        document.cookie = name + "=" + encodeURI(Value) + ";expires=" + date.toGMTString() + ";path=" + path;
    },
    getCookie: function (name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
        if (arr = document.cookie.match(reg)) {
            return (decodeURI(arr[2]));
        }
        else {
            return null;
        }
    },
    setCookies: function (className, array, exp, path) {
        var date = new Date();
        date.setTime(date.getTime() + exp * 1000);
        var str = className + "=";
        var i = 0;
        for (arr in array) {
            if (i == 0) {
                str += arr + "=" + encodeURI(array[arr]);
            }
            else {
                str += "&" + arr + "=" + encodeURI(array[arr]);
            }
            i++;
        }
        if (path == null) {
            path = "/";
        }
        str += ";expires=" + date.toGMTString() + ";path=" + path;
        document.cookie = str;
    },
    getCookies: function (className, name) {
        var search = className + "=";
        var CnValue = "";
        if (document.cookie.length > 0) {
            offset = document.cookie.indexOf(search);
            if (offset != -1) {
                offset += search.length;
                end = document.cookie.indexOf(";", offset);
                if (end == -1)
                    end = document.cookie.length;
                CnValue = decodeURI(document.cookie.substring(offset, end));
            }
        }
        var ReturnValue;
        if (name == "" || name == null) {
            ReturnValue = CnValue;
        }
        else {
            var re = eval("/&" + name + "=[^&]*/");
            var arr = ("&" + CnValue + "&").match(re);
            if (arr + "" == "") {
                ReturnValue = "";
            } else if (arr == null) {
                ReturnValue = "";
            }
            else {
                ReturnValue = arr[0].replace("&" + name + "=", "");
            }
        }
        return ReturnValue;
    },
    updateCookies: function (className, name, value, exp, path) {
        var date = new Date();
        if (exp == null) {
            exp = 120;
        }
        if (path == null) {
            path = "/";
        }
    
        date.setTime(date.getTime() + exp * 1000);
        var search = className + "=";
        var reg = new RegExp("(^|)" + className + "=[^;]*");
        
        // 全局字符串
        var cstr = document.cookie;
        var arr = cstr.match(reg);
        
        if (arr != null) {
            // 当前组字符串;
            var clstr = arr[0];
            // 名/值不能为空
            if (name != null && value != null) {
                // 头部正则;
                var re = eval("/[&|=]" + name + "=/");
                // 值头部;
                var cnstr = clstr.match(re);
                // 值全局正则;
                var re1 = eval("/" + cnstr + "[^&|^;]*/");
                // 旧值;
                var oldstr = clstr.match(re1);
                // 新值;
                var newstr = cnstr + encodeURI(value);
                // 替换
                clstr = clstr.replace(oldstr, newstr);
            }
            
            // 写入;
            document.cookie = clstr + ";expires=" + date.toGMTString() + ";path=" + path;
            return true;
        }
        else {
            return false;
        }
    },
    deleteCookies: function (className, path) {
        UnCookie.updateCookies(className, null, null, -1, path)
    }
}