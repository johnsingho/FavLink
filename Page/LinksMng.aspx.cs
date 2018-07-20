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
    public partial class LinksMng : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitPageInfo();
            }
        }

        private void InitPageInfo()
        {
            BindLinks();
        }

        #region Links data
        private void BindLinks()
        {
            if (CurrentUserInfo == null)
            {
                return;
            }
            //var links = Common.LoadUserLinks(CurrentUserInfo.id);
            var links = Common.LoadUserLinks(-1);
            rptLinks.DataSource = links;
            rptLinks.DataBind();

            var cates = Common.LoadCategories();
            ddlLinkCate.DataSource = cates;
            ddlLinkCate.DataTextField = "name";
            ddlLinkCate.DataValueField = "id";
            ddlLinkCate.DataBind();

            var icons = GetDefaultIcons();
            rptIcons.DataSource = icons;
            rptIcons.DataBind();

            var colors = GetDefaultColors();
            rptColors.DataSource = colors;
            rptColors.DataBind();
        }


        private List<ThzxStr> GetDefaultColors()
        {
            var colors = new List<ThzxStr>();
            colors.Add(new ThzxStr("red"));
            colors.Add(new ThzxStr("blue"));
            colors.Add(new ThzxStr("yellow"));
            colors.Add(new ThzxStr("blueDark"));
            colors.Add(new ThzxStr("orange"));
            colors.Add(new ThzxStr("purple"));
            colors.Add(new ThzxStr("green"));
            colors.Add(new ThzxStr("greenDark"));
            colors.Add(new ThzxStr("greenLight"));
            colors.Add(new ThzxStr("pink"));
            colors.Add(new ThzxStr("black"));
            return colors;
        }


        protected class ThzxStr
        {
            internal ThzxStr(string savl)
            {
                Value = savl;
            }
            public string Value { get; set; }
        }
        private List<ThzxStr> GetDefaultIcons()
        {
            var icons = new List<ThzxStr>();
            icons.Add(new ThzxStr("fa-gamepad"));
            icons.Add(new ThzxStr("fa-gavel"));
            icons.Add(new ThzxStr("fa-gear"));
            icons.Add(new ThzxStr("fa-gears"));
            icons.Add(new ThzxStr("fa-gift"));
            icons.Add(new ThzxStr("fa-glass"));
            icons.Add(new ThzxStr("fa-globe"));
            icons.Add(new ThzxStr("fa-group"));
            icons.Add(new ThzxStr("fa-hdd-o"));
            icons.Add(new ThzxStr("fa-headphones"));
            icons.Add(new ThzxStr("fa-heart"));
            icons.Add(new ThzxStr("fa-heart-o"));
            icons.Add(new ThzxStr("fa-home"));
            icons.Add(new ThzxStr("fa-comments"));
            icons.Add(new ThzxStr("fa-envelope-o"));
            icons.Add(new ThzxStr("fa-barcode"));
            icons.Add(new ThzxStr("fa-bars"));
            icons.Add(new ThzxStr("fa-beer"));
            icons.Add(new ThzxStr("fa-bell"));
            icons.Add(new ThzxStr("fa-bell-o"));
            icons.Add(new ThzxStr("fa-bolt"));
            icons.Add(new ThzxStr("fa-book"));
            icons.Add(new ThzxStr("fa-bookmark"));
            icons.Add(new ThzxStr("fa-bookmark-o"));
            icons.Add(new ThzxStr("fa-briefcase"));
            icons.Add(new ThzxStr("fa-bug"));
            icons.Add(new ThzxStr("fa-building-o"));
            icons.Add(new ThzxStr("fa-bullhorn"));
            icons.Add(new ThzxStr("fa-bullseye"));
            icons.Add(new ThzxStr("fa-calendar"));
            icons.Add(new ThzxStr("fa-calendar-o"));
            icons.Add(new ThzxStr("fa-camera"));
            icons.Add(new ThzxStr("fa-camera-retro"));
            icons.Add(new ThzxStr("fa-caret-square-o-down"));
            icons.Add(new ThzxStr("fa-caret-square-o-left"));
            icons.Add(new ThzxStr("fa-caret-square-o-right"));
            icons.Add(new ThzxStr("fa-caret-square-o-up"));
            icons.Add(new ThzxStr("fa-certificate"));
            icons.Add(new ThzxStr("fa-check"));
            icons.Add(new ThzxStr("fa-check-circle"));
            icons.Add(new ThzxStr("fa-check-circle-o"));
            icons.Add(new ThzxStr("fa-check-square"));
            icons.Add(new ThzxStr("fa-frown-o"));
            return icons;
        }

        protected void btnAddLink_OnClick(object sender, EventArgs e)
        {
            var linkName = txtLinkName.Value.Trim();
            var linkUrl = txtLinkUrl.Value.Trim();
            var linkIcon = hidSelectedIcon.Value.Trim();
            var linkColor = hidSelectedColor.Value.Trim();
            var linkCate = ddlLinkCate.SelectedValue;

            if (string.IsNullOrEmpty(linkName)
                || string.IsNullOrEmpty(linkUrl)
                || string.IsNullOrEmpty(linkIcon)
                || string.IsNullOrEmpty(linkColor)
            )
            {
                WebMessageBox.Show(this, "有部分选项值未提供。Some required value(s) haven't provide!");
                return;
            }

            //fix for <a>
            linkUrl = FixUrl(linkUrl);
            var errmsg = string.Empty;
            if (Common.InsertLinkData(CurrentUser.UserID, linkName,
                        linkUrl, linkIcon, linkColor, linkCate, out errmsg))
            {
                BindLinks();
                WebMessageBox.ShowSuccess(this, "Add Link ok.");
            }
            else
            {
                WebMessageBox.Show(this, errmsg);
            }
        }

        private static string FixUrl(string linkUrl)
        {
            if (!linkUrl.StartsWith("http:", StringComparison.InvariantCultureIgnoreCase)
                && !linkUrl.StartsWith("https:", StringComparison.InvariantCultureIgnoreCase)
                && !linkUrl.StartsWith("\\\\", StringComparison.InvariantCultureIgnoreCase)
                )
            {
                linkUrl = "http://" + linkUrl;
            }
            return linkUrl;
        }

        protected void btnUpdateLink_OnClick(object sender, EventArgs e)
        {
            var slid = hidSelectedLink.Value.Trim();
            if (string.IsNullOrEmpty(slid))
            {
                WebMessageBox.Show(this, "Please, select a link first!");
                return;
            }
            var lid = int.Parse(slid);
            var linkName = txtLinkName.Value.Trim();
            var linkUrl = txtLinkUrl.Value.Trim();
            var linkIcon = hidSelectedIcon.Value.Trim();
            var linkColor = hidSelectedColor.Value.Trim();
            var linkCate = ddlLinkCate.SelectedValue;

            //fix for <a>
            linkUrl = FixUrl(linkUrl);
            var errmsg = string.Empty;
            if (Common.UpdateLinkData(CurrentUser.UserID, lid, linkName,
                        linkUrl, linkIcon, linkColor, linkCate, out errmsg))
            {
                BindLinks();
                WebMessageBox.ShowSuccess(this, "Update Link ok.");
            }
            else
            {
                WebMessageBox.Show(this, errmsg);
            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteLink(string linkid)
        {
            int lid = -1;
            try
            {
                lid = int.Parse(linkid);
            }
            catch (Exception)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new { ret = 0, errMsg = "Invalid link ID" });
            }
            string msg = string.Empty;
            bool bOk = Common.DeleteLinkByID(lid, out msg);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { ret = bOk ? 1 : 0, errMsg = msg });
        }
        #endregion
    }
}