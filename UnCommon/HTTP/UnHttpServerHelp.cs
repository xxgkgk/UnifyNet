using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnCommon.Files;
using UnCommon.Tool;

namespace UnCommon.HTTP
{

    /// <summary>
    /// 处理类
    /// </summary>
    public class UnHttpServerProcessor
    {

        /// <summary>
        /// SOCKET对象
        /// </summary>
        public TcpClient socket;

        /// <summary>
        /// 服务对象
        /// </summary>
        public UnHttpServerAbs srv;

        private Stream inputStream;

        /// <summary>
        /// 输出流
        /// </summary>
        public StreamWriter outputStream;

        /// <summary>
        /// 连接方式
        /// </summary>
        public String http_method;

        /// <summary>
        /// URL地址
        /// </summary>
        public String http_url;

        /// <summary>
        /// 协议版本
        /// </summary>
        public String http_protocol_versionstring;

        /// <summary>
        /// 头信息
        /// </summary>
        public Hashtable httpHeaders = new Hashtable();


        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="s"></param>
        /// <param name="srv"></param>
        public UnHttpServerProcessor(TcpClient s, UnHttpServerAbs srv)
        {
            this.socket = s;
            this.srv = srv;
        }


        private string streamReadLine(Stream inputStream)
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }

        /// <summary>
        /// 处理
        /// </summary>
        public void process()
        {
            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            inputStream = new BufferedStream(socket.GetStream());

            // we probably shouldn't be using a streamwriter for all output from handlers either
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try
            {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET"))
                {
                    handleGETRequest();
                }
                else if (http_method.Equals("POST"))
                {
                    handlePOSTRequest();
                }
            }
            catch (Exception e)
            {
                console("Exception: " + e.ToString());
                writeFailure();
            }
            outputStream.Flush();
            // bs.Flush(); // flush any remaining output
            inputStream.Dispose();
            outputStream.Dispose();
            //inputStream = null; //outputStream = null; // bs = null;            
            socket.Close();
        }

        /// <summary>
        /// 参数
        /// </summary>
        public void parseRequest()
        {
            String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            console(UnDate.shortNowTime() + " 开始: " + request);
        }

        /// <summary>
        /// 读取头信息
        /// </summary>
        public void readHeaders()
        {
            //console("readHeaders()");
            String line;
            while ((line = streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    //console("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                //console("header: " + name + ":" + value);
                httpHeaders[name] = value;
            }
        }

        /// <summary>
        /// GET句柄
        /// </summary>
        public void handleGETRequest()
        {
            srv.handleGETRequest(this);
        }

        private const int BUF_SIZE = 4096;

        /// <summary>
        /// 打印次数
        /// </summary>
        private int consoleNum = 1;

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="str">内容</param>
        private void console(string str)
        {
            consoleNum++;
            if (consoleNum > 100)
            {
                consoleNum = 0;
                Console.Clear();
            }
            Console.WriteLine(str);
        }


        /// <summary>
        /// POST句柄
        /// </summary>
        public void handlePOSTRequest()
        {
            // this post data processing just reads everything into a memory stream.
            // this is fine for smallish things, but for large stuff we should really
            // hand an input stream to the request processor. However, the input stream 
            // we hand him needs to let him see the "end of the stream" at this content 
            // length, because otherwise he won't know when he's seen it all! 

            console(UnDate.shortNowTime() + " 开始 POST 获取");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    UnFile.writeLog("handlePOSTRequest", "POST Content-Length(" + content_len + ") 超过大小限制(" + MAX_POST_SIZE + ")");
                    return;
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            console("客户端关闭了链接");
                            return;
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            console(UnDate.shortNowTime() + " 结束 POST 获取");
            srv.handlePOSTRequest(this, new StreamReader(ms));

        }

        /// <summary>
        /// 写入成功
        /// </summary>
        public void writeSuccess()
        {
            outputStream.WriteLine("HTTP/1.0 200 OK");
            outputStream.WriteLine("Content-Type: text/html");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        /// <summary>
        /// 写入失败
        /// </summary>
        public void writeFailure()
        {
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
    }

    /// <summary>
    /// 抽象服务类
    /// </summary>
    public abstract class UnHttpServerAbs
    {

        /// <summary>
        /// 端口
        /// </summary>
        protected int port;
        TcpListener listener;
        bool is_active = true;

        /// <summary>
        /// 服务类
        /// </summary>
        /// <param name="port"></param>
        public UnHttpServerAbs(int port)
        {
            this.port = port;
        }

        /// <summary>
        /// 监听
        /// </summary>
        public void listen()
        {
            IPAddress MyIP = IPAddress.Parse("0");
            listener = new TcpListener(MyIP, port);
            listener.Start();
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                UnHttpServerProcessor processor = new UnHttpServerProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// GET句柄
        /// </summary>
        /// <param name="p"></param>
        public abstract void handleGETRequest(UnHttpServerProcessor p);

        /// <summary>
        /// POST句柄
        /// </summary>
        /// <param name="p"></param>
        /// <param name="inputData"></param>
        public abstract void handlePOSTRequest(UnHttpServerProcessor p, StreamReader inputData);
    }

    /// <summary>
    /// 实现服务类
    /// </summary>
    public class UnHttpServerTest : UnHttpServerAbs
    {

        /// <summary>
        /// 构造类
        /// </summary>
        /// <param name="port"></param>
        public UnHttpServerTest(int port)
            : base(port)
        {
        }

        /// <summary>
        /// GET处理
        /// </summary>
        /// <param name="p"></param>
        public override void handleGETRequest(UnHttpServerProcessor p)
        {
            //Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
            p.outputStream.WriteLine("<html><body><h1>test server</h1>");
            p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
            p.outputStream.WriteLine("url : {0}", p.http_url);

            p.outputStream.WriteLine("<form method=post action=/form>");
            p.outputStream.WriteLine("<input type=text name=foo value=foovalue>");
            p.outputStream.WriteLine("<input type=submit name=bar value=barvalue>");
            p.outputStream.WriteLine("</form>");
        }

        /// <summary>
        /// POST处理
        /// </summary>
        /// <param name="p"></param>
        /// <param name="inputData"></param>
        public override void handlePOSTRequest(UnHttpServerProcessor p, StreamReader inputData)
        {
            //Console.WriteLine("POST request: {0}", p.http_url);
            string data = inputData.ReadToEnd();
            Console.WriteLine(data);
            p.outputStream.WriteLine("<html><body><h1>test server</h1>");
            p.outputStream.WriteLine("<a href=/test>return</a><p>");
            p.outputStream.WriteLine("postbody: <pre>{0}</pre>", data);

        }
    }

}



