//======================================================================
// 所属项目：Spider
// 创 建 人：lifei
// 创建日期：2015/5/2
// 用    途：实体
//====================================================================== 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuceneSearch.Logic
{
    public class SearchResult
    {
        public string Number { get; set; }
        public string Title { get; set; }
        public string BodyPreview { get; set; }
    }
}