//======================================================================
// 所属项目：Spider
// 创 建 人：lifei
// 创建日期：2015/5/2
// 用    途：分词
//====================================================================== 
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LuceneSearch.Logic
{
    public class CommonHelper
    {
        #region 分词
        //分词，注意这儿的分词算法要和前面的算法一致，都为PanGu分词
        public static string[] SplitWord(string str)
        {
            List<string> list = new List<string>();
            Analyzer analyzer = new PanGuAnalyzer();
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(str));
            Token token = null;
            while ((token = tokenStream.Next()) != null)
            {
                list.Add(token.TermText());
            }
            return list.ToArray();
        }
        #endregion
    }
}