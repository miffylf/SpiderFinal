using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Index
{
    public class CreateIndex
    {
        /// <summary>
        /// 从一段HTML文本中提取出一定字数的纯文本
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string GetString(string html)
        {
            string temp = html.Clone().ToString();
            temp = new Regex(@"(?m)<script[^>]*>(\w|\W)*?</script[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(temp, "");
            temp = new Regex(@"(?m)<style[^>]*>(\w|\W)*?</style[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(temp, "");
            temp = new Regex(@"(?m)<select[^>]*>(\w|\W)*?</select[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase).Replace(temp, "");
            Regex objReg = new System.Text.RegularExpressions.Regex("(<[^>]+?>)|&nbsp;", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            temp = objReg.Replace(temp, "");
            Regex objReg2 = new System.Text.RegularExpressions.Regex("(\\s)+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            temp = objReg2.Replace(temp, " ");
            return temp;
        }
    }
}
