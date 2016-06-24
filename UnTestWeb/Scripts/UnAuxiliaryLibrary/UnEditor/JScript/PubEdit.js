var UpLoadFileUrl = "AspNet/UpLoad.aspx";
var href = window.location.href;
var ifid = href.match(/ifid=[^&]*/).toString().replace("ifid=", "");
var taid = href.match(/taid=[^&]*/).toString().replace("taid=", "").replace("#", "");
var editor;
editor = document.getElementById("htmledit").contentWindow;
editor.document.designMode = 'On';
editor.document.contentEditable = true;
editor.document.open();
editor.document.write("<style>body{font-size:14px;}</style>");
editor.document.close();
var Sys = {};
var ua = navigator.userAgent.toLowerCase();
var s; (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] : (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] : (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] : (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] : (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;
function dg(x, tp) {
	var dg1;
	if (tp == "id") {
		dg1 = document.getElementById(x)
	} else if (tp == "name") {
		dg1 = document.getElementsByName(x)
	}
	return dg1
}
function sublr(x) {
	var txtaid = window.parent.document.getElementById(taid);
	var wayv = dg("wayv", "id").value;
	var codeedit = dg("codeedit", "id");
	var editorb = editor.document.body;
	if (x == 0) {
	    editorb.innerHTML = "<div>" + txtaid.value + "</div>";
	    //editorb.innerHTML = txtaid.value;
	} else if (x == 1) {
		var lr = "";
		if (wayv == 0) {
			lr = lr + editorb.innerHTML
		} else {
			lr = lr + codeedit.value
		}
		txtaid.value = lr
	}
}
function effbut(a, x, y) {
	var but = dg(a, "id");
	var idx = new Array();
	idx = x.split(",");
	var idy = new Array();
	idy = y.split(",");
	var ft, str;
	for (i = 0; i < idx.length; i++) {
		if ("ff,fs,ffc,fbc,tb,img,face,media,file".indexOf(idx[i]) > -1) {
			ft = "swh_kg('" + idx[i] + "')"
		} else if ("modecode,modeedit".indexOf(idx[i]) > -1) {
			ft = "way('" + idx[i] + "')"
		} else if ("sizeminus,sizeplus".indexOf(idx[i]) > -1) {
			ft = "sizekz('" + idx[i] + "')"
		} else {
			ft = "effect('" + idx[i] + "',false,null)"
		}
		str = "<div class=\"but1_1\" id=\"" + idx[i] + "\" title=\"" + idy[i] + "\" onClick=\"" + ft + "\" onMouseDown=\"fomdu(this.id,0)\" onMouseUp=\"fomdu(this.id,1)\"><img src=\"images/but/" + idx[i] + ".gif\"/></div>";
		but.innerHTML += str
	}
}
function fomdu(x, y) {
	var id = dg(x, "id").style;
	if (y == 0) {
		id.border = "0";
		id.borderLeft = "2px solid #0cc";
		id.borderTop = "2px solid #0cc"
	}
	if (y == 1) {
		id.border = "1px solid #fff"
	}
}
function swh_kg(x) {
	var a = "ff,fs,ffc,fbc,tb,img,face,media,file";
	var idx = new Array();
	idx = a.split(",");
	for (i = 0; i < idx.length; i++) {
		var kgv = dg(idx[i] + "v", "id");
		var kgk = dg(idx[i] + "k", "id").style;
		if (idx[i] != x && kgv.value == 1) {
			kgv.value = 0;
			kgk.display = "none"
		} else if (idx[i] == x) {
			if (kgv.value == 0) {
				kgv.value = 1;
				kgk.display = "block"
			} else if (kgv.value == 1) {
				kgv.value = 0;
				kgk.display = "none"
			}
		}
	}
	swh_panel(x)
}
function swh_panel(x) {
	var id = dg(x + "k", "id");
	if (id.innerHTML != "") {
		return false
	}
	if (x == "ff") {
		var a = "SimSun,KaiTi_GB2312,SimHei,微软雅黑,Arial,Times New Roman,Courier New,Verdana,GulimCh,MS Gothic";
		var b = "宋体,楷体_GB2312,黑体,微软雅黑,Arial,Times-New-Roman,Courier-New,Verdana,GulimChe,MS Gothi";
		var ax = new Array();
		ax = a.split(",");
		var bx = new Array();
		bx = b.split(",");
		for (i = 0; i < ax.length; i++) {
			id.innerHTML += "<div><a href=\"#\" onClick=\"effect('fontname',false,'" + ax[i] + "')\"><font face=\"" + ax[i] + "\">" + bx[i] + "</font></a></div>"
		}
	} else if (x == "fs") {
		for (i = 1; i < 8; i++) {
			id.innerHTML += "<a href=\"#\" onClick=\"effect('fontsize',false," + i + ")\"><font size=\"" + i + "\">" + i + "号</font></a>"
		}
	} else if (x == "ffc" || x == "fbc") {
		var str = "#FF0000,#6B8E23,#FFFF00,#808080,#008000,#EE82EE,#FFC0CB,#4682B4,#FFFF93,#800080,#A52A2A,#CC0000,#800000,#FF8282,#000000,#8080FF,#8080C0,#FFA500,#FFFFFF,#0000FF";
		var strx = new Array();
		strx = str.split(",");
		var c;
		if (x == "ffc") {
			c = "forecolor"
		} else {
			c = "backcolor"
		}
		for (i = 0; i < strx.length; i++) {
			id.innerHTML += "<a href=\"#\"><div class=\"fck1\" style=\"background-color:" + strx[i] + ";\" onClick=\"effect('" + c + "',false,this.style.backgroundColor)\"></div></a>"
		}
	} else if (x == "tb") {
		id.innerHTML = "<table><tr><td>宽度：<input id=\"kd\" name=\"kd\" type=\"text\" value=\"350\" size=\"6\"></td><td>高度：<input id=\"gd\" name=\"gd\" type=\"text\" value=\"100\" size=\"6\"></td></tr><tr><td>行数：<input id=\"hs\" name=\"hs\" type=\"text\" value=\"5\" size=\"6\"></td><td>列数：<input id=\"ls\" name=\"ls\" type=\"text\" value=\"5\" size=\"6\"></td></tr><tr><td>边宽：<input id=\"bk\" name=\"bk\" type=\"text\" value=\"1\" size=\"6\"></td><td>边色：<input id=\"bs\" name=\"bs\" type=\"text\" value=\"#ccc\" size=\"6\"></td></tr><tr><td style=\"border-right:0;text-align:right;\"><input type=\"button\" onClick=\"int_tb()\" value=\"确定\">&nbsp;</td><td style=\"border-left:0;text-align:left;\">&nbsp;<input type=\"reset\" value=\"重置\"></td></tr></table>"
	} else if (x == "face") {
		id.innerHTML = "<div id=\"facenav\"><div id=\"facelb0\" onClick=\"swh_lb('face',0,2)\">常用</div><div id=\"facelb1\" onClick=\"swh_lb('face',1,2)\">梦幻</div><div id=\"facelb2\" onClick=\"swh_lb('face',2,2)\">兔斯基</div><div id=\"faceckg\" onClick=\"swh_kg('face')\">X</div></div><div id=\"facect\"><div id=\"facect0\"></div><div id=\"facect1\"></div><div id=\"facect2\"></div></div>";
		swh_lb(x, 0, 2)
	} else if (x == "img") {
		id.innerHTML = "<div id=\"imgnav\"><div id=\"imglb0\" onClick=\"swh_lb('img',0,1)\">本地</div><div id=\"imglb1\" onClick=\"swh_lb('img',1,1)\">网络</div><div id=\"imgkg\" onClick=\"swh_kg('img')\">X</div></div><div id=\"imgct\"><div id=\"imgct0\"><div class=\"imgct_1\">本地:</div><div class=\"imgct_2\"><input name=\"wj\" type=\"file\" onChange=\"wjcg('imgsv')\"></div></div><div id=\"imgct1\"><div class=\"imgct_1\">URL:</div><div class=\"imgct_2\"><input name=\"url\" id=\"url\" type=\"text\" value=\"http://\" size=\"30\"></div></div><div id=\"ylk\"><div class=\"ylk_1\"><input name=\"\" type=\"button\" value=\"预览\" onClick=\"yl_img()\"></div><div class=\"ylk_2\" id=\"yl\"></div></div><div id=\"ipt\"><input name=\"\" type=\"button\" value=\"确定\" onClick=\"int_img(document.getElementById('yl').innerHTML)\"> <input name=\"\" type=\"reset\" value=\"重置\"></div></div>";
		swh_lb(x, 0, 1);
		swh_ck(x)
	} else if (x == "media") {
		id.innerHTML = "<div id=\"medianav\"><div id=\"medialb0\" style='display:none;' onClick=\"swh_lb('media',0,2)\">本地</div><div id=\"medialb1\" onClick=\"swh_lb('media',1,2)\">网络</div><div id=\"medialb2\" onClick=\"swh_lb('media',2,2)\">网络</div><div id=\"mediakg\" onClick=\"swh_kg('media')\">X</div></div><div id=\"mediact0\"><div class=\"mediact_1\">本地:</div><div class=\"mediact_2\"><input name=\"wjmedia\" type=\"file\" onChange=\"wjcg('mediasv')\"></div></div><div id=\"mediact1\"><div class=\"mediact_1\">URL:</div><div class=\"mediact_2\"><input name=\"mediaurl\" type=\"text\" size=\"\"></div></div><div id=\"mediact2\"><div class=\"mediact_1\">HTML:</div><div class=\"mediact_2\"><input name=\"mediahtml\" type=\"text\" size=\"\"></div></div><div id=\"ipt\"><input name=\"\" type=\"button\" value=\"确定\" onClick=\"int_media()\"> <input name=\"\" type=\"reset\" value=\"重置\"></div>";
		swh_lb(x, 2, 2)
	} else if (x == "file") {
		id.innerHTML = "<div id=\"filenav\"><div id=\"filelb0\" onClick=\"swh_lb('file',0,1)\">本地</div><div id=\"filelb1\" style='display:none;' onClick=\"swh_lb('file',1,1)\">网络</div><div id=\"filekg\" onClick=\"swh_kg('file')\">X</div></div><div id=\"filect\"><div id=\"filect0\"><div class=\"filect_1\">本地:</div><div class=\"filect_2\"><input name=\"wjfile\" type=\"file\" onChange=\"wjcg('filesv')\"></div></div><div id=\"filect1\"><div class=\"filect_1\">URL:</div><div class=\"filect_2\"><input name=\"\" type=\"text\"></div></div></div><div id=\"ipt\"><input name=\"\" type=\"button\" value=\"确定\" onClick=\"int_file()\"> <input name=\"\" type=\"reset\" value=\"重置\"></div>";
		swh_lb(x, 0, 1)
	}
}
function swh_lb(id, m, n) {
	var ida, idb, idm;
	for (i = 0; i < n + 1; i++) {
		var ida = dg(id + "lb" + i, "id").style;
		var idb = dg(id + "ct" + i, "id").style;
		ida.backgroundColor = "";
		ida.color = "";
		ida.fontWeight = "";
		idb.display = "none";
		if (i == m) {
			ida.backgroundColor = "#FFF";
			ida.color = "#F00";
			ida.fontWeight = "bold";
			idb.display = "block"
		}
	}
	if (id == "face") {
		idm = dg(id + "ct" + m, "id");
		if (m == 0 && idm.innerHTML == "") {
			var str = "face_mh/1.gif,face_mh/2.gif,face_mh/3.gif,face_mh/11.gif,face_mh/15.gif,face_mh/17.gif,face_mh/18.gif,face_mh/29.gif,face_mh/41.gif,face_mh/69.gif,face_mh/74.gif,face_mh/77.gif,face_mh/82.gif,face_mh/89.gif,face_mh/91.gif,face_tsj/5.gif,face_tsj/7.gif,face_tsj/12.gif,face_tsj/15.gif,face_tsj/18.gif,face_tsj/21.gif,face_tsj/22.gif,face_tsj/23.gif,face_tsj/24.gif,face_tsj/26.gif,face_tsj/27.gif";
			var strx = new Array();
			strx = str.split(",");
			for (i = 0; i < strx.length; i++) {
				idm.innerHTML += "<img onClick=\"int_face('<img src='+this.src+'>')\" src=\"images/" + strx[i] + "\"/>"
			}
		} else if (m == 1 && idm.innerHTML == "") {
			for (i = 0; i < 104; i++) {
				idm.innerHTML += "<img onClick=\"int_face('<img src='+this.src+'>')\" src=\"images/face_mh/" + i + ".gif\"/>"
			}
		} else if (m == 2 && idm.innerHTML == "") {
			for (i = 0; i < 30; i++) {
				idm.innerHTML += "<img onClick=\"int_face('<img src='+this.src+'>')\" src=\"images/face_tsj/" + i + ".gif\"/>"
			}
		}
	} else if (id == "img" || id == "media" || id == "file") {
		dg(id + "lbv", "id").value = m
	}
}
function effect(x, y, z) {
	var jdlr;
	editor.focus();
	if (x == "createlink" || x == "unlink") {
		editor.document.execCommand(x)
	} else {
		editor.document.execCommand(x, y, z)
	}
	if (x == "fontsize") {
		swh_kg("fs")
	} else if (x == "fontname") {
		swh_kg("ff")
	} else if (x == "forecolor") {
		swh_kg("ffc")
	} else if (x == "backcolor") {
		swh_kg("fbc")
	}
}
function int_x(str1) {
	if (Sys.ie) {
		editor.focus();
		var range = editor.document.selection.createRange();
		range.pasteHTML(str1)
	} else if (Sys.firefox) {
		var sel = editor.getSelection(),
		rng = sel.getRangeAt(0),
		frg = rng.createContextualFragment(str1);
		rng.insertNode(frg)
	} else {
		alert("不支持此浏览器！");
		return false
	}
}
function int_tb() {
	var kd = dg("kd", "id");
	var gd = dg("gd", "id");
	var hs = dg("hs", "id");
	var ls = dg("ls", "id");
	var bk = dg("bk", "id");
	var bs = dg("bs", "id");
	var tdk, tdg, std, str, stb;
	tdk = kd.value / ls.value;
	tdg = gd.value / hs.value;
	std = "";
	str = "";
	for (i = 0; i < ls.value; i++) {
		std += "<td style=\"border:" + bk.value + "px solid " + bs.value + ";width:" + tdk + "px;height:" + tdg + "px;\"> </td>"
	}
	for (n = 0; n < hs.value; n++) {
		str += "<tr>" + std + "</tr>"
	}
	stb = "<div style=\"width:" + kd.value + "px;height:" + gd.value + "px;\"><table style=\"width:100%;height:100%;text-align:center;border-collapse:collapse;\">" + str + "</table></div>";
	int_x(stb);
	swh_kg("tb")
}
function int_face(x) {
	int_x(x);
	swh_kg("face")
}
function wjcg(x) {
	dg(x, "id").value = 1
}
function yl_img() {
	var imglbv = dg("imglbv", "id");
	var yl = dg("yl", "id");
	var imgsv = dg("imgsv", "id");
	if (imglbv.value == 0 && imgsv.value == 1) {
		dg("form1", "id").action = UpLoadFileUrl + "?type=img";
		form1.submit();
		imgsv.value = 0
	} else if (imglbv.value == 1) {
		var urlv = dg("url", "id").value;
		if (urlv == "http://" || urlv == "") {
			alert("请填写图片完整url地址!");
			return false
		}
		yl.innerHTML = "<img src=\"" + urlv + "\"/>"
	}
}
function int_img(x) {
	if (x == "") {
		alert("没有预览！");
		return false
	}
	int_x(x);
	swh_kg('img')
}
function int_media() {
	var medialbv = dg("medialbv", "id");
	var mediasv = dg("mediasv", "id");
	if (medialbv.value == 0 && mediasv.value == 1) {
		dg("form1", "id").action = UpLoadFileUrl + "?type=media";
		form1.submit();
		mediasv.value = 0
	} else if (medialbv.value == 1) {
		var urlv = dg("mediaurl", "id").value;
		var str = "<object id=\"player\"  width=\"60%\" height=\"60%\" classid=\"CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6\"><param name=\"autostart\" value=\"-1\"><param name=\"enabled\" value=\"-1\"><param name=\"url\" value=\"" + urlv + "\"><param name=\"uiMode\" value=\"mini\"></object>";
		int_x(str)
	} else if (medialbv.value == 2) {
		var htmlv = dg("mediahtml", "id").value;
		if (htmlv.indexOf("script") > 0 || htmlv.indexOf("embed") < 0) {
			alert("请输入正确格式!");
			return false
		}
		int_x(htmlv)
	}
	swh_kg("media")
}
function int_file() {
	var filelbv = dg("filelbv", "id");
	var filesv = dg("filesv", "id");
	if (filelbv.value == 0 && filesv.value == 1) {
		dg("form1", "id").action = UpLoadFileUrl + "?type=file";
		form1.submit();
		filesv.value = 0
	} else if (filelbv.value == 1) {}
	swh_kg("file")
}
function way(x) {
	var editbox = dg("editbox", "id").style;
	var codebox = dg("codebox", "id").style;
	var wayv = dg("wayv", "id");
	var htmledit = editor.document.body;
	var codeedit = dg("codeedit", "id");
	if (x == "modecode") {
		editbox.display = "none";
		codebox.display = "block";
		if (wayv.value == 0) {
			codeedit.value = htmledit.innerHTML;
			wayv.value = 1
		}
	} else if (x == "modeedit") {
		codebox.display = "none";
		editbox.display = "block";
		if (wayv.value == 1) {
			htmledit.innerHTML = codeedit.value;
			wayv.value = 0
		}
	}
	sublr(1)
}
function sizekz(x) {
	var htmledit = dg("htmledit", "id");
	var lxledit = window.parent.document.getElementById(ifid);
	if (x == "sizeminus" && htmledit.offsetHeight > 300) {
		htmledit.style.height = htmledit.offsetHeight - 202 + "px";
		lxledit.style.height = lxledit.offsetHeight - 200 + "px"
	} else if (x == "sizeplus") {
		htmledit.style.height = htmledit.offsetHeight + 200 + "px";
		lxledit.style.height = lxledit.offsetHeight + 202 + "px"
	}
} 