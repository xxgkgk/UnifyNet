var AdminLoginB = {
    createNew: function (success, error) {
        var n = {};
        // 验证
        n.valid = function (adminLogin) {
            var http = HttpB.createNew(UnInit.crossUrl("admin_login"));
            http.setOnSuccessListener(success);
            if (error != null) {
                http.setOnErrorListener(error);
            }
            var inxd = XmlData.createNew();
            inxd.AdminLogin = adminLogin;
            http.loadData(inxd);
        };
        return n;
    }
};
