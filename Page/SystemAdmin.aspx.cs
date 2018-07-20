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
    public partial class SystemAdmin : PageBase
    {
        //private FavLinkEntities mContext = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitPageInfo();
            }
        }

        private void InitPageInfo()
        {   
            BindGvUser();
            BindGvItSupport();
            BindGvItSupportArrangements();
            BindGvItSupportShift();
            BindLinks();
        }
        #region GridView User
        private void BindGvUser()
        {
            using (var mContext = new FavLinkEntities())
            { 
                var users = (from x in mContext.tbl_user select x);
                gvUsers.DataSource = users;
                gvUsers.DataKeyNames = new string[] { "id" };
                gvUsers.DataBind();
            }
        }

        //删除
        protected void GVUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowid = e.RowIndex;
            int id = (int)gvUsers.DataKeys[rowid].Value;
            Common.DeleteUser(id);
            BindGvUser();
        }

        //取消编辑
        protected void GVUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            BindGvUser();
        }

        //编辑
        protected void GVUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            BindGvUser();
        }

        //更新
        protected void GVUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowid = e.RowIndex;
            int id = (int)gvUsers.DataKeys[rowid].Value;
            using (var mContext = new FavLinkEntities())
            {
                var persons = from p in mContext.tbl_user
                    where p.id == id
                    select p;
                foreach (var obj in persons)
                {
                    var sVal = ((TextBox) (gvUsers.Rows[e.RowIndex].Cells[4].Controls[0])).Text.ToString().Trim();
                    obj.IsValid = bool.Parse(sVal);
                    //sVal = ((TextBox) (gvUsers.Rows[e.RowIndex].Cells[5].Controls[0])).Text.ToString().Trim();
                    //obj.IsAdmin = bool.Parse(sVal);
                }
                mContext.SaveChanges();
            }
            BindGvUser();
        }

        protected void btnAddUser_OnClick(object sender, EventArgs e)
        {
            var sinput = txtNewADAccount.Text.Trim();
            if (string.IsNullOrEmpty(sinput)) { return; }
            string errmsg = string.Empty;
            if (Common.InsertUserInfo(sinput, ref errmsg))
            {
                BindGvUser();
                WebMessageBox.ShowSuccess(this, "Add login user ok.");
            }
            else
            {
                WebMessageBox.Show(this, errmsg);
            }
        }
        #endregion

        #region GridView IT support 
        private void BindGvItSupport()
        {
            using (var mContext = new FavLinkEntities())
            { 
                var items = (from x in mContext.tbl_itsupport select x);
                gvItSupports.DataSource = items;
                gvItSupports.DataKeyNames = new string[] { "id" };
                gvItSupports.DataBind();

                ddlItSupportUsers.DataSource = items;
                ddlItSupportUsers.DataTextField = "name";
                ddlItSupportUsers.DataValueField = "id";
                ddlItSupportUsers.DataBind();
            }
        }

        //删除
        protected void GVItSupports_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowid = e.RowIndex;
            int id = (int)gvItSupports.DataKeys[rowid].Value;
            string errmsg = string.Empty;
            if (Common.DeleteItSupport(id, out errmsg))
            {
                BindGvItSupport();
                //WebMessageBox.ShowSuccess(this, "Add login user ok.");
            }
            else
            {
                WebMessageBox.Show(this, errmsg);
            }
        }

        //取消编辑
        protected void GVItSupports_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItSupports.EditIndex = -1;
            BindGvItSupport();
        }

        //编辑
        protected void GVItSupports_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvItSupports.EditIndex = e.NewEditIndex;
            BindGvItSupport();
        }

        //更新
        protected void GVItSupports_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowid = e.RowIndex;
            int id = (int)gvItSupports.DataKeys[rowid].Value;
            using (var mContext = new FavLinkEntities())
            {
                var its = from p in mContext.tbl_itsupport
                          where p.id == id
                          select p;
                foreach (var obj in its)
                {
                    var sVal = ((TextBox)(gvItSupports.Rows[e.RowIndex].Cells[0].Controls[0])).Text.ToString().Trim();
                    obj.name = sVal;
                    sVal = ((TextBox)(gvItSupports.Rows[e.RowIndex].Cells[1].Controls[0])).Text.ToString().Trim();
                    obj.phone_number = sVal;
                }
                mContext.SaveChanges();
            }
            BindGvItSupport();
            e.Cancel = true; //!temp
        }

        protected void btnAddItSupport_OnClick(object sender, EventArgs e)
        {
            var sname = txtItName.Text.Trim();
            var sphone = txtItPhone.Text.Trim();
            if (string.IsNullOrEmpty(sname) || string.IsNullOrEmpty(sphone)) {
                return;
            }
            string errmsg = string.Empty;
            if (Common.InsertItSupport(sname, sphone, out errmsg))
            {
                BindGvItSupport();
                WebMessageBox.ShowSuccess(this, "Add IT support ok.");
            }
            else
            {
                WebMessageBox.Show(this, errmsg);
            }
        }
        #endregion

        #region GridView It support arrangement
        protected class TItSupportArrangement
        {
            public int id { get; set; }
            public string ITSupportName { get; set; }
            public string project { get; set; }
            public int shift { get; set; }
            public int month { get; set; }
            public string shiftName
            {
                get { return EnumShiftUtil.GetShiftStr(shift); }                
            }
        }

        private void BindGvItSupportArrangements()
        {
            using (var mContext = new FavLinkEntities())
            {
                var items = from x in mContext.tbl_itsupport_arrangment
                            select new TItSupportArrangement()
                            {
                                id=x.id,
                                ITSupportName=x.tbl_itsupport.name,
                                project=x.project,
                                shift = x.shift.HasValue ? x.shift.Value : 0,
                                month = x.month.HasValue ? x.month.Value : 0
                            };
                gvItShift.DataSource = items;
                gvItShift.DataKeyNames = new string[] { "id" };
                gvItShift.DataBind();
            }
        }

        protected class TShift
        {
            public string name { get; set; }
            public int value { get; set; }
        }
        private void BindGvItSupportShift()
        {
            List<TShift> shifts = new List<TShift>();
            shifts.Add(new TShift { name = "Morning Shift", value = 0 });
            shifts.Add(new TShift { name = "Middle Shift", value = 1 });
            shifts.Add(new TShift { name = "Morning Night", value = 2 });

            ddlItSupportShifts.DataSource = shifts;
            ddlItSupportShifts.DataTextField = "name";
            ddlItSupportShifts.DataValueField = "value";
            ddlItSupportShifts.DataBind();
        }
        //删除
        protected void GVItShift_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowid = e.RowIndex;
            int id = (int)gvItShift.DataKeys[rowid].Value;
            using (var mContext = new FavLinkEntities())
            {
                var its = from p in mContext.tbl_itsupport_arrangment
                            where p.id == id
                            select p;
                if(its.Any())
                {
                    var obj = its.First();
                    mContext.tbl_itsupport_arrangment.DeleteObject(obj);
                }
                mContext.SaveChanges();
            }
            BindGvItSupportArrangements();
        }

        //取消编辑
        protected void GVItShift_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItShift.EditIndex = -1;
            BindGvItSupportArrangements();
        }

        //编辑
        protected void GVItShift_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvItShift.EditIndex = e.NewEditIndex;
            BindGvItSupportArrangements();
        }

        protected void btnAddItShift_OnClick(object sender, EventArgs e)
        {
            var sCurUser = ddlItSupportUsers.SelectedValue;
            var sCurShift = ddlItSupportShifts.SelectedValue;
            var sProject = txtItSupportProject.Text.Trim();
            var nMonth = 0;
            try
            {
                nMonth = int.Parse(txtShiftMonth.Text.Trim());
            }
            catch (Exception)
            {
            }
            
            if (!(1<=nMonth && nMonth<=12))
            {
                WebMessageBox.Show(this, "month is not valid!");
                return;
            }

            string errmsg=string.Empty;
            if (Common.InsertItSupportArrangement(int.Parse(sCurUser), int.Parse(sCurShift), sProject, nMonth, out errmsg))
            {
                BindGvItSupportArrangements();
                WebMessageBox.Show(this, "Add IT support arrangement ok.");
            }
            else
            {
                WebMessageBox.Show(this, errmsg);
            }
        }

        private int GetShiftValue(string sShift)
        {
            if (sShift.StartsWith("Morning", StringComparison.InvariantCultureIgnoreCase))
            {
                return (int)EnumShift.MorningShift;
            }
            else if (sShift.StartsWith("Middle", StringComparison.InvariantCultureIgnoreCase))
            {
                return (int)EnumShift.MiddleShift;
            }
            else if (sShift.StartsWith("Night", StringComparison.InvariantCultureIgnoreCase))
            {
                return (int)EnumShift.NightShift;
            }
            return -1;
        }

        protected void GVItShift_SelectedIndexChanged(Object sender, EventArgs e)
        {
            // Get the currently selected row using the SelectedRow property.
            GridViewRow row = gvItShift.SelectedRow;
            var suser = row.Cells[1].Text;
            var it = ddlItSupportUsers.Items.FindByText(suser);
            if (it != null)
            {
                ddlItSupportUsers.SelectedValue = it.Value;
            }
            
            txtItSupportProject.Text = row.Cells[2].Text;
            ddlItSupportShifts.SelectedValue = GetShiftValue(row.Cells[3].Text.Trim()).ToString();
            txtShiftMonth.Text = row.Cells[4].Text;            
        }

        protected void btnUpdateItShift_OnClick(object sender, EventArgs e)
        {
            GridViewRow row = gvItShift.SelectedRow;
            var id = int.Parse(row.Cells[0].Text);

            var sCurUser = ddlItSupportUsers.SelectedValue;
            var sCurShift = ddlItSupportShifts.SelectedValue;
            var sProject = txtItSupportProject.Text.Trim();
            var nMonth = 0;
            try
            {
                nMonth = int.Parse(txtShiftMonth.Text.Trim());
            }
            catch (Exception)
            {
            }

            if (!(1 <= nMonth && nMonth <= 12))
            {
                WebMessageBox.Show(this, "month is not valid!");
                return;
            }

            string errmsg = string.Empty;
            if (Common.UpdateItSupportArrangement(id, int.Parse(sCurUser), int.Parse(sCurShift), sProject, nMonth, out errmsg))
            {
                BindGvItSupportArrangements();
                WebMessageBox.Show(this, "Update IT support arrangement ok.");
            }
            else
            {
                WebMessageBox.Show(this, errmsg);
            }
        }
        #endregion

        #region Links data
        private void BindLinks()
        {
            if (CurrentUserInfo == null)
            {
                return;
            }
            var links = Common.LoadUserLinks(CurrentUserInfo.id);
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


        #endregion


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
            if (Common.UpdateLinkData(lid, linkName,
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
    }
}