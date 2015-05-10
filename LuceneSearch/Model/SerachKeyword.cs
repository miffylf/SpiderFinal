using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuceneSearch
{
    public class SerachKeyword
    {
        public int KeywordID { get; set; }
        public string Keyword { get; set; }
        public DateTime SearchDateTime { get; set; }
        public string ClinetAddress { get; set; }
    }
}