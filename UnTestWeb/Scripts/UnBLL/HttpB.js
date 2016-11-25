var HttpB = {
    createNew: function (url) {
        var n = {};
        n.success = function (xd) {
        };
        n.error = function (rst) {
            alert(rst.msg);
        };
        n.setOnSuccessListener = function (success) {
            this.success = success;
        };
        n.setOnErrorListener = function (error) {
            this.error = error;
        };
        n.msg = "";
        n.rst = "";
        // 数据加载
        n.loadData = function (xd) {
            this.msg = UnXMMPXml.tToXml("XmlData", xd);
            var http = UnHttpClient.createNew(url);
            http.setTimeOut(5000);
            http.dataType = "xml";
            http.setOnSuccessListener(httpSuccess);
            http.setOnErrorListener(httpError);
            http.sendMsg(this.msg);
            // 成功
            function httpSuccess(rst) {
                this.rst = rst;
                var xd = UnXMMPXml.xmlDocToT("XmlData", rst.data);
                n.success(xd);
            };
            // 失败
            function httpError(rst) {
                this.rst = rst;
                n.error(rst);
            };
        };
        return n;
    }
};
