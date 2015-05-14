//======================================================================
// 所属项目：Spider
// 创 建 人：lifei
// 创建日期：2015/5/2
// 用    途：分页
//====================================================================== 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LuceneSearch.Logic
{
    public class MyPage
    {
        #region 分页方法
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPageIndex { get; set; }
        public int MaxPageCount { get; set; }
        public string UrlFormat { get; set; }
        public MyPage()
        {
            PageSize = 10;
            MaxPageCount = 10;
        }
        private void Check()
        {
            System.Diagnostics.Debug.Assert(PageSize > 0);
            System.Diagnostics.Debug.Assert(CurrentPageIndex > 0);
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(UrlFormat));
        }

        public void TryParseCurrentPageIndex(string pn)
        {
            int temp;
            if (int.TryParse(pn, out temp))
            {
                CurrentPageIndex = temp;
            }
            else
            {
                CurrentPageIndex = 1;
            }
        }
        public string RenderToHTML()
        {
            Check();
            StringBuilder sb = new StringBuilder();
            double tempCount = TotalCount / PageSize;
            int pageCount = (int)Math.Ceiling(tempCount);
            int visibleStart = CurrentPageIndex - MaxPageCount / 2;
            if (visibleStart < 1)//6还是没区别
            {
                visibleStart = 1;
            }
            int visibleEnd = visibleStart + MaxPageCount;
            if (visibleEnd > pageCount)
            {
                visibleEnd = pageCount;
            }
            if (CurrentPageIndex > 1)
            {
                sb.Append(GetPageLink(1, "首页"));
                sb.Append(GetPageLink(CurrentPageIndex - 1, "上一页"));
            }
            else
            {
                sb.Append("<span>首页</span>");
                sb.Append("<span>上一页</span>");
            }
            for (int i = visibleStart; i <= visibleEnd; i++)
            {
                if (i == CurrentPageIndex)
                {
                    sb.Append("<span>").Append(i).Append("</span>");
                }
                else
                {
                    sb.Append(GetPageLink(i, i.ToString()));
                }
            }
            if (CurrentPageIndex < pageCount)
            {
                sb.Append(GetPageLink(CurrentPageIndex + 1, "下一页"));
                sb.Append(GetPageLink(pageCount, "末页"));
            }
            else
            {
                sb.Append("<span>下一页</span>");
                sb.Append("<span>末页</span>");
            }
            return sb.ToString();
        }
        private string GetPageLink(int i, string text)
        {
            StringBuilder sb = new StringBuilder();
            string url = UrlFormat.Replace("{n}", i.ToString());
            sb.Append("<a href='").Append(url).Append("'>").Append(text).Append("</a>");
            return sb.ToString();
        }
        #endregion
    }
}