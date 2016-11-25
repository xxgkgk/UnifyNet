<%
'李晓龙无组件上传
Function StrToBin(str)'字符串转二进制
dim curChr, curAsc, low, high
dim i
for i=1 To Len(str)
    curChr = Mid(str, i, 1)
    curAsc = Asc(curChr)
    if curAsc < 0 then
       curAsc = curAsc + 65535
    end if
    if curAsc > 255 then
       low = Left(Hex(Asc(curChr)), 2)
       high = Right(Hex(Asc(curChr)), 2)
       StrToBin = StrToBin & ChrB("&H" & low) & ChrB("&H" & high)
     else
       StrToBin = StrToBin & ChrB(AscB(CurChr)) 
     end If
    next
End function
Function BinToStr(binStr)'二进制转换为字符串
    if IsNull(binStr) then
        BinToStr = ""
        exit function
    end if    
    dim newStr, chnFlag
    dim i, c
    newStr = ""
    chnFlag = true
    for i=1 To LenB(binStr)
        if chnFlag then
            c = MidB(BinStr, i, 1)
            if AscB(c) > 127 then
                newStr = newStr & Chr(AscW(MidB(binStr,i+1,1)&c))
                chnFlag = false
            else
                newStr = newStr & Chr(AscB(c)) '这里用的是 ASCB 不是 ASCW
            end If
        else
            chnFlag = true
        end If
    next   
    BinToStr = newStr
End function 

Function frpl(x,a,b)
 Dim re,re1
 set re=new regExp'建立正则表达式对象
 re.IgnoreCase=true'忽略大小写 
 re.Global=true'设置全程可用性
 re.Pattern=a
 re1=re.replace(x,b)
 frpl=re1
End Function

'文件扩展名类型
Function kztp(kzm)
 Select Case kzm
  Case "gif"
   kztp="image/gif"
  Case "jpe","jpg","jpeg"
   kztp="image/jpeg,image/pjpeg"
  Case "png"
   kztp="image/png,image/x-png"
  Case "bmp"
   kztp="image/bmp,image/x-ms-bmp"
  Case "txt"
   kztp="text/plain"
  Case "doc","dot"
   kztp="application/msword"
  Case "xls"
   kztp="application/vnd.ms-excel,application/excel"
  Case "ppt"
   kztp="application/vnd.ms-powerpoint"
  Case "pdf"
   kztp="application/pdf"
  Case "rar","zip"
   kztp="application/octet-stream,application/zip"
  Case "ra"
   kztp="audio/x-pn-realaudio"
  Case "rm"
   kztp="application/vnd.rn-realmedia"
  Case "ram"
   kztp="application/x-pn-realaudio"
  Case "mp2","mp3","mpa","mpe","mpeg","mpg"
   kztp="audio/mpeg"
  Case "wma"
   kztp="audio/x-ms-wma"
  Case "wmv"
   kztp="video/x-ms-wmv"
  Case "avi"
   kztp="video/x-msvideo"
  Case "swf"
   kztp="application/x-shockwave-flash"
  Case Else
   kztp=kzm
 End Select
End Function

Dim frt,hh,fg,fgn,fsize,ftype,fpath
frt=Request.BinaryRead(Request.TotalBytes)'获取二进制信息
hh=chrB(13)&chrB(10)
If LenB(frt)>0 Then
fg=LeftB(frt,Clng(Instrb(frt,hh)-1))
fgn=LeftB(frt,Clng(Instrb(frt,hh)+39))
End If

Function getup(fn,fs)
Dim fstr,fhh,flg,flgn,fsz,ftp,fph
 fstr=frt'文件信息
 fhh=hh'换行符
 flg=fg'文件随机标识
 flgn=fgn'表单名匹配字符串
 fsz=fsize'文件大小,留空不限制
 ftp=ftype'文件类型,留空不限制
 fph=fpath'存放路径,留空保存在当前
Dim tlg,tk1,tlr1,tlr2,tkb
 tlg=flgn&strtobin(fn&"""")'表单标识
 tk1=instrb(fstr,tlg)'表单开始位
 '**表单存在判断**LG1
 If tk1=0 Then
  getup=""
 Else
  tlr1=instrb(tk1,fstr,fhh&fhh)+3'内容开始位
  tlr2=instrb(tlr1,fstr,flg)'内容结束位
  tkb=tlr2-tlr1-3'内容大小
  '**判定表单(0)/文件(1)并获取**LG2
  If fs=0 Then
   getup=bintostr(midb(fstr,tlr1+1,tkb))
  ElseIf fs=1 Then
   '**内容大小判断**LG3
   If tkb>0 Then
    If fsz="" Or tkb<=fsz Then
	 Dim fn1,fn2,fnm,kzm,ktp,ary,i,n
	  fn1=instrb(tk1,fstr,strtobin("filename"))+10'文件名开始位
	  fn2=instrb(fn1,fstr,strtobin(""""))'文件名结束位
	  fnm=bintostr(midb(fstr,fn1,fn2-fn1))'文件名
	  kzm=frpl(fnm,".*\.","")'扩展名
	  ktp=bintostr(midb(fstr,fn2+17,tlr1-fn2-17-3))'文件类型
	  ary=Split(ftp,",")'类型检测
	  n=0
	  For i=0 To Ubound(ary)
	   If Instr(kztp(ary(i)),ktp)>0 And Instr(ftp,kzm)>0 Then
	    n=1
	    Exit For
	   End If
	  Next
	  If ftp="" Or n=1 Then'保存文件
	   Dim stm1,stm2,svn,fso
	   Randomize
	   svn=Cstr(year(now())&month(now())&day(now())&hour(now())&minute(now())&second(now()))&"-"&int((100+1)*rnd)
	   Set fso=Server.Createobject("Scripting.FileSystemObject")
	    If Not fso.FolderExists(Server.MapPath(fph)) Then
		 fso.CreateFolder(Server.MapPath(fph))
		End If
	   Set fso=Nothing
	   Set stm1=CreateObject("Adodb.Stream")
        stm1.Type=1
        stm1.Open
        stm1.Write fstr
		stm1.Position=tlr1
	   Set stm2=CreateObject("Adodb.Stream")
	    stm2.Type=1
        stm2.Open
		stm1.CopyTo stm2,tkb
		stm2.SaveToFile Server.mappath(fph&svn&"."&kzm),2
		stm1.Close
		stm2.Close
	   Set stm1=Nothing
	   Set stm2=Nothing	
       getup=svn&"."&kzm
	  Else
	   getup="notp"
	  End If
	Else
	 getup="nosz"
	End If
   End If
   '**LG3
  End If
  '**LG2
 End If
 '**LG1
End Function
%>