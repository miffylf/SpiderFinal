//======================================================================
// 所属项目：Spider
// 创 建 人：lifei
// 创建日期：2015/5/2
// 用    途：爬虫
//====================================================================== 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spider
{
    public class SpiderLogic
    {
        #region 属性
        private int maxDepth = 4;
        /// <summary>
        /// 初始化最大深度
        /// </summary>
        public int MaxDepth
        {
            get { return maxDepth; }
            set { maxDepth = value; }
        }

        private int maxConnection = 10;
        /// <summary>
        /// 初始化最大连接数
        /// </summary>
        public int MaxConnection
        {
            get { return maxConnection; }
            set { maxConnection = value; }
        }

        private string rootUrl;
        /// <summary>
        /// 根网址
        /// </summary>
        public string RootUrl
        {
            get { return rootUrl; }
            set
            {
                if (!value.Contains("http://"))
                {
                    rootUrl = "http://" + value;
                }
                else
                {
                    rootUrl = value;
                }
                mBaseurl = rootUrl.Replace("www.", "");
                mBaseurl = mBaseurl.Replace("http://", "");
                mBaseurl = mBaseurl.TrimEnd('/');
            }
        }

        private string path;
        /// <summary>
        /// 下载路径 
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private bool stop = false;
        /// <summary>
        /// 是否停止 
        /// </summary>
        public bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        private bool isFinish = false;
        /// <summary>
        /// 是否完成全部下载
        /// </summary>
        public bool IsFinish
        {
            get { return isFinish; }
            set { isFinish = value; }
        }

        private Dictionary<string, int> read = new Dictionary<string, int>();
        /// <summary>
        /// 已下载url
        /// </summary>
        public Dictionary<string, int> Read
        {
            get { return read; }
            set { read = value; }
        }

        private Dictionary<string, int> unread = new Dictionary<string, int>();
        /// <summary>
        /// 未下载url
        /// </summary>
        public Dictionary<string, int> Unread
        {
            get { return unread; }
            set { unread = value; }
        }

        #endregion

        #region 委托
        public delegate void ContentsSavedHandler(string path, string url);

        public delegate void DownloadFinishHandler(int count);
        #endregion

        #region 事件
        /// <summary>
        /// 正文内容被保存到本地后触发
        /// </summary>
        public event ContentsSavedHandler ContentsSaved = null;

        /// <summary>
        /// 全部链接下载分析完毕后触发
        /// </summary>
        public event DownloadFinishHandler DownloadFinish = null;
        #endregion

        #region 成员变量
        private readonly object mLocker = new object();
        private string mBaseurl = null;
        private static Encoding GB18030 = Encoding.GetEncoding("GB18030");   // GB18030兼容GBK和GB2312
        private static Encoding UTF8 = Encoding.UTF8;
        private string mUserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
        private string mAccept = "text/html";
        private string mMethod = "GET";
        private Encoding mEncoding = GB18030;
        private Encodings mEnc = Encodings.GB;
        private int mMaxTime = 2 * 60 * 1000;
        RedisCommon mRedisCommon;

        public enum Encodings
        {
            UTF8,
            GB
        }
        #endregion

        #region 方法
        private void Init()
        {
            mRedisCommon = new RedisCommon();
            AddUrl(new string[] { RootUrl }, 0);
        }

        /// <summary>
        /// 开始工作
        /// </summary>
        /// 通过并发的方式来读取多个url的资源
        private void DispatchWork()
        {
            if (Stop)
            {
                return;
            }
        http://111.27.2.14:8089/LuceneSearch/Search.aspx
            foreach (var item in Unread)
            {
                if (read.Keys.Contains(item.Key))
                {
                    continue;
                }
                RequestResource(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 使用线程池并发执行下载url
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Depth"></param>
        private void RequestResource(string Url, int Depth)
        {
            try
            {
                lock (mLocker)
                {
                    if (IsFinish == true)//如果全部下载完毕返回
                    {
                        return;
                    }
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                    req.Method = mMethod;
                    req.Accept = mAccept;
                    req.UserAgent = mUserAgent;
                    RequestState rs = new RequestState(req, Url, Depth);
                    var reuslt = req.BeginGetResponse(new AsyncCallback(ReceivedResource), rs);//异步请求
                    ThreadPool.RegisterWaitForSingleObject(reuslt.AsyncWaitHandle, TimeOutCallBack, rs, mMaxTime, true);//注册超时处理方法
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 接受下载数据
        /// </summary>
        /// <param name="ar"></param>
        private void ReceivedResource(IAsyncResult ar)
        {
            RequestState rs = ar.AsyncState as RequestState;
            if (rs == null)
            {
                return;
            }
            string url = rs.Url;
            HttpWebRequest req = rs.Req;
            try
            {
                HttpWebResponse res = (HttpWebResponse)req.EndGetResponse(ar);//获取响应
                if (Stop)
                {
                    res.Close();
                    req.Abort();
                    return;
                }
                if (res != null && res.StatusCode == HttpStatusCode.OK)//判断是否成功获取响应
                {
                    Stream resStream = res.GetResponseStream();//获取资源
                    rs.ResStream = resStream;
                    var result = resStream.BeginRead(rs.Data, 0, rs.BufferSize, new AsyncCallback
                        (ReceivedData), rs);
                }
                else
                {
                    res.Close();
                    req.Abort();
                    DispatchWork();//重新分配
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("ReceivedResource " + ex.Message + url);
            }
        }

        private void ReceivedData(IAsyncResult ar)
        {
            RequestState rs = ar.AsyncState as RequestState;
            HttpWebRequest req = rs.Req;
            Stream resStream = rs.ResStream;
            string url = rs.Url;
            int depth = rs.Depth;
            string html = string.Empty;
            int read = 0;
            try
            {
                read = resStream.EndRead(ar);
                if (Stop)
                {
                    rs.ResStream.Close();
                    req.Abort();
                    return;
                }
                if (read > 0)
                {
                    MemoryStream ms = new MemoryStream(rs.Data, 0, read);//创建内存流
                    StreamReader reader = new StreamReader(ms, mEncoding);
                    string str = reader.ReadToEnd();//读取全部字符串
                    rs.Html.Append(str);
                    var result = resStream.BeginRead(rs.Data, 0, rs.BufferSize, new AsyncCallback(ReceivedData), rs);//再次请求数据
                    return;
                }
                html = rs.Html.ToString();
                bool isRedis = Convert.ToBoolean(RedisCommon.GetConfig("IsRedis"));
                if (!isRedis)
                {
                    SaveContents(html, url);//保存到本地
                }
                else
                {
                    mRedisCommon.SetValue(url, html);
                }
                string[] links = GetLinks(html); //获取页面中的链接
                Read.Add(url, depth);//添加到已经下载集合
                AddUrl(links, depth + 1);
                Unread.Remove(url);//移除未下载
                mRedisCommon.SetValue(url, url);
                DispatchWork();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        private void SaveContents(string html, string url)
        {
            if (string.IsNullOrEmpty(html))
            {
                return;
            }
            string path = "";
            lock (mLocker)
            {
                string name = url.Replace('/', '@').Replace('.', '#').Replace(':', '$');
                path = string.Format("{0}\\{1}.txt", Path, name);
            }

            try
            {
                using (StreamWriter fs = new StreamWriter(path))
                {
                    fs.Write(html);
                }
            }
            catch (IOException ioe)
            {
                MessageBox.Show(ioe.Message);
            }

            if (ContentsSaved != null)
            {
                ContentsSaved(path, url);
            }
        }

        private string[] GetLinks(string html)
        {
            const string pattern = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(html);
            //HtmlAgilityPack.HtmlDocument document = new HtmlDocument();
            //document.LoadHtml(html);
            //var linksOnPage = from lnks in document.DocumentNode.Descendants()
            //                  where lnks.Name == "a" &&
            //                  lnks.Attributes["href"] != null &&
            //                  lnks.InnerText.Trim().Length > 0
            //                  select new
            //                  {
            //                      Url = lnks.Attributes["href"].Value,
            //                      Text = lnks.InnerText
            //                  };
            string[] links = new string[m.Count];

            for (int i = 0; i < m.Count; i++)
            {
                links[i] = m[i].ToString();
            }
            return links;
        }

        private bool UrlExists(string url)
        {
            bool result = read.ContainsKey(url);
            result |= read.ContainsKey(url);
            return result;
        }

        private bool UrlAvailable(string url)
        {
            if (UrlExists(url))
            {
                return false;
            }
            if (url.Contains(".jpg") || url.Contains(".gif")
                || url.Contains(".png") || url.Contains(".css")
                || url.Contains(".js"))
            {
                return false;
            }
            return true;
        }

        public void AddUrl(string[] urls, int depth)
        {
            try
            {
                if (depth >= MaxDepth)
                {
                    return;
                }
                foreach (string url in urls)
                {
                    string cleanUrl = url.Trim();
                    int end = cleanUrl.IndexOf(' ');
                    if (end > 0)
                    {
                        cleanUrl = cleanUrl.Substring(0, end);
                    }
                    cleanUrl = cleanUrl.TrimEnd('/');
                    if (UrlAvailable(cleanUrl))
                    {
                        if (cleanUrl.Contains(mBaseurl))
                        {
                            if (unread.Keys.Contains(cleanUrl))
                            {
                                continue;
                            }
                            Unread.Add(cleanUrl, depth);
                        }
                        else
                        {
                            // 外链
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="param"></param>
        public void DownLoad(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return;
            }
            Path = param;
            Init();
            DispatchWork();
        }


        /// <summary>
        /// 超时处理方法
        /// </summary>
        /// <param name="state"></param>
        /// <param name="timedOut"></param>
        /// 
        private void TimeOutCallBack(object state, bool timedOut)
        {
            if (timedOut)
            {
                RequestState rs = state as RequestState;
                if (rs != null)
                {
                    rs.Req.Abort();
                }
                DispatchWork();
            }
        }
        private void Finish()
        {
            if (Unread.Count <= 0)
            {
                IsFinish = true;
            }
            else
            {
                IsFinish = false;
            }
        }


        #endregion
    }

    public class RequestState
    {
        #region 成员变量
        private const int BUFFER_SIZE = 131072; //接收数据包的空间大小
        private byte[] _data = new byte[BUFFER_SIZE]; //接收数据包的buffer
        private StringBuilder _sb = new StringBuilder(); //存放所有接收到的字符

        public HttpWebRequest Req { get; private set; } //请求
        public string Url { get; private set; } //请求的URL
        public int Depth { get; private set; } //此次请求的相对深度
        public Stream ResStream { get; set; } //接收数据流
        public StringBuilder Html
        {
            get
            {
                return _sb;
            }
        }

        public byte[] Data
        {
            get
            {
                return _data;
            }
        }

        public int BufferSize
        {
            get
            {
                return BUFFER_SIZE;
            }
        }
        #endregion

        public RequestState(HttpWebRequest req, string url, int depth)
        {
            Req = req;
            Url = url;
            Depth = depth;
        }


    }
}
