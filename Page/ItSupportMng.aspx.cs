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
    public partial class ItSupportMng : PageBase
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
            BindGvItSupport();
            BindGvHotline();
        }

        #region GridView IT support
        private void BindGvItSupport()
        {
            var items = Common.GetItSupports();
            gvItSupports.DataSource = items;
            gvItSupports.DataKeyNames = new string[] { "id" };
            gvItSupports.DataBind();
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
            if (string.IsNullOrEmpty(sname) || string.IsNullOrEmpty(sphone))
            {
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

        #region GridView HotLine        
        
        private void BindGvHotline()
        {
            var items = Common.LoadHotlines();
            gvHotline.DataSource = items;
            gvHotline.DataKeyNames = new string[] { "id" };
            gvHotline.DataBind();

            var cates = Common.LoadHotlineCategories();
            ddlHotlineCategory.DataSource = cates;
            ddlHotlineCategory.DataTextField = "name";
            ddlHotlineCategory.DataValueField = "id";
            ddlHotlineCategory.DataBind();
        }
        //删除
        protected void GVHotline_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowid = e.RowIndex;
            int id = (int)gvHotline.DataKeys[rowid].Value;
            string errmsg = string.Empty;
            if (Common.DeleteHotline(id, out errmsg))
            {
                BindGvHotline();
                //WebMessageBox.ShowSuccess(this, "Add login user ok.");
            }
            else
            {
                WebMessageBox.Show(this, errmsg);
            }
        }

        //取消编辑
        protected void GVHotline_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvHotline.EditIndex = -1;
            BindGvHotline();
        }

        //编辑
        protected void GVHotline_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvHotline.EditIndex = e.NewEditIndex;
            BindGvHotline();
        }

        //更新
        protected void GVHotline_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowid = e.RowIndex;
            int id = (int)gvHotline.DataKeys[rowid].Value;
            using (var mContext = new FavLinkEntities())
            {
                var its = from p in mContext.tbl_hotline
                          where p.id == id
                          select p;
                foreach (var obj in its)
                {
                    var sVal = ((TextBox)(gvHotline.Rows[e.RowIndex].Cells[0].Controls[0])).Text.ToString().Trim();
                    obj.name = sVal;
                    sVal = ((TextBox)(gvHotline.Rows[e.RowIndex].Cells[1].Controls[0])).Text.ToString().Trim();
                    obj.phone_number = sVal;
                }
                mContext.SaveChanges();
            }
            BindGvHotline();
            e.Cancel = true;
        }
        
        protected void btnAddHotline_OnClick(object sender, EventArgs e)
        {
            var sname = txtHotlineName.Text.Trim();
            var sphone = txtHotlinePhone.Text.Trim();
            var scate = ddlHotlineCategory.SelectedValue;
            int ncate = int.Parse(scate);
            if (string.IsNullOrEmpty(sname) || string.IsNullOrEmpty(sphone))
            {
                return;
            }
            string errmsg = string.Empty;
            if (Common.InsertHotline(sname, sphone, ncate, out errmsg))
            {
                BindGvHotline();
                WebMessageBox.ShowSuccess(this, "Add Hotline ok.");
            }
            else
            {
                WebMessageBox.Show(this, errmsg);
            }
        }
        #endregion 
    }
}