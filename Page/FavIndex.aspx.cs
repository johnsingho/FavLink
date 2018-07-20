using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FavLink.Utility;
using FavLink.Entity;

namespace FavLink.Page
{
    public partial class FavIndex : System.Web.UI.Page
    {
        protected string ReUrl
        {
            set { ViewState["ReUrl"] = value; }
            get { return ViewState["ReUrl"] == null ? "" : ViewState["ReUrl"].ToString(); }
        }

        private string _catID;
        protected string CateID
        {
            get { return _catID; }
            set { _catID = value; }
        }

        private string _searchName;
        protected string SearchName
        {
            get { return _searchName; }
            set { _searchName = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReUrl = Request.QueryString["reurl"];
                var scate = Request.QueryString["cateid"];
                CateID = string.IsNullOrEmpty(scate) ? string.Empty : scate.Trim();
                SearchName = Request.QueryString["searchName"];

                this.Response.Buffer = true;
                DateTime dt = DateTime.Now;
                this.Response.ExpiresAbsolute = dt.AddHours(-1);
                this.Response.Expires = 0;
                this.Response.CacheControl = "no-cache";

                InitLoadPageInfo();
            }
        }

        private void InitLoadPageInfo()
        {
            var data = LoadLinks();
            rptLinks.DataSource = data;
            rptLinks.DataBind();
        }

        private List<tbl_link_data> LoadLinks()
        {
            //int userID = CurrentUserInfo.id;
            if (!string.IsNullOrEmpty(CateID))
            {
                return Common.LoadLinksByCateID(CateID);
            }
            else if (!string.IsNullOrEmpty(SearchName))
            {
                return Common.LoadLinksByName(SearchName);
            }
            else
            {
                //default
                return Common.LoadLinksByCateID("1");
            }            
        }
    }
}