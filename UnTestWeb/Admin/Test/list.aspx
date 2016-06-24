<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Master/Site.Master" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="UnTestWeb.Admin.Test.list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Admin/Styles/permission.css" type="text/css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
<div class="location">
    <span>主菜单</span>
    &ensp;&gt;&ensp;
    <span>列表</span>   
</div>
<div class="line"></div>
<div class="content">
    <!--主体内容 start-->
    <div class="searchKeyWord">
    <input type="text" id="searchItem" />
    <div id="sureItem"></div>
    </div>
    <div style="clear :both">
    </div>
    <div class="add"><a href="/Admin/AutoReply/keywordAdd.aspx">添加</a></div>
    <table cellpadding="0" cellspacing="0" width="100%">
        <thead>
            <tr>
                <th width="6%">ID</th>
                <th width="30%">列1</th>
                <th width="30%">列2</th>
                <th width="15%">列3</th>
                <th width="19%">操作</th>
            </tr>
        </thead>
        <tbody id="pageBody" only="$ArrayOfWxAutoReply.WxAutoReplyID$" style="display:block;border:1px solid;">
            <tr>
                <td>$ID$</td>
                <td>$列1$</td>
                <td>$列2$</td>
                <td>$列3$</td>
                <td><a href="" class="editor">编辑</a><a href="#" class="delete" did="$ID$">删除</a></td>
            </tr>
        </tbody>
    </table>
    <!--主体内容 end-->
    <!--翻页 start-->
    <div class="page" id="pageTurn" numID="trunNum">
        <span>
            <a href="?p=$turnPrevious$&key=kw">上一页</a>
            <label id="trunNum" hover="page_hover">
            <a href="?p=$pages$&key=kw" class="$hover$">$pages$</a>
            </label>
            <a href="?p=$turnNext$&key=kw">下一页</a>
        </span>
        共有$totalRowCount$条数据，当前第$currentPage$页
    </div>
    <!--翻页 end-->
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">

    // 加载数据
    function loadPage() {
    }

    // 搜索关键字
    $("#sureItem").click(function () {
        if ($("#searchItem").val().trim().byteLength() > 20) {
            layer.msg("最多输入20个字节")
        } else {
            var keyWord = $("#searchItem").val();
            location.href = "?p=1&key=" + keyWord;
        }
    })

    // 更新ui
    function updateUI(data) {
        var body = page.pageBody("XmlData", data);
        var turn = page.pageTurn();
        turn = turn.replace(/kw/g, key);
        $("#pageBody").html(body);
        $("#pageBody").show()
        $("#pageTurn").html(turn);
        $("#pageTurn").show()
        $(".delete").click(function () {
            var did = $(this).attr("did");
            deleteID(did);
        });
    };

    // 删除
    function deleteID(did) {
        layer.confirm('删除后不可恢复,确定继续?', { icon: 3, title: '提示' }, function (index) {
        });
    };
</script>
</asp:Content>
