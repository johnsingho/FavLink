using System.Web;

namespace FavLink.Login
{
    /// <summary>
    /// Summary description for LoginOut
    /// </summary>
    public class Signout : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Business.Authorization.UserState.Logout();
            //context.Response.Redirect(new System.Web.UI.Control().ResolveUrl("~/Login/Signin.aspx"));
            context.Response.Redirect(new System.Web.UI.Control().ResolveUrl("~/Page/FavIndex.aspx?cateid=1"));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}