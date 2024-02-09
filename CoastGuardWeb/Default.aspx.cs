using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Utils;

namespace CoastGuardWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        private static readonly Logger Logger = new Logger(typeof(_Default));
        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.Info("Starting Web");
            Response.Redirect("login.htm");
           
        }
    }
}
