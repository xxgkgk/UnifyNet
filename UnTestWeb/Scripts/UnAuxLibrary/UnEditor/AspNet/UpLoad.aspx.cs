using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;

public partial class _UpLoad : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string CdPath = "../../";
        string UpPath = "UpFile/PubEditor/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
        List<string> _GetUpLoad = GetUpLoad(Request.Files["wj"], new string[] { "jpg", "gif", "png" }, 500, CdPath + UpPath);
        if (_GetUpLoad[0] == "false")
        {
            Response.Write("<script>alert('只允许jpg,gif,png格式!');</script>");
        }
        else if (_GetUpLoad[1] == "false")
        {
            Response.Write("<script>alert('只允许最大500K的图片!');</script>");
        }
        else
        {
            Response.Write("<script>window.parent.document.getElementById('yl').innerHTML='<img src=" + CdPath + UpPath + _GetUpLoad[2] + ">';</script>");
        }
        Response.Write("<script>var str='';</script>");
    }

    /// <summary>
    /// FileUpload控件上传文件
    /// </summary>
    /// <param name="_FileUpload">FileUpload控件</param>
    /// <param name="FileType">允许的类型(gif,jpg,txt...)</param>
    /// <param name="FileSize">允许的大小(KB)</param>
    /// <param name="FilePath">保存的路径(相对路径)</param>
    /// <returns>List[0]=类型是否允许,List[1]=大小是否允许,List[2]=保存的文件名称</returns>
    public List<string> GetFileUpload(System.Web.UI.WebControls.FileUpload _FileUpload, string[] FileType, int FileSize, string FilePath)
    {
        return GetUpLoad(_FileUpload.PostedFile, FileType, FileSize, FilePath);
    }

    /// <summary>
    /// 单独文件上载处理
    /// </summary>
    /// <param name="HPF">上载文件对象</param>
    /// <param name="FileType">允许的类型(gif,jpg,txt...)</param>
    /// <param name="FileSize">允许的大小(KB)</param>
    /// <param name="FilePath">保存的路径(相对路径)</param>
    /// <returns>List[0]=类型是否允许,List[1]=大小是否允许,List[2]=保存的文件名称</returns>
    public List<string> GetUpLoad(HttpPostedFile HPF, string[] FileType, int FileSize, string FilePath)
    {
        List<string> _List = new List<string>();
        string _ContentType = HPF.ContentType;
        string _IsType = "false";
        //文件类型判断
        for (int i = 0; i < FileType.Length; i++)
        {
            List<string> _ListMIME = GetMIME(FileType[i]);
            for (int n = 0; n < _ListMIME.Count; n++)
            {
                if (_ListMIME[n] == _ContentType)
                {
                    _IsType = "true";
                    break;
                }
            }
            if (_IsType == "true")
            {
                break;
            }
        }
        //文件大小判断
        string _IsSize = "false";
        if (HPF.ContentLength <= FileSize * 1024)
        {
            _IsSize = "true";
        }
        string _UpFileUrl = "";
        string _UpFileName = "";
        if (_IsType == "true" && _IsSize == "true")
        {
            //获得文件扩展名
            string _FileExt = System.IO.Path.GetExtension(HPF.FileName).ToLower();
            //产生随机文件名
            _UpFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + new Random().Next(1, 9999) + _FileExt;
            _UpFileUrl = FilePath + "/" + _UpFileName;
            //创建文件夹
            DirectoryInfo _DirectoryInfo = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(FilePath));
            if (_DirectoryInfo.Exists == false)
            {
                _DirectoryInfo.Create();
            }
            //保存文件
            HPF.SaveAs(System.Web.HttpContext.Current.Server.MapPath(_UpFileUrl));
        }
        _List.Add(_IsType);
        _List.Add(_IsSize);
        _List.Add(_UpFileName);
        return _List;
    }

    /// <summary>
    /// 文件扩展名对应MIME类型
    /// </summary>
    /// <param name="FileType">文件扩展名</param>
    /// <returns></returns>
    public List<string> GetMIME(string FileType)
    {
        List<string> _List = new List<string>();
        switch (FileType)
        {
            case "gif":
                _List.Add("image/gif");
                break;
            case "jpe":
            case "jpg":
            case "jpeg":
                _List.Add("image/jpeg");
                _List.Add("image/pjpeg");
                break;
            case "png":
                _List.Add("image/png");
                _List.Add("image/x-png");
                break;
            case "bmp":
                _List.Add("image/bmp");
                _List.Add("image/x-ms-bmp");
                break;
            case "txt":
                _List.Add("text/plain");
                break;
            case "doc":
            case "dot":
                _List.Add("application/msword");
                break;
            case "xls":
                _List.Add("application/vnd.ms-excel");
                _List.Add("application/excel");
                break;
            case "ppt":
                _List.Add("application/vnd.ms-powerpoint");
                break;
            case "pdf":
                _List.Add("application/pdf");
                break;
            case "rar":
            case "zip":
                _List.Add("application/octet-stream");
                _List.Add("application/zip");
                break;
            case "ra":
                _List.Add("audio/x-pn-realaudio");
                break;
            case "rm":
                _List.Add("application/vnd.rn-realmedia");
                break;
            case "ram":
                _List.Add("application/x-pn-realaudio");
                break;
            case "mp2":
            case "mp3":
            case "mpa":
            case "mpe":
            case "mpeg":
            case "mpg":
                _List.Add("audio/mpeg");
                break;
            case "wma":
                _List.Add("audio/x-ms-wma");
                break;
            case "wmv":
                _List.Add("video/x-ms-wmv");
                break;
            case "avi":
                _List.Add("video/x-msvideo");
                break;
            case "swf":
                _List.Add("application/x-shockwave-flash");
                break;
        }
        return _List;
    }
}