var ValidCodeM = {
    createNew: function (success, error) {
        var n = {};
        // 验证
        n.valid = function (code) {
            var http = HttpM.createNew(UnInit.verCodeUrl("valid", code));
            http.setOnSuccessListener(success);
            if (error != null) {
                http.setOnErrorListener(error);
            }
            http.loadData("");
        };
        return n;
    }
};
