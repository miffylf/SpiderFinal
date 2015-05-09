using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LuceneIndex
{
    public partial class CreateIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LuceneLogic llog = new LuceneLogic();
            llog.CreateIndex();
        }
    }
}