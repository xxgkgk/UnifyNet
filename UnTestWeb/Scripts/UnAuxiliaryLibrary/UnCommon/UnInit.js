var UnInit = {
    init: function () {
        this._pid = new Date().getTime();
    },
    pid: function () {
        this._pid++;
        return this._pid;
    },
    crossUrl: function (key) {
        return "/handle/cross.ashx?key=" + key;
    },
    uploadUrl: function (key, type) {
        return "/handle/upload.ashx?key=" + key + "&type=" + type;
    },
    verCodeUrl: function (key, code) {
        var url = "/handle/vercode.ashx?key=" + key + "&code=" + code + "&time=" + new Date().getMilliseconds();
        //alert(url);
        return url;
    }
};
UnInit.init();
