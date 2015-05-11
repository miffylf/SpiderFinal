using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using mshtml;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace LuceneIndex
{
    public class LuceneLogic
    {
        Document docment;
        private static Encoding GB18030 = Encoding.GetEncoding("GB18030");   // GB18030兼容GBK和GB2312
        private static Encoding UTF8 = Encoding.UTF8;
        static RedisClient mRedisClient;

        public LuceneLogic()
        {
            string host = ConfigurationManager.AppSettings["RedisConnection"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            mRedisClient = new RedisClient(host, port);
            docment = new Document();
        }

        public void CreateIndex()
        {

            //爬虫文件路径
            string path = ConfigurationManager.AppSettings["path"];
            string pathIndex = ConfigurationManager.AppSettings["pathIndex"];
            bool IsRedis = Convert.ToBoolean(ConfigurationManager.AppSettings["Redis"]);
            //Directory表示索引文件保存的地方，是抽象类，两个子类FSDirectory表示文件中，RAMDirectory 表示存储在内存中
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(pathIndex), new NativeFSLockFactory());

            //判断目录directory是否是一个索引目录。
            bool isUpdate = IndexReader.IndexExists(directory);

            if (isUpdate)
            {
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }

            //第三个参数为是否创建索引文件夹,Bool Create,如果为True，则新创建的索引会覆盖掉原来的索引文件，反之，则不必创建,更新即可。
            IndexWriter write = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate, IndexWriter.MaxFieldLength.UNLIMITED);
            dynamic files;
            if (IsRedis)
            {
                files = GetRedisInfo();
            }
            else
            {
                files = System.IO.Directory.GetFiles(path);
            }
            int count = 0;
            try
            {
                foreach (dynamic item in files)
                {
                    string html = string.Empty;
                    StreamReader file;
                    if (IsRedis)
                    {
                        html = files[item];
                    }
                    else
                    {
                        file = new StreamReader(item);
                        html = file.ReadToEnd();
                        file.Dispose();
                    }
                    HTMLDocumentClass doc = new HTMLDocumentClass();

                    doc.designMode = "on";//不让解析引擎尝试去执行
                    doc.IHTMLDocument2_write(html);
                    doc.close();

                    string url = item.Substring(item.LastIndexOf("\\") + 1, (item.LastIndexOf(".") - item.LastIndexOf("\\") - 1));  //文件名
                    string title = doc.title;
                    string body = doc.body.innerText;
                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(body))
                    {
                        continue;
                    }
                    //为避免重复索引，先输出number=i的记录，在重新添加
                    write.DeleteDocuments(new Term("number", count.ToString()));

                    Document document = new Document();
                    //Field为字段，只有对全文检索的字段才分词，Field.Store是否存储
                    document.Add(new Field("number", url, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", title, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("body", body, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    write.AddDocument(document);
                    // file.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                write.Close();
                directory.Close();
            }
        }

        /// <summary>
        /// redis中读取html
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetRedisInfo()
        {
            List<string> temp = mRedisClient.SearchKeys("*");
            Dictionary<string, string> redisInfo = new Dictionary<string, string>();
            foreach (string item in temp)
            {
                string html = mRedisClient.Get<string>(item);
                if (redisInfo.Keys.Contains(item))
                {
                    continue;
                }
                redisInfo.Add(item, html);
            }
            return redisInfo;
        }

    }
}