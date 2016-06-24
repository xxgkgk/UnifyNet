<%
'������������ϴ�
Function StrToBin(str)'�ַ���ת������
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
Function BinToStr(binStr)'������ת��Ϊ�ַ���
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
                newStr = newStr & Chr(AscB(c)) '�����õ��� ASCB ���� ASCW
            end If
        else
            chnFlag = true
        end If
    next   
    BinToStr = newStr
End function 

Function frpl(x,a,b)
 Dim re,re1
 set re=new regExp'����������ʽ����
 re.IgnoreCase=true'���Դ�Сд 
 re.Global=true'����ȫ�̿�����
 re.Pattern=a
 re1=re.replace(x,b)
 frpl=re1
End Function

'�ļ���չ������
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
frt=Request.BinaryRead(Request.TotalBytes)'��ȡ��������Ϣ
hh=chrB(13)&chrB(10)
If LenB(frt)>0 Then
fg=LeftB(frt,Clng(Instrb(frt,hh)-1))
fgn=LeftB(frt,Clng(Instrb(frt,hh)+39))
End If

Function getup(fn,fs)
Dim fstr,fhh,flg,flgn,fsz,ftp,fph
 fstr=frt'�ļ���Ϣ
 fhh=hh'���з�
 flg=fg'�ļ������ʶ
 flgn=fgn'����ƥ���ַ���
 fsz=fsize'�ļ���С,���ղ�����
 ftp=ftype'�ļ�����,���ղ�����
 fph=fpath'���·��,���ձ����ڵ�ǰ
Dim tlg,tk1,tlr1,tlr2,tkb
 tlg=flgn&strtobin(fn&"""")'����ʶ
 tk1=instrb(fstr,tlg)'����ʼλ
 '**�������ж�**LG1
 If tk1=0 Then
  getup=""
 Else
  tlr1=instrb(tk1,fstr,fhh&fhh)+3'���ݿ�ʼλ
  tlr2=instrb(tlr1,fstr,flg)'���ݽ���λ
  tkb=tlr2-tlr1-3'���ݴ�С
  '**�ж���(0)/�ļ�(1)����ȡ**LG2
  If fs=0 Then
   getup=bintostr(midb(fstr,tlr1+1,tkb))
  ElseIf fs=1 Then
   '**���ݴ�С�ж�**LG3
   If tkb>0 Then
    If fsz="" Or tkb<=fsz Then
	 Dim fn1,fn2,fnm,kzm,ktp,ary,i,n
	  fn1=instrb(tk1,fstr,strtobin("filename"))+10'�ļ�����ʼλ
	  fn2=instrb(fn1,fstr,strtobin(""""))'�ļ�������λ
	  fnm=bintostr(midb(fstr,fn1,fn2-fn1))'�ļ���
	  kzm=frpl(fnm,".*\.","")'��չ��
	  ktp=bintostr(midb(fstr,fn2+17,tlr1-fn2-17-3))'�ļ�����
	  ary=Split(ftp,",")'���ͼ��
	  n=0
	  For i=0 To Ubound(ary)
	   If Instr(kztp(ary(i)),ktp)>0 And Instr(ftp,kzm)>0 Then
	    n=1
	    Exit For
	   End If
	  Next
	  If ftp="" Or n=1 Then'�����ļ�
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