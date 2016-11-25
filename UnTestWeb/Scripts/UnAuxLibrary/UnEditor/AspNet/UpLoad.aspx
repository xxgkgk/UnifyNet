<%@ Page Language="C#" AutoEventWireup="true" Inherits="_UpLoad" Codebehind="UpLoad.aspx.cs" %>
<html>
<head>
<title>AspNet上传</title>
<script type="text/javascript">
    function int_x(x) {
        var editor;
        editor = window.parent.document.getElementById("htmledit").contentWindow;
        editor.focus();
        var range = editor.document.selection.createRange();
        range.pasteHTML(x);
    }
</script>
</head>
<body>
</body>
<script type="text/javascript">
    if (str) {
        int_x(str); 
     }
</script>
</html>
