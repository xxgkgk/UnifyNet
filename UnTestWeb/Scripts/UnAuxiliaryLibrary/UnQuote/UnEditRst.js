var UnEditRst = {
    apiNote: function (msg_suc, msg_err, fun_suc, fun_err) {
        var n = function (data) {
            var apiNote = data.ApiNote;
            if (msg_suc == null) {
                msg_suc = apiNote.NoteMsg;
            }
            if (msg_err == null) {
                msg_err = apiNote.NoteMsg;
            }
            if (apiNote.NoteCode > 0) {
                if (msg_suc != "") {
                    layer.msg(msg_suc);
                }
                if (fun_suc != null) {
                    fun_suc(data);
                }
            }
            else {
                if (msg_err != "") {
                    layer.msg(msg_err);
                }
                if (fun_err != null) {
                    fun_err(data);
                }
            }
        };
        return n;
    },
    unAttrRst: function (msg_suc, msg_err, fun_suc, fun_err) {
        var n = function (rst) {
            if (msg_suc == null) {
                msg_suc = rst.msg;
            }
            if (msg_err == null) {
                msg_err = rst.msg;
            }
            if (rst.code > 0) {
                if (msg_suc != "") {
                    layer.msg(msg_suc);
                }
                if (fun_suc != null) {
                    fun_suc(rst);
                }
            }
            else {
                if (msg_err != "") {
                    layer.msg(msg_err);
                }
                if (fun_err != null) {
                    fun_err(rst);
                }
            }
        };
        return n;
    }
};