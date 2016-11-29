var UnHttpClient = {
    createNew: function (url) {
        jQuery.support.cors = true;
        var n = {};
        n.url = url;
        n.timeOut = 5000;
        n.dataType = "html";
        n.type = "POST";
        n.success = function (rst) {
        };
        n.error = function (rst) {
        };
        n.setTimeOut = function (timeOut) {
            this.timeOut = timeOut;
        };
        n.setOnSuccessListener = function (success) {
            this.success = success;
        };
        n.setOnErrorListener = function (error) {
            this.error = error;
        };
        n.sendMsg = function (msg) {
            $.ajax({
                url: this.url,
                data: msg,
                dataType: this.dataType,
                type: this.type,
                crossDomain: true,
                timeout: this.timeOut,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    var rst = UnAttrRst.createNew();
                    rst.pid = UnInit.pid();
                    rst.code = -100;
                    rst.msg = "错误: " + textStatus + "," + errorThrown;
                    n.error(rst);
                },
                success: function (s) {
                    var rst = UnAttrRst.createNew();
                    rst.pid = 1;
                    rst.code = 1;
                    rst.msg = "连接成功";
                    rst.data = s;
                    n.success(rst);
                }
            });
        };
        var upCusor = 0;
        var upLength = -1;
        n.upFile = function (cName, dJson) {
            if (dJson == null) {
                dJson = { "type": 'Image' };
            }
            if (upLength == -1) {
                upLength = $("input[name^='" + cName + "']").length;
            }
            if (upCusor < upLength) {
                var id = cName + upCusor;
                $.ajaxFileUpload(
                   {
                       url: this.url,
                       secureuri: false,
                       data: dJson,
                       type: "post",
                       fileElementId: id,
                       success: function (data, status) {
                           var rst = UnXMMPXml.xmlDocToT("UnAttrRst", data);
                           if (rst != null) {
                               rst.pid = upCusor;
                               n.success(rst);
                               upCusor++;
                           }
                           else {
                               this.error(null, id + "-服务端异常", "配置错误或文件受限制");
                           }
                           n.upFile(cName);
                       },
                       error: function (data, status, e) {
                           var rst = UnAttrRst.createNew();
                           rst.pid = upCusor;
                           rst.code = -100;
                           rst.msg = status + "," + e;
                           n.error(rst);
                       }
                   }
               );
            }
        };
        return n;
    }
};
