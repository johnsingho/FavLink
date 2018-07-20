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
    public partial class UserMng : PageBase
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
            BindGvUser();
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
            int curUserID = CurrentUserInfo.id;
            string errmsg = string.Empty;
            bool bRet = Common.DeleteUser(id, out errmsg);
            if ( bRet && id==curUserID)
            {
                //用户自杀, redirect to login page
                Response.Redirect(ResolveUrl("~/Login/Signout.ashx"));
                return;
            }
            else if(!bRet)
            {
                WebMessageBox.Show(this, errmsg);
                return;
            }
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
            int uid = (int)gvUsers.DataKeys[rowid].Value;

            var sVal = ((TextBox)(gvUsers.Rows[e.RowIndex].Cells[4].Controls[0])).Text.ToString().Trim();
            bool bEnabled = bool.Parse(sVal);
            string errmsg = string.Empty;
            Common.EnableUser(uid, bEnabled, out errmsg);
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
    }
}