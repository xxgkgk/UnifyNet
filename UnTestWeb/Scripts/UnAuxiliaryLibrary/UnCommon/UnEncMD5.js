var UnEncMD5 =
{
    getMd5Hash: function (str) {
        return $.md5(str);
    },
    getMd5Hashs: function (str) {
        return $.md5($.md5(str));
    }
}