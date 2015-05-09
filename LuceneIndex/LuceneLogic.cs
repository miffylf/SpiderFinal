using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using mshtml;
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

        public LuceneLogic()
        {
            docment = new Document();
        }

        public void CreateIndex()
        {

            //爬虫文件路径
            string path = ConfigurationManager.AppSettings["path"];
            string pathIndex = ConfigurationManager.AppSettings["pathIndex"];

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

            var files = System.IO.Directory.GetFiles(path);
            int count = 0;
            try
            {
                foreach (var item in files)
                {
                    StreamReader file = new StreamReader(item);
                    string html = file.ReadToEnd();
                    HTMLDocumentClass doc = new HTMLDocumentClass();

                    doc.designMode = "on";//不让解析引擎尝试去执行
                    doc.IHTMLDocument2_write(html);
                    doc.close();

                    string url = item.Substring(item.LastIndexOf("\\") + 1, (item.LastIndexOf(".") - item.LastIndexOf("\\") - 1));  //文件名
                    string title = doc.title;
                    string body = doc.body.innerText;
                    //为避免重复索引，先输出number=i的记录，在重新添加
                    write.DeleteDocuments(new Term("number", count.ToString()));

                    Document document = new Document();
                    //Field为字段，只有对全文检索的字段才分词，Field.Store是否存储
                    document.Add(new Field("number", url, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", title, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("body", body, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));
                    write.AddDocument(document);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            write.Close();
            directory.Close();
        }

    }
}