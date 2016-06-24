<script type="text/javascript">
function int_x(x){
var editor
editor=window.parent.document.getElementById("htmledit").contentWindow;
editor.focus();
var range=editor.document.selection.createRange();
range.pasteHTML(x);
}
</script>
<!--#include file="lxlup.asp"-->
<%
tp=Request.QueryString("type")
fpath="../upfile/"
Select Case tp
 Case "img"
  fsize=200*1024
  ftype="gif,jpg,png" 
  fname=getup("wj",1)
 Case "file"
  fsize=20000*1024
  ftype="doc,dot,xls,rar,zip,txt"
  fname=getup("wjfile",1)
 Case Else
  Call alert("未知类型!","0")
  Response.End()
End Select
Select Case fname
 Case "nosz"
  Response.Write("<script>alert('必须小于"&fsize/1024&"M!');</script>")
  Response.End()
 Case "notp"
  Response.Write("<script>alert('只支持"&ftype&"!');</script>")
  Response.End()
End Select
Select Case tp
 Case "img"
  Response.Write("<script>window.parent.document.getElementById('yl').innerHTML='<img src=upfile/"&fname&">';</script>")
 Case "file"
  flg=frpl(fname,".*\.","")
  str="<a href=upfile/"&fname&"><img src=images/filelogo/"&flg&".gif border=0></a>"
  Response.Write("<script>int_x('"&str&"');</script>")
End Select
%>

 