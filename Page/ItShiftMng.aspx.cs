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
    public partial class ItShiftMng : PageBase
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
            BindGvItSupportArrangements();
            BindGvItSupportShift();
            BindGvMonths();
        }

        #region GridView It support arrangement
        private void BindGvMonths()
        {
            var items = new List<string>();
            for(int i=1; i<13; i++)
            {
                items.Add(i.ToString());
            }            
            var mon = DateTime.Now.Month;
            ddlShiftMonth.DataSource = items;
            ddlShiftMonth.DataBind();
            ddlShiftMonth.SelectedIndex = mon - 1;

            var itemFilters = items.ToList();
            itemFilters.Add("All");
            ddlFilterMonth.DataSource = itemFilters;
            //ddlFilterMonth.DataTextField = "name";
            //ddlFilterMonth.DataValueField = "id";
            ddlFilterMonth.DataBind();
            ddlFilterMonth.SelectedIndex = ddlFilterMonth.Items.Count - 1;
        }
        private void BindGvItSupport()
        {
            var items = Common.GetItSupports();
            ddlItSupportUsers.DataSource = items;
            ddlItSupportUsers.DataTextField = "name";
            ddlItSupportUsers.DataValueField = "id";
            ddlItSupportUsers.DataBind();
        }

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
                                id = x.id,
                                ITSupportName = x.tbl_itsupport.name,
                                project = x.project,
                                shift = x.shift.HasValue ? x.shift.Value : 0,
                                month = x.month.HasValue ? x.month.Value : 0
                            };
                gvItShift.DataSource = items;
                gvItShift.DataKeyNames = new string[] { "id" };
                gvItShift.DataBind();
            }
        }
        private void BindGvItSupportArrangementsByMonth(int nMonth)
        {
            using (var mContext = new FavLinkEntities())
            {
                var items = from x in mContext.tbl_itsupport_arrangment
                            where x.month==nMonth
                            select new TItSupportArrangement()
                            {
                                id = x.id,
                                ITSupportName = x.tbl_itsupport.name,
                                project = x.project,
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
            shifts.Add(new TShift { name = "Night Shift", value = 2 });

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
                if (its.Any())
                {
                    var obj = its.First();
                    mContext.tbl_itsupport_arrangment.DeleteObject(obj);
                }
                mContext.SaveChanges();
            }
            FilterShowByMon();
        }

        //取消编辑
        protected void GVItShift_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItShift.EditIndex = -1;
            FilterShowByMon();
        }

        //编辑
        protected void GVItShift_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvItShift.EditIndex = e.NewEditIndex;
            FilterShowByMon();
        }

        protected void btnAddItShift_OnClick(object sender, EventArgs e)
        {
            var sCurUser = ddlItSupportUsers.SelectedValue;
            var sCurShift = ddlItSupportShifts.SelectedValue;
            var sProject = txtItSupportProject.Text.Trim();
            var nMonth = 0;
            try
            {
                //nMonth = int.Parse(txtShiftMonth.Text.Trim());
                var sval = ddlShiftMonth.SelectedValue;
                nMonth = int.Parse(sval);
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
            if (Common.InsertItSupportArrangement(int.Parse(sCurUser), int.Parse(sCurShift), sProject, nMonth, out errmsg))
            {
                FilterShowByMon();
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
            var sid = row.Cells[0].Text;
            int shiftID = int.Parse(sid);
            var objShift = Common.GetItSupportArrangement(shiftID);
            if (objShift != null)
            {
                var it = ddlItSupportUsers.Items.FindByText(objShift.suser);
                if (it != null)
                {
                    ddlItSupportUsers.SelectedValue = it.Value;
                }
            }
            txtItSupportProject.Text = objShift.sproject;
            ddlItSupportShifts.SelectedValue = objShift.shift.ToString();
            ddlShiftMonth.SelectedIndex = objShift.month - 1;

            //var suser = row.Cells[1].Text;
            //var it = ddlItSupportUsers.Items.FindByText(suser);
            //if (it != null)
            //{
            //    ddlItSupportUsers.SelectedValue = it.Value;
            //}
            //txtItSupportProject.Text = row.Cells[2].Text;
            //ddlItSupportShifts.SelectedValue = GetShiftValue(row.Cells[3].Text.Trim()).ToString();
            //ddlShiftMonth.SelectedIndex = int.Parse(row.Cells[4].Text) - 1;
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
                //nMonth = int.Parse(txtShiftMonth.Text.Trim());
                var sval = ddlShiftMonth.SelectedValue;
                nMonth = int.Parse(sval);
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
                FilterShowByMon();
                WebMessageBox.Show(this, "Update IT support arrangement ok.");
            }
            else
            {
                WebMessageBox.Show(this, errmsg);
            }
        }

        protected void ddlFilterMonth_SelectedChanged(Object sender, EventArgs e)
        {
            FilterShowByMon();
        }

        private void FilterShowByMon()
        {
            int nMon = -1;
            try
            {
                nMon = int.Parse(ddlFilterMonth.SelectedValue);
            }
            catch (Exception)
            {
            }
            if (nMon <= 0)
            {
                BindGvItSupportArrangements();
            }
            else
            {
                BindGvItSupportArrangementsByMonth(nMon);
            }
        }
        #endregion
    }
}