using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FavLink.Entity;
using Utility;
using System.Text;

namespace FavLink.Page
{
    public partial class Contactus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitLoadPageInfo();
            }
        }

        private void InitLoadPageInfo()
        {
            var contracts = Common.LoadContractInfo(DateTime.Now.Month);
            rptLinks.DataSource = contracts;
            rptLinks.DataBind();
        }

        protected string GetRowClass(int row)
        {
            return (0 == row % 2) ? "odd" : "even";
        }

        protected string GetShiftStr(object eval)
        {
            return EnumShiftUtil.GetShiftStr((int)eval);
        }
        private string FormatPhoneNumber(string sphone)
        {
            var lst = new List<string>();
            int nLen = sphone.Length;
            int n = nLen;
            for (; n > 4; n -= 4)
            {
                var s = sphone.Substring(n - 4, 4);
                lst.Add(s);
            }
            if (n <= 4)
            {
                var s = sphone.Substring(0, n);
                lst.Add(s);
            }
            lst.Reverse();
            return string.Join(" ", lst);
        }

        protected string GetHotlinesList()
        {
            var hotLines = Common.LoadHotlines();
            var sbHotline = new StringBuilder();
            sbHotline.Append("<h4 class='underline'>Hot Line</h4><div class='row'><div class='col-sm-2 lsLeft'><table class='table'> ");
            var sbEscalation = new StringBuilder();
            sbEscalation.Append("<h4 class='underline'>Escalation</h4><div class='row'><div class='col-sm-2 lsLeft'><table class='table'>");
            foreach (var item in hotLines)
            {
                var scolor = "green";
                if (item.category == 1)
                {
                    if (0 == item.name.CompareTo("MES")) { scolor = "blue"; }
                    sbHotline.AppendFormat("<tr class='{2}'><td class='text-right'>{0}:</td><td>{1}</td></tr>",
                        item.name, FormatPhoneNumber(item.phone_number), scolor);
                }
                else if (item.category == 2)
                {
                    sbEscalation.AppendFormat("<tr class='{2}'><td class='text-right'>{0}:</td><td>{1}</td></tr>",
                        item.name, FormatPhoneNumber(item.phone_number), scolor);
                }
            }
            sbHotline.Append("</table></div></div>");
            sbEscalation.Append("</table></div></div>");
            sbHotline.Append(sbEscalation);
            return sbHotline.ToString();
        }
        protected string GetYearMon()
        {
            var now = DateTime.Now;
            return now.ToString("yyyy/MM");
        }
    }
}