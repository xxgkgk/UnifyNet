<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Master/Site.Master" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="UnTestWeb.Admin.Test.edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Admin/Styles/edit.css" type="text/css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
<div class="location">
    <span>主菜单</span>
    &ensp;&gt;&ensp;
    <a href="/admin/news_list_1">子菜单</a>
    &ensp;&gt;&ensp;
    <span>编辑</span>
</div>
<div class="line"></div>
<div class="content">
    <div class="edit">
        <ul class="ct">
            <li><span class="title">标题：</span><input id="Name" class="text" type="text" /><span class="error" id="Name_error">* 请填写标题</span></li>
            <li><span class="title">关键词：</span><input id="KeyWords" class="text" type="text" /><span class="error" id="KeyWords_error">* 请填写关键词</span></li>
            <li><span class="title">描述：</span><textarea rows="3" id="Description" class="textarea"></textarea></li>
            <li class="error" id="Description_error"><span class="title"></span><span>* 描述不能为空且不得超过100字</span></li>


            <li><span class="title">大图：</span><input class="text" type="file" id="upImg0" name="upImg0" /> <input class="button" type="button" value="上传图片" id="btImg" />&nbsp;&nbsp;</li>
            <li><span class="title"></span><input id="ImagePath" class="lable" type="text" readonly="readonly" /><span class="error" id="ImagePath_error">* 请上传图片</span></li>
            <li><span class="title"></span><img width="300" id="ImagePath_img"/></li>

            <li><span class="title">文件：</span><input class="text" type="file" id="upFile0" name="upFile0" /> <input class="button" type="button" value="上传文件" id="btFile" />&nbsp;&nbsp;</li>
            <li><span class="title"></span><input id="FilePath" class="lable" type="text" readonly="readonly" /><span class="error" id="FilePath_error">* 请上传文件</span></li>

            <li>
            <span class="title">内容：</span>
            <script id="editor" class="editor" type="text/plain" style="width:700px;height:350px;" ></script>
            </li>
            <li><span class="title">作者：</span><input id="InsertPerson" class="lable" type="text" value="管理员" readonly="readonly" /></li>
            <li><span class="title">日期：</span><input id="InsertDate" class="lable" type="text" readonly="readonly" /></li>
        </ul>
        <ul class="op">
            <li><input id="reset" type="button" value="重置" /></li>
            <li><input id="save" type="button" value="保存" /></li>
        </ul>
    </div>
</div> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
   
    var ue = UE.getEditor('editor');

    // 上传图片
    $("#btImg").click(function () {
        UnEditUp.upImage("btImg", "upImg", "ImagePath", "ImagePath_img");
    });

    // 上传文件
    $("#btFile").click(function () {
        UnEditUp.upFile("btFile", "upFile", "FilePath");
    });
</script>
</asp:Content>
