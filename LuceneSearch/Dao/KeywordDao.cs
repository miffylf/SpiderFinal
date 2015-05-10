using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace LuceneSearch
{
    public class KeywordDao
    {
        public IEnumerable<SearchSum> GetSuggestion(string kw)
        {
            DataTable dt = SqlHelper.ExecuteDataTable(@"select top 5 Keyword,count(*) as searchcount  from keywords 
                where datediff(day,searchdatetime,getdate())<7
                and keyword like @keyword
                group by Keyword 
                order by count(*) desc", new SqlParameter("@keyword", "%" + kw + "%"));
            List<SearchSum> list = new List<SearchSum>();
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    SearchSum oneModel = new SearchSum();
                    oneModel.Keyword = Convert.ToString(row["keyword"]);
                    oneModel.SearchCount = Convert.ToInt32(row["SearchCount"]);
                    list.Add(oneModel);
                }
            }
            return list;
        }
        public IEnumerable<SearchSum> GetHotWords()
        {
            //缓存
            var data = HttpRuntime.Cache["hotwords"];
            if (data == null)
            {
                IEnumerable<SearchSum> hotWords = DoSelect();
                HttpRuntime.Cache.Insert("hotwords", hotWords, null, DateTime.Now.AddMilliseconds(30), TimeSpan.Zero);
                return hotWords;
            }
            return (IEnumerable<SearchSum>)data;
        }
        private IEnumerable<SearchSum> DoSelect()
        {
            DataTable dt = SqlHelper.ExecuteDataTable(@"
                                        select top 5 Keyword,count(*) as searchcount  from keywords 
                                        where datediff(day,searchdatetime,getdate())<7
                                        group by Keyword 
                                        order by count(*) desc ");
            List<SearchSum> list = new List<SearchSum>();
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    SearchSum oneModel = new SearchSum();
                    oneModel.Keyword = Convert.ToString(row["keyword"]);
                    oneModel.SearchCount = Convert.ToInt32(row["SearchCount"]);
                    list.Add(oneModel);
                }
            }
            return list;
        }

        public int Add
           (SerachKeyword searchKeyword)
        {
            string sql = "INSERT INTO Keywords (SearchDateTime, KeyWord, ClientAddress)  output inserted.KeywordID VALUES (@SearchDateTime, @KeyWord, @ClientAddress)";
            SqlParameter[] para = new SqlParameter[]
					{
						new SqlParameter("@SearchDateTime",searchKeyword.SearchDateTime),
						new SqlParameter("@KeyWord", searchKeyword.Keyword),
						new SqlParameter("@ClientAddress", searchKeyword.ClinetAddress),
					};
            return SqlHelper.ExecuteNonQuery(sql, para);


        }


    }
}