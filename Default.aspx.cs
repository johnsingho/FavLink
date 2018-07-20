using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FavLink
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sDefUrl = "Page/FavIndex.aspx?cateid=1";
            Response.Redirect(sDefUrl);
        }
    }
}