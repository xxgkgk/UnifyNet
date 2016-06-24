// 翻页类
var UnPage = {
    createNew: function (bodyName, turnName) {
        // 内容模板
        var bodyTemp = $("#" + bodyName).html();
        var bodyOnly = $("#" + bodyName).attr("only")

        // 跳转模板
        var turnTemp = $("#" + turnName).html();
        // 页码ID
        var turnNumID = $("#" + turnName).attr("numID");
        // 页码模板
        var turnNumTemp = $("#" + turnNumID).html();
        // 页码选中状态
        var turnNumHover = $("#" + turnNumID).attr("hover");

        // 翻页数据
        var pageData = null;
        // 重写父类
        var rewriteParent = function (xml, name) {
            return xml;
        };
        // 重写数组类
        var rewriteArrayOf = function (obj, name, arrXml) {
            return arrXml;
        };
        // 重写属性
        var name0 = null;
        var rewriteField = function (obj, name, value, fName) {
            if (bodyOnly == ("$" + fName + "." + name + "$")) {
                var item = bodyTemp;
                for (var t in obj) {
                    var v = rewField(t, obj[t]);
                    item = item.replace(eval("/\\$" + t + "\\$/g"), v);
                }
                value = item;
            }
            else {
                value = "";
            }
            return value;
        };

        // 类实例
        var n = {};
        n.pageBody = function (name, data) {
            pageData = data;
            var s = tToHtml(name, data, "");
            return s;
        };
        var rewField = function (name, value) {
            return value;
        };
        n.setRewriteField = function (_rewField) {
            rewField = _rewField;
        };
        n.pageTurn = function () {
            var pageInfo = pageData.PageInfo;
            var currentPage = parseInt(pageInfo.CurrentPage);
            var turnPrevious = currentPage - 1;
            var turnNext = currentPage + 1;
            if (turnPrevious < 1) {
                turnPrevious = 1;
            }
            if (turnNext > pageInfo.TotalPage) {
                turnNext = pageInfo.TotalPage;
            }
            var turn = turnTemp;
            turn = turn.replace(eval("/\\$turnHome\\$/g"), 1);
            turn = turn.replace(eval("/\\$turnPrevious\\$/g"), turnPrevious);
            turn = turn.replace(eval("/\\$turnNext\\$/g"), turnNext);
            turn = turn.replace(eval("/\\$turnLast\\$/g"), pageInfo.TotalPage);
            turn = turn.replace(eval("/\\$currentPage\\$/g"), pageInfo.CurrentPage);
            turn = turn.replace(eval("/\\$totalPage\\$/g"), pageInfo.TotalPage);
            turn = turn.replace(eval("/\\$totalRowCount\\$/g"), pageInfo.TotalRowCount);

            var pageNum = "";
            var pageNumSteup = 4;
            var regPages = eval("/\\$pages\\$/g");
            var regHover = eval("/\\$hover\\$/g");
            // 页码前半部分
            var s0 = 0, s1 = currentPage;
            if (s1 > pageNumSteup) {
                s0 = s1 - pageNumSteup;
                pageNum += turnNumTemp.replace(regPages, 1);
                if (s0 > 1) {
                    pageNum += "…";
                }
            }

            turnNumTemp += "";
            for (var i = s0; i < s1; i++) {
                var pt = turnNumTemp.replace(regPages, i + 1);
                if (i == s1 - 1) {
                    pt = pt.replace(regHover, turnNumHover);
                }
                pageNum += pt;
            }

            // 页码后半部分
            var e0 = currentPage, e1 = e0 + 3;
            var totalPage = parseInt(pageInfo.TotalPage);
            var space = totalPage - e1;
            if (space < 0) {
                e1 = totalPage;
            }
            for (var i = e0; i < e1; i++) {
                pageNum += turnNumTemp.replace(regPages, i + 1);
            }
            if (space > 0) {
                if (space > 1) {
                    pageNum += "…";
                }
                pageNum += turnNumTemp.replace(regPages, totalPage);
            }

            turn = turn.replace(turnNumTemp, pageNum);
            return turn;
        };
        var pgXml = "";

        var tToHtml = function (tName, ts, fName) {
            var xml = "";
            for (var t in ts) {
                var type = typeof (ts[t]);
                switch (type) {
                    case "object":
                        var nts = t;
                        if (nts.indexOf(UnXMMPXml.arrayOf) != 0) {
                            var fValue = tToHtml(nts, ts[t], fName);
                            xml += fValue;
                        }
                        else {
                            nts = nts.substring(UnXMMPXml.arrayOf.length, nts.length);
                            var arrXml = "";
                            for (var i = 0; i < ts[t].length; i++) {
                                var item = ts[t][i];
                                var fValue = tToHtml(nts, item, t);
                                arrXml += fValue;
                            }
                            if (arrXml != "") {
                                xml += rewriteArrayOf(ts, t, arrXml);
                            }
                        }
                        break;
                    case "function":
                        break
                    default:
                        xml += rewriteField(ts, t, ts[t], fName);
                        break;
                }
            }
            if (xml != "") {
                xml = rewriteParent(xml, tName);
            }
            return xml;
        };
        return n;
    }
};
