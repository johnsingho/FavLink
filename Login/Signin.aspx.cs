using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.UI;
using FavLink.Business.Authorization;
using FavLink.Entity;

namespace FavLink.Login {
    public partial class Signin : System.Web.UI.Page {
        protected string ReUrl {
            set { ViewState["ReUrl"] = value; }
            get {
                var vtemp = ViewState["ReUrl"];
                return vtemp == null ? "~/Page/FavIndex.aspx?cateid=1" : vtemp.ToString(); 
            }
        }
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                ReUrl = Request.QueryString["reurl"];
                this.Response.Buffer = true;
                DateTime dt = DateTime.Now;
                this.Response.ExpiresAbsolute = dt.AddHours(-1);
                this.Response.Expires = 0;
                this.Response.CacheControl = "no-cache";
            }
        }
        
        protected void btnLoginIn_Click(object sender, EventArgs e) {
            string ad = Request.Form["txUsername"];
            string pwd = Request.Form["txPassword"];
            string msg = "";

            Business.CacheFactory.ClearCache();
            UserBasicInfo domainUser = null;
            try {
                if(!UserState.IsLogin) {
                    // 检查用户是否存在
                    tbl_user user = Common.GetUserInfoByAd(ad);
                    if (user == null)
                    {
                        msg = "账号不存在，请先注册! \nLogin user not exist, please register first!";
                        alertMsg.Style[HtmlTextWriterStyle.Display] = "";
                        alertMsg.InnerText = msg;
                        return;
                    }

                    //验证域密码
                    if (user != null)
                    {
                        domainUser = new UserBasicInfo(user);
                    }
                    if (user == null || false == Business.AppSettings.DebugRun)
                    {
                        domainUser = new ActiveDirectoryHelper().GetDomainUser(ad, pwd, out msg);
                        if (domainUser == null)
                        {
                            alertMsg.Style[HtmlTextWriterStyle.Display] = "";
                            alertMsg.InnerText = msg;
                            return;
                        }
                        if (user != null)
                        {
                            domainUser = new UserBasicInfo(user);
                        }
                        // 更新用户邮箱
                        if (user != null && string.IsNullOrEmpty(domainUser.Email) == false &&
                            domainUser.Email != user.Email)
                        {
                            user.Email = domainUser.Email;
                            using (FavLinkEntities context = new FavLinkEntities())
                            {
                                context.ObjectStateManager.ChangeObjectState(user, System.Data.EntityState.Modified);
                                context.SaveChanges();
                            }
                        }
                    }
                    // 帐号被停用
                    if (!user.IsValid)
                    {
                        msg = "账号已被停用. Your account was disabled.";
                        alertMsg.Style[HtmlTextWriterStyle.Display] = "";
                        alertMsg.InnerText = msg;
                        return;
                    }
                } else {
                    domainUser = UserState.GetLoginUser();
                }
                UserState.Login(domainUser);
                Common.UpdateUserLoginTimeByAd(ad);
                if(!string.IsNullOrEmpty(ReUrl)) {
                    Response.Redirect(ReUrl);
                } else {
                    Response.Redirect(ResolveUrl("~/Default.aspx"));
                }
            } catch(Exception ex) {
                alertMsg.Style[HtmlTextWriterStyle.Display] = "";
                alertMsg.InnerText = ex.Message;
            }
        }
                

        [System.Web.Services.WebMethod]
        public static string DoRegister(string inputad)
        {
            string msg=string.Empty;            
            bool bOk = Common.InsertUserInfo(inputad, ref msg);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { ret=bOk?1:0, errMsg = msg });
        }

        protected bool HasOtherUsers()
        {
            return Common.HasOtherUsers();
        }
    }
}