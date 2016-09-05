using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UnCommon.HTTP{

    /// <summary>
    /// 处理类
    /// </summary>
    public class UnHttpProcessor {

        /// <summary>
        /// SOCKET对象
        /// </summary>
        public TcpClient socket;       

        /// <summary>
        /// 服务对象
        /// </summary>
        public UnHttpServer srv;

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
        public UnHttpProcessor(TcpClient s, UnHttpServer srv) {
            this.socket = s;
            this.srv = srv;                   
        }
        

        private string streamReadLine(Stream inputStream) {
            int next_char;
            string data = "";
            while (true) {
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
        public void process() {                        
            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            inputStream = new BufferedStream(socket.GetStream());

            // we probably shouldn't be using a streamwriter for all output from handlers either
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET")) {
                    handleGETRequest();
                } else if (http_method.Equals("POST")) {
                    handlePOSTRequest();
                }
            } catch (Exception e) {
                Console.WriteLine("Exception: " + e.ToString());
                writeFailure();
            }
            outputStream.Flush();
            // bs.Flush(); // flush any remaining output
            inputStream = null; outputStream = null; // bs = null;            
            socket.Close();             
        }

        /// <summary>
        /// 参数
        /// </summary>
        public void parseRequest() {
            String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3) {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            Console.WriteLine("starting: " + request);
        }

        /// <summary>
        /// 读取头信息
        /// </summary>
        public void readHeaders() {
            Console.WriteLine("readHeaders()");
            String line;
            while ((line = streamReadLine(inputStream)) != null) {
                if (line.Equals("")) {
                    Console.WriteLine("got headers");
                    return;
                }
                
                int separator = line.IndexOf(':');
                if (separator == -1) {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' ')) {
                    pos++; // strip any spaces
                }
                    
                string value = line.Substring(pos, line.Length - pos);
                Console.WriteLine("header: {0}:{1}",name,value);
                httpHeaders[name] = value;
            }
        }

        /// <summary>
        /// GET句柄
        /// </summary>
        public void handleGETRequest() {
            srv.handleGETRequest(this);
        }

        private const int BUF_SIZE = 4096;

        /// <summary>
        /// POST句柄
        /// </summary>
        public void handlePOSTRequest() {
            // this post data processing just reads everything into a memory stream.
            // this is fine for smallish things, but for large stuff we should really
            // hand an input stream to the request processor. However, the input stream 
            // we hand him needs to let him see the "end of the stream" at this content 
            // length, because otherwise he won't know when he's seen it all! 

            Console.WriteLine("get post data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length")) {
                 content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                 if (content_len > MAX_POST_SIZE) {
                     throw new Exception(
                         String.Format("POST Content-Length({0}) too big for this simple server",
                           content_len));
                 }
                 byte[] buf = new byte[BUF_SIZE];              
                 int to_read = content_len;
                 while (to_read > 0) {  
                     Console.WriteLine("starting Read, to_read={0}",to_read);

                     int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                     Console.WriteLine("read finished, numread={0}", numread);
                     if (numread == 0) {
                         if (to_read == 0) {
                             break;
                         } else {
                             throw new Exception("client disconnected during post");
                         }
                     }
                     to_read -= numread;
                     ms.Write(buf, 0, numread);
                 }
                 ms.Seek(0, SeekOrigin.Begin);
            }
            Console.WriteLine("get post data end");
            srv.handlePOSTRequest(this, new StreamReader(ms));

        }

        /// <summary>
        /// 写入成功
        /// </summary>
        public void writeSuccess() {
            outputStream.WriteLine("HTTP/1.0 200 OK");            
            outputStream.WriteLine("Content-Type: text/html");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        /// <summary>
        /// 写入失败
        /// </summary>
        public void writeFailure() {
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
    }

    /// <summary>
    /// 抽象服务类
    /// </summary>
    public abstract class UnHttpServer {

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
        public UnHttpServer(int port) {
            this.port = port;
        }

        /// <summary>
        /// 监听
        /// </summary>
        public void listen() {
            IPAddress MyIP = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(MyIP, port);
            listener.Start();
            while (is_active) {                
                TcpClient s = listener.AcceptTcpClient();
                UnHttpProcessor processor = new UnHttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// GET句柄
        /// </summary>
        /// <param name="p"></param>
        public abstract void handleGETRequest(UnHttpProcessor p);

        /// <summary>
        /// POST句柄
        /// </summary>
        /// <param name="p"></param>
        /// <param name="inputData"></param>
        public abstract void handlePOSTRequest(UnHttpProcessor p, StreamReader inputData);
    }

    /// <summary>
    /// 实现服务类
    /// </summary>
    public class MyHttpServer : UnHttpServer {

        /// <summary>
        /// 构造类
        /// </summary>
        /// <param name="port"></param>
        public MyHttpServer(int port)
            : base(port) {
        }

        /// <summary>
        /// GET处理
        /// </summary>
        /// <param name="p"></param>
        public override void handleGETRequest(UnHttpProcessor p) {
            Console.WriteLine("request: {0}", p.http_url);
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
        public override void handlePOSTRequest(UnHttpProcessor p, StreamReader inputData) {
            Console.WriteLine("POST request: {0}", p.http_url);
            string data = inputData.ReadToEnd();

            p.outputStream.WriteLine("<html><body><h1>test server</h1>");
            p.outputStream.WriteLine("<a href=/test>return</a><p>");
            p.outputStream.WriteLine("postbody: <pre>{0}</pre>", data);
            

        }
    }

    /// <summary>
    /// 测试
    /// </summary>
    public class TestMain {

        /// <summary>
        /// 主入口
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Main(String[] args) {
            UnHttpServer httpServer;
            if (args.GetLength(0) > 0) {
                httpServer = new MyHttpServer(Convert.ToInt16(args[0]));
            } else {
                httpServer = new MyHttpServer(8080);
            }
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
            return 0;
        }

    }

}



