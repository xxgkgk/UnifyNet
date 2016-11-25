function getOs() {
    var OsObject = "";
    var appV = navigator.appVersion;
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        if (appV.match(/MSIE 6./i) != null) {
            return "IE6";
        }
        else if (appV.match(/MSIE 7./i) != null) {
            return "IE7";
        }
        else if (appV.match(/MSIE 8./i) != null) {
            return "IE8";
        }
        else if (appV.match(/MSIE 9./i) != null) {
            return "IE9";
        }
        else {
            return "IE";
        }
    }
    if (isFirefox = navigator.userAgent.indexOf("Firefox") > 0) {
        return "Firefox";
    }
    if (isSafari = navigator.userAgent.indexOf("Safari") > 0) {
        return "Safari";
    }
    if (isCamino = navigator.userAgent.indexOf("Camino") > 0) {
        return "Camino";
    }
    if (isMozilla = navigator.userAgent.indexOf("Gecko/") > 0) {
        return "Gecko";
    }
}