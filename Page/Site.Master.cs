using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FavLink.Entity;

namespace FavLink
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitPageInfo();
        }

        private void InitPageInfo()
        {
            var user = Business.Authorization.UserState.GetLoginUser();
            if (user != null)
            {
                litUser.Text = user.Email;
            }
            
            //load category
            rptCate.DataSource = Common.LoadCategories();
            rptCate.DataBind();
        }
    }
}
