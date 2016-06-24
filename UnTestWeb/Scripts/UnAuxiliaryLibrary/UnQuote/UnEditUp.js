var UnEditUp = {
    upImage: function (clickID, fileID, showID_text, showID_img) {
        var n = {};
        var success_http = function (success) {
            if (success.code > 0) {
                var bi = UnXMMPXml.xmlToT("BackInfo", success.back);
                $("#" + showID_text).val(bi.Url);
                $("#" + showID_img).attr("src", bi.Url);
                $("#" + showID_img).show();
            }
            $("#" + clickID).attr('disabled', false)
            $("#" + clickID).val("上传");
            layer.msg(success.msg);
        };
        var error_http = function (error) {
            layer.msg(error.msg);
            $("#" + clickID).attr('disabled', false)
            $("#" + clickID).val("上传");
            layer.msg(error.msg);
        };
        var http = UnHttpClient.createNew(UnInit.uploadUrl("", "Image"));
        http.setOnSuccessListener(success_http);
        http.setOnErrorListener(error_http);
        http.upFile(fileID);
        $("#" + clickID).attr('disabled', true);
        $("#" + clickID).val("正在上传…");
    },
    upFile: function (clickID, fileID, showID_text) {
        var n = {};
        var success_http = function (success) {
            if (success.code > 0) {
                var bi = UnXMMPXml.xmlToT("BackInfo", success.back);
                $("#" + showID_text).val(bi.UNCode);
            }
            $("#" + clickID).attr('disabled', false)
            $("#" + clickID).val("上传");
            layer.msg(success.msg);
        };
        var error_http = function (error) {
            layer.msg(error.msg);
            $("#" + clickID).attr('disabled', false)
            $("#" + clickID).val("上传");
            layer.msg(error.msg);
        };
        var http = UnHttpClient.createNew(UnInit.uploadUrl("", "File"));
        http.setOnSuccessListener(success_http);
        http.setOnErrorListener(error_http);
        http.upFile(fileID);
        $("#" + clickID).attr('disabled', true);
        $("#" + clickID).val("正在上传…");
    }
};