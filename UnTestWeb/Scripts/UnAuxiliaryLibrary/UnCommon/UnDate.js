var UnDate =
{
    getInt: function (Min, Max) {
        var Range = Max - Min;
        var Rand = Math.random();
        return (Min + Math.round(Rand * Range));
    },
    getRandom: function (length) {
        var str = "qwertyuiopasdfghjklmnbvcxz1234567890";
        var s = str.split("");
        var t = "";
        for (var i = 0; i < length; i++) {
            t += s[GetRandom_Num(1, 36)];
        }
        return t;
    },
    getUID: function () {
        return new Date().getTime() + UnInit.pid();
    },
    getStr: function (min, max) {
        //字符列表 
        var s1 = new Array("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9");
        var str = "";
        var rnum = this.getInt(min, max);
        for (var m = 0; m < rnum; m++) {
            var rnum1 = this.getInt(0, s1.length - 1);
            str += s1[rnum1];
        }
        return str;
    }
}