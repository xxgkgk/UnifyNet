var UnBaiduMapApi = {
    createNew: function (dituID, Longitude, Latitude, Level) {
        var n = {};
        //在百度地图容器中创建一个地图
        var map = new BMap.Map("dituContent");
        //定义一个中心点坐标
        var point = new BMap.Point(Longitude, Latitude);
        n.initMap = function () {
            //设定地图的中心点和坐标并将地图显示在地图容器中
            map.centerAndZoom(point, Level);
            //设置地图事件
            setMapEvent();
            //向地图添加控件
            addMapControl();
            //添加标志
            addMarker();
        };
        var setMapEvent = function () {
            //启用地图拖拽事件，默认启用(可不写)
            map.enableDragging();
            //启用地图滚轮放大缩小
            map.enableScrollWheelZoom();
            //启用鼠标双击放大，默认启用(可不写)
            map.enableDoubleClickZoom();
            //启用键盘上下左右键移动地图
            map.enableKeyboard();
        };
        var addMapControl = function () {
            //向地图中添加缩放控件
            var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_LARGE });
            map.addControl(ctrl_nav);
            //向地图中添加缩略图控件
            var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 1 });
            map.addControl(ctrl_ove);
            //向地图中添加比例尺控件
            var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
            map.addControl(ctrl_sca);
        };

        n.setMarker = function (title, content, Longitude, Latitude, isOpen) {
            markerArr = [{ title: title, content: content, point: Longitude + "|" + Latitude, isOpen: isOpen, icon: { w: 21, h: 21, l: 0, t: 0, x: 6, lb: 5}}];
        };
        var markerArr = new Array();
        var addMarker = function () {
            for (var i = 0; i < markerArr.length; i++) {
                var json = markerArr[i];
                var p0 = json.point.split("|")[0];
                var p1 = json.point.split("|")[1];
                var point = new BMap.Point(p0, p1);
                var iconImg = createIcon(json.icon);
                var marker = new BMap.Marker(point, { icon: iconImg });
                var iw = createInfoWindow(i);
                var label = new BMap.Label(json.title, { "offset": new BMap.Size(json.icon.lb - json.icon.x + 10, -20) });
                marker.setLabel(label);
                map.addOverlay(marker);
                label.setStyle({
                    borderColor: "#808080",
                    color: "#333",
                    cursor: "pointer"
                });

                (function () {
                    var index = i;
                    var _iw = createInfoWindow(i);
                    var _marker = marker;
                    _marker.addEventListener("click", function () {
                        this.openInfoWindow(_iw);
                    });
                    _iw.addEventListener("open", function () {
                        _marker.getLabel().hide();
                    })
                    _iw.addEventListener("close", function () {
                        _marker.getLabel().show();
                    })
                    label.addEventListener("click", function () {
                        _marker.openInfoWindow(_iw);
                    })
                    if (!!json.isOpen) {
                        label.hide();
                        _marker.openInfoWindow(_iw);
                    }
                })()
            }
        };
        //创建InfoWindow
        var createInfoWindow = function (i) {
            var json = markerArr[i];
            var iw = new BMap.InfoWindow("<b class='iw_poi_title' title='" + json.title + "'>" + json.title + "</b><div class='iw_poi_content'>" + json.content + "</div>");
            return iw;
        }
        //创建一个Icon
        var createIcon = function (json) {
            var icon = new BMap.Icon("http://app.baidu.com/map/images/us_mk_icon.png", new BMap.Size(json.w, json.h), { imageOffset: new BMap.Size(-json.l, -json.t), infoWindowOffset: new BMap.Size(json.lb + 5, 1), offset: new BMap.Size(json.x, json.h) })
            return icon;
        }
        return n;
    }
};