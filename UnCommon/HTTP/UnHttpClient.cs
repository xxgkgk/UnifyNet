using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.IO;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;
using System.Web;
using UnCommon.Config;
using UnCommon.Files;
using UnCommon.Entity;
using UnCommon.Extend;
using UnCommon.Delegates;
using UnCommon.Interfaces;


namespace UnCommon.HTTP
{
    /// <summary>
    /// http客户端处理类
    /// </summary>
    public class UnHttpClient
    {

        // 进程ID
        private int _pid;
        // url地址
        private string _url = null;
        // 消息
        private string _msg = null;
        // 文件名
        private string _fileName = null;
        // 事件
        private UnHttpUpEvent _eve = UnHttpUpEvent.Image;
        // 证书路径
        private string _cerPath = null;
        // p12证收密码
        private string _cerPass = null;
        // 缓存过期时间(毫秒)
        private int _cacheTimeOut = -1;
        // 编码
        private Encoding en = UnInit.getEncoding();
        // 过期时间
        private int _timeOut = 5000;

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="url"></param>
        public UnHttpClient(string url)
        {
            init(url, -1, null);
        }

        /// <summary>
        /// 实例化 cert证书
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cerPath"></param>
        public UnHttpClient(string url, string cerPath)
        {
            init(url, -1, cerPath);
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="url"></param>
        /// <param name="packPath"></param>
        /// <param name="packPass"></param>
        public UnHttpClient(string url, string packPath, string packPass)
        {
            init(url, -1, packPath);
            this._cerPass = packPass;
        }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cacheTimeOut"></param>
        public UnHttpClient(string url, int cacheTimeOut)
        {
            init(url, cacheTimeOut, null);
        }

        // 初始化
        private void init(string url, int cacheTimeOut,string cerPath)
        {
            this._url = url;
            this._cacheTimeOut = cacheTimeOut;
            this._cerPath = cerPath;
            this._pid = UnInit.pid();
        }

        // 发送
        private UnAttrRst send()
        {
            UnAttrRst rst = new UnAttrRst();
            UnAttrPgs pgs = new UnAttrPgs();
            try
            {
                byte[] send = en.GetBytes(_msg);
                // md5加密文件名
                string md5 = (_msg + _url).md5Hash();
                string path = UnFileEvent.caches.fullPath() + md5 + ".txt";
                // 缓存文件
                UnFileInfo f = new UnFileInfo(path, null, null, _cacheTimeOut);
                if (_cacheTimeOut > 0 && f.exists && !f.isLate)
                {
                    // 进度
                    pgs.pid = _pid;
                    pgs.totalLength = f.length;
                    pgs.length = f.length;
                    this.tryProgress(pgs);

                    // 完成
                    rst.pid = _pid;
                    rst.code = 2;
                    rst.msg = "返回缓存";
                    using (FileStream inf_fs = new FileStream(f.fullName, FileMode.Open))
                    {
                        byte[] data = new byte[inf_fs.Length];
                        inf_fs.Read(data, 0, data.Length);
                        rst.back = en.GetString(data);
                        rst.data = rst.back;
                    }
                    this.trySuccess(rst);
                    return rst;
                }

                // 设置参数
                HttpWebRequest request = UnHttpHelp.createPost(_url, _timeOut, "text/xml", _eve.text(), _cerPath, _cerPass);
                request.ContentLength = send.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(send, 0, send.Length);
                }

                // 下载数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                byte[] back = UnHttpHelp.getResponseData(response);

                pgs.pid = _pid;
                pgs.totalLength = back.Length;
                pgs.length = back.Length;
                this.tryProgress(pgs);

                rst.pid = _pid;
                rst.code = 1;
                rst.msg = "提交成功";
                rst.back = en.GetString(back);
                rst.data = rst.back;
                bool b = this.trySuccess(rst);
                if (_cacheTimeOut > 0 && b)
                {
                    Console.WriteLine(f.fullName);
                    UnFile.createDirectory(UnFileEvent.caches);
                    using (FileStream inf_fs = new FileStream(f.fullName, FileMode.Create))
                    {
                        inf_fs.Seek(0, SeekOrigin.Begin);
                        inf_fs.Write(back, 0, (int)back.Length);
                    }
                }
            }
            catch (Exception e)
            {
                rst.pid = _pid;
                rst.code = -1;
                rst.msg = e.ToString();
                this.tryError(rst);
            }
            return rst;
        }

        // 异步提交
        private void sendAsyn()
        {
            send();
        }

