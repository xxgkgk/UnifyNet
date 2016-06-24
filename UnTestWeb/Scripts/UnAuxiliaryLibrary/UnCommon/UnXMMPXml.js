var UnXMMPXml = {
    arrayOf: "ArrayOf",
    xmlToT: function (tName, str) {
        var xml;
        if ($.browser.msie) {
            xml = new ActiveXObject("Microsoft.XMLDOM");
            xml.async = false;
            xml.loadXML(xmlStr);
        } else {
            xml = new DOMParser().parseFromString(str, "text/xml");
        }
        return UnXMMPXml.xmlDocToT(tName, xml);
    },
    xmlDocToT: function (tName, xmlDoc, isArray) {
        if (isArray == null) {
            isArray = false;
        }
        var list = new Array();
        $(xmlDoc).find(tName).each(function () {
            var ts = eval(tName + ".createNew()");
            for (var t in ts) {
                var type = typeof (ts[t]);
                switch (type) {
                    case "object":
                        var nts = t;
                        var b = null;
                        if (nts.indexOf(UnXMMPXml.arrayOf) == 0) {
                            nts = nts.substring(UnXMMPXml.arrayOf.length, nts.length);
                            b = true;
                        }
                        // 回调
                        var fValue = UnXMMPXml.xmlDocToT(nts, this, b);
                        // 赋值
                        ts[t] = fValue;
                        break;
                    case "function":
                        break;
                    default:
                        var field = $(this);
                        //读取节点属性
                        //var fName = field.attr("Name");
                        //读取子节点的值
                        var dataType = field.find(t).text();
                        // 赋值
                        ts[t] = dataType;
                        break;
                }
            }
            list.push(ts);
        });
        if (!isArray) {
            return list[0];
        }
        return list;
    },
    tToXml: function (tName, ts) {
        var xml = "";
        for (var t in ts) {
            var type = typeof (ts[t]);
            switch (type) {
                case "object":
                    var nts = t;
                    if (nts.indexOf(UnXMMPXml.arrayOf) != 0) {
                        var fValue = UnXMMPXml.tToXml(nts, ts[t]);
                        xml += fValue;
                    }
                    else {
                        nts = nts.substring(UnXMMPXml.arrayOf.length, nts.length);
                        var arrXml = "";
                        for (var i = 0; i < ts[t].length; i++) {
                            var item = ts[t][i];
                            var fValue = UnXMMPXml.tToXml(nts, item);
                            arrXml += fValue;
                        }
                        if (arrXml != "") {
                            xml += "<" + t + ">" + arrXml + "</" + t + ">";
                        }
                    }
                    break;
                case "function":
                    break
                default:
                    xml += "<" + t + "><![CDATA[" + ts[t] + "]]></" + t + ">";
                    break;
            }
        }
        if (xml != "") {
            xml = "<" + tName + ">" + xml + "</" + tName + ">";
        }
        return xml;
    },
    xmlDocToString: function (xmlData) {
        var xmlString;
        //IE
        if (window.ActiveXObject) {
            xmlString = xmlData.xml;
        }
        else {
            xmlString = (new XMLSerializer()).serializeToString(xmlData);
        }
        return xmlString;
    }
};
