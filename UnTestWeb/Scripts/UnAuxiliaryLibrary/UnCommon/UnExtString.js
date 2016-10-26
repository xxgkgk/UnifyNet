
String.prototype.TrimStart = function (trimStr) {
    if (!trimStr) { return this; }
    var temp = this;
    while (true) {
        if (temp.substr(0, trimStr.length) != trimStr) {
            break;
        }
        temp = temp.substr(trimStr.length);
    }
    return temp;
};

String.prototype.trimEnd = function (trimStr) {
    if (!trimStr) { return this; }
    var temp = this;
    while (true) {
        if (temp.substr(temp.length - trimStr.length, trimStr.length) != trimStr) {
            break;
        }
        temp = temp.substr(0, temp.length - trimStr.length);
    }
    return temp;
};
String.prototype.trim = function (trimStr) {
    var temp = trimStr;
    if (!trimStr) { temp = " "; }
    return this.trimStart(temp).trimEnd(temp);
};
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.ltrim = function () {
    return this.replace(/(^\s*)/g, "");
}
String.prototype.rtrim = function () {
    return this.replace(/(\s*$)/g, "");
}
String.prototype.byteLength = function () {
    var bytesCount = 0;
    for (var i = 0; i < this.length; i++) {
        var c = this.charAt(i);
        if (/^[\u0000-\u00ff]$/.test(c)) {
            bytesCount += 1;
        } else {
            bytesCount += 2;
        }
    }
    return (bytesCount);
}
String.prototype.isBetweenLength = function (min, max) {
    var length = this.byteLength();
    if (min != null && length < min) {
        return false;
    }
    if (max != null && length > max) {
        return false;
    }
    return true;
}

