// 翻页类
var UnHref = {
    createNew: function (url) {
        if (url == null) {
            url = location.href;
        };
        var n = {};
        // 名+参;
        n.nameAttr = function () {
            var a = url.lastIndexOf("/");
            var b = url.substring(a + 1, url.length);
            return b;
        };
        // 名;
        n.name = function (chr) {
            if (chr == null) {
                chr = "?";
            };
            var s1 = this.nameAttr();
            var a = s1.indexOf(chr);
            if (a == -1) {
                a = s1.length;
            }
            var b = s1.substring(0, a);
            return b;
        };
        // 参数串
        n.attrStr = function (chr) {
            var s1 = this.nameAttr();
            var s2 = this.name(chr);
            var a = s1.indexOf(s2);
            var b = s1.substring(a + s2.length + 1, s1.length);
            return b;
        };
        // 参数组
        n.attrArray = function (chr, chr_split) {
            if (chr_split == null) {
                chr_split = "&";
            }
            var s1 = this.attrStr(chr);
            var arr = s1.split(chr_split);
            return arr;
        };
        // 参数
        n.attr = function (name, chr, chr_split, char_logo) {
            if (char_logo == null) {
                char_logo = "=";
            }
            var ary = this.attrArray(chr, chr_split);
            var b = null;
            ary.forEach(function (e) {
                var s = name + "" + char_logo;
                var a = e.indexOf(s);
                if (a == 0) {
                    b = e.substring(s.length, e.length);
                }
                return;
            });
            return b;
        };
        n.host = function () {
            var a = url.indexOf("://");
            var b = url.indexOf("/", a + 3);
            var c = url.substring(0, b);
            return c;
        };
        n.directory = function () {
            var s1 = this.host();
            var a = url.lastIndexOf("/");
            var b = url.substring(s1.length, a);
            return b;
        };
        n.path = function () {
            var a = this.directory() + "/" + this.name();
            return a;
        }
        return n;
    }
};
