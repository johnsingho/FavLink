using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using FavLink.Business.Authorization;
using FavLink.Entity;

namespace FavLink.Utility
{
    public class PageBase : System.Web.UI.Page
    {
        private UserBasicInfo _currentUser = null;
        
        //页面标题
        public static string GetPublicPageTitle()
        {
            return "B11 IT System";
        }

        /// <summary>
        /// 获取当前登录的用户, 该值可能为 null
        /// </summary>
        public UserBasicInfo CurrentUser
        {
            get
            {
                if (!UserState.IsLogin)
                    return null;
                else
                {
                    if (_currentUser == null)
                    {
                        _currentUser = UserState.GetLoginUser();
                    }
                    return _currentUser;
                }
            }
        }

        /// <summary>
        /// 获取当前用户的信息
        /// </summary>
        public tbl_user CurrentUserInfo
        {
            get
            {
                return UserState.GetLoginUserUserInfo();
            }
        }
        public string Readonly
        {
            get
            {
                //System.Web.HttpRequest oHttpRequest;
                string sTemp = "";
                //oHttpRequest.

                if (Request.QueryString["Readonly"] != null)
                {
                    sTemp = Request.QueryString["Readonly"].ToString();
                }
                //return sTemp;测试阶段都可写--Carter2012-11-16
                return "Write";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            // 如果没有登录
            if (!UserState.IsLogin)
            {
                var sLoc = ResolveUrl("~/Login/Signin.aspx?reurl=") + System.Web.HttpUtility.UrlEncode(Request.RawUrl);
                Response.Redirect(sLoc);
                //Response.Write(string.Format(
                //    "<script type=\"text/javascript\">top.location.href='{0}Login.aspx';</script>",
                //    Utility.WebUtility.SiteVirtualRoot
                //    ));
                Response.End();
            }
        }

        protected string EditKey
        {
            get { return Request.QueryString["eid"]; }
        }
    }
}
