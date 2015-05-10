using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace LuceneSearch
{
    /// <summary>
    /// SearchSuggestion 的摘要说明
    /// </summary>
    public class SearchSuggestion : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string kw = context.Request["term"];
            var result = new KeywordDao().GetSuggestion(kw);
            List<string> list = new List<string>();
            foreach (var item in result)
            {
                list.Add(item.Keyword);
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string json = jss.Serialize(list);
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}