using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneSearch.Logic;
using mshtml;
using PanGu;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace LuceneSearch
{
    public partial class Search : System.Web.UI.Page
    {
        public string kw = string.Empty;
        //RenderToHTML为输出的分页控件<a>..<a>
        protected string RenderToHTML { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            hotwordsRepeater.DataSource = new KeywordDao().GetHotWords();
            hotwordsRepeater.DataBind();
            kw = Request["kw"];
            if (string.IsNullOrWhiteSpace(kw))
            {
                return;
            }
            //处理：将用户的搜索记录加入数据库，方便统计热词
            SerachKeyword model = new SerachKeyword();
            model.Keyword = kw;
            model.SearchDateTime = DateTime.Now;
            model.ClinetAddress = Request.UserHostAddress;

            new KeywordDao().Add(model);
            //分页控件
            MyPage pager = new MyPage();
            pager.TryParseCurrentPageIndex(Request["pagenum"]);
            //超链接href属性
            pager.UrlFormat = "CreateIndex.aspx?pagenum={n}&kw=" + Server.UrlEncode(kw);

            int startRowIndex = (pager.CurrentPageIndex - 1) * pager.PageSize;


            int totalCount = -1;
            List<SearchResult> list = DoSearch(startRowIndex, pager.PageSize, out totalCount);
            pager.TotalCount = totalCount;
            RenderToHTML = pager.RenderToHTML();
            dataRepeater.DataSource = list;
            dataRepeater.DataBind();
        }

        private List<SearchResult> DoSearch(int startRowIndex, int pageSize, out int totalCount)
        {
            string indexPath = ConfigurationManager.AppSettings["pathIndex"]; ;
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            //IndexSearcher是进行搜索的类
            IndexSearcher searcher = new IndexSearcher(reader);
            PhraseQuery query = new PhraseQuery();

            foreach (string word in CommonHelper.SplitWord(kw))
            {
                query.Add(new Term("body", word));
            }
            query.SetSlop(100);//相聚100以内才算是查询到
            TopScoreDocCollector collector = TopScoreDocCollector.create(1024, true);//最大1024条记录
            searcher.Search(query, null, collector);
            totalCount = collector.GetTotalHits();//返回总条数
            ScoreDoc[] docs = collector.TopDocs(startRowIndex, pageSize).scoreDocs;//分页,下标应该从0开始吧，0是第一条记录
            List<SearchResult> list = new List<SearchResult>();
            for (int i = 0; i < docs.Length; i++)
            {
                int docID = docs[i].doc;//取文档的编号，这个是主键，lucene.net分配
                //检索结果中只有文档的id，如果要取Document，则需要Doc再去取
                //降低内容占用
                Document doc = searcher.Doc(docID);
                string number = doc.Get("number");
                string title = doc.Get("title");
                string body = doc.Get("body");

                SearchResult searchResult = new SearchResult() { Number = number, Title = title, BodyPreview = Preview(body, kw) };
                list.Add(searchResult);

            }
            return list;
        }

        private string Preview(string body, string keyword)
        {
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter = new PanGu.HighLight.SimpleHTMLFormatter("<font color=\"Red\">", "</font>");
            PanGu.HighLight.Highlighter highlighter = new PanGu.HighLight.Highlighter(simpleHTMLFormatter, new Segment());
            highlighter.FragmentSize = 100;
            string bodyPreview = highlighter.GetBestFragment(keyword, body);
            return bodyPreview;
        }

        private int GetMaxID()
        {
            XDocument xdoc = XDocument.Load("Http://localhost:8080/tools/rss.aspx");
            XElement channel = xdoc.Root.Element("channel");
            XElement fitstItem = channel.Elements("item").First();
            XElement link = fitstItem.Element("link");
            Match match = Regex.Match(link.Value, @"http://localhost:8080/showtopic-(\d+)\.aspx");
            string id = match.Groups[1].Value;
            return Convert.ToInt32(id);
        }

        protected void searchButton_Click(object sender, EventArgs e)
        {
            //索引库的位置
            string indexPath = "C:/index";
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            bool isUpdate = IndexReader.IndexExists(directory);
            if (isUpdate)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }
            IndexWriter write = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate, IndexWriter.MaxFieldLength.UNLIMITED);

            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            int maxID = GetMaxID();
            for (int i = 1; i <= maxID; i++)
            {
                string url = "http://localhost:8080/showtopic-" + i + ".aspx";
                string html = wc.DownloadString(url);
                HTMLDocumentClass doc = new HTMLDocumentClass();

                doc.designMode = "on";
                doc.IHTMLDocument2_write(html);
                doc.close();

                string title = doc.title;
                string body = doc.body.innerText;

                write.DeleteDocuments(new Term("number", i.ToString()));

                Document document = new Document();
                document.Add(new Field("number", i.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("title", title, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("body", body, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                write.AddDocument(document);
                //logger.Debug("索引" + i.ToString() + "完毕");

            }
            write.Close();
            directory.Close();
            //logger.Debug("全部索引完毕");

        }
    }
}