        // 上传
        private void up()
        {
            HttpWebRequest request = UnHttpHelp.createPost(_url, _timeOut, null, _eve.text());
            UnFileInfo _f = new UnFileInfo(_fileName);
            request.Headers["md5"] = _f.md5;
            request.Headers["extens"] = _f.extens;

            using (FileStream fs = new FileStream(_fileName, FileMode.Open))
            {
                request.ContentLength = fs.Length;
                BinaryReader r = new BinaryReader(fs);
                // 每次读取大小  
                int bufferLength = 1024;
                byte[] buffer = new byte[bufferLength];

                // 开始发送
                long fSize = fs.Length;
                long dSize = 0;
                int size = r.Read(buffer, 0, bufferLength);
                using (Stream post = request.GetRequestStream())
                {
                    while (size > 0)
                    {
                        dSize += size;
                        post.Write(buffer, 0, size);
                        size = r.Read(buffer, 0, bufferLength);

                        UnAttrPgs pgs = new UnAttrPgs();
                        pgs.pid = _pid;
                        pgs.totalLength = fSize;
                        pgs.length = dSize;
                        this.tryProgress(pgs);
                    }
                }
            }
            // 开始传输
            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                byte[] back = UnHttpHelp.getResponseData(response);
                UnAttrRst rst = new UnAttrRst();
                rst.pid = _pid;
                rst.code = 1;
                rst.msg = "通讯成功";
                rst.back = en.GetString(back);
                rst.data = rst.back;
                bool _bool = this.trySuccess(rst);
            }
            catch(Exception e)
            {
                UnAttrRst rst = new UnAttrRst();
                rst.pid = _pid;
                rst.code = -1;
                rst.msg = e.ToString();
                this.tryError(rst);
            }
        }

        // 下载
        private void down()
        {
            UnAttrPgs pgs = new UnAttrPgs();
            UnAttrRst rst = new UnAttrRst();
            try
            {
                string kzm = _url.Substring(_url.LastIndexOf("."));
                if (kzm.Length < 2)
                {
                    kzm = ".no";
                }
                // md5加密文件名
                string md5 = _url.md5Hash();
                string path = UnFileEvent.caches.fullPath() + md5 + kzm;
                // 缓存文件
                UnFileInfo f = new UnFileInfo(path, null, null, _cacheTimeOut);
                if (_cacheTimeOut > 0 && f.exists && !f.isLate)
                {
                    pgs.pid = _pid;
                    pgs.totalLength = f.length;
                    pgs.length = f.length;
                    this.tryProgress(pgs);

                    rst.pid = _pid;
                    rst.code = 2;
                    rst.msg = "返回缓存";
                    rst.back = path;
                    rst.data = f;
                    this.trySuccess(rst);
                    return;
                }

                // 下载数据
                HttpWebResponse response = UnHttpHelp.creageGet(_url, _timeOut).GetResponse() as HttpWebResponse;
                using (Stream rsps = response.GetResponseStream())
                {
                    UnFile.createDirectory(UnFileEvent.caches);
                    UnFile.createDirectory(UnFileEvent.tmp);
                    //创建本地文件写入流
                    using (Stream stream = new FileStream(f.fullNameTmp, FileMode.Create))
                    {
                        long fSize = response.ContentLength;
                        long dSize = 0;
                        byte[] buff = new byte[1024];
                        int size = rsps.Read(buff, 0, buff.Length);
                        while (size > 0)
                        {
                            dSize += size;
                            stream.Write(buff, 0, size);
                            size = rsps.Read(buff, 0, buff.Length);

                            pgs.pid = _pid;
                            pgs.totalLength = fSize;
                            pgs.length = dSize;
                            this.tryProgress(pgs);
                        }
                    }
                }

                // 转正式文件
                if (f.exists)
                {
                    f.baseInfo.Delete();
                }
                File.Move(f.fullNameTmp, f.fullName);
                f = new UnFileInfo(f.fullName);
                rst.pid = _pid;
                rst.code = 1;
                rst.msg = "下载完成";
                rst.data = f;
                bool b = this.trySuccess(rst);
                // 不缓存
                if (!b)
                {
                    f.baseInfo.Delete();
                }
            }
            catch (Exception e)
            {
                rst.pid = _pid;
                rst.code = -1;
                rst.msg = e.ToString();
                this.tryError(rst);
            }
        }

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="timeOut"></param>
        public void setTimeOut(int timeOut)
        {
            if (timeOut < 1000)
            {
                timeOut = 5000;
            }
            _timeOut = timeOut;
        }

        /// <summary>
        /// 发送消息(同步)
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public UnAttrRst sendMsgSyn(string msg)
        {
            this._msg = msg + "";
            return send();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void sendMsg(string msg)
        {
            this._msg = msg + "";
            new Thread(sendAsyn).Start();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="eve"></param>
        public void upFile(string fileName, UnHttpUpEvent eve)
        {
            this._fileName = fileName;
            this._eve = eve;
            new Thread(up).Start();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        public void downFile()
        {
            downFile(false);
        }

        /// <summary>
        /// 下载文件(最长缓存)
        /// </summary>
        /// <param name="maxChche"></param>
        public void downFile(bool maxChche)
        {
            if (maxChche)
            {
                this._cacheTimeOut = int.MaxValue;
            }
            new Thread(down).Start();
        }

        #region Listener

        // 传输接口
        private UnIntTransfer transfer = null;

        /// <summary>
        /// 设置传输接口实例
        /// </summary>
        /// <param name="t"></param>
        public void setIntTransfer(UnIntTransfer t)
        {
            this.transfer = t;
        }

        // 尝试回调进度监听
        private void tryProgress(UnAttrPgs pgs)
        {
            if (transfer != null)
            {
                transfer.progress(pgs);
            }
        }

        // 尝试回调完成监听
        private bool trySuccess(UnAttrRst rst)
        {
            if (transfer != null)
            {
                return transfer.success(rst);
            }
            return false;
        }

        // 尝试错误回调
        private void tryError(UnAttrRst rst)
        {
            if (transfer != null)
            {
                transfer.error(rst);
            }
        }

        #endregion
    }

}


