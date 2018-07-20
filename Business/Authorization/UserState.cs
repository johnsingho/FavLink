using System;
using System.Linq;
using FavLink.Entity;

namespace FavLink.Business.Authorization {

    /// <summary>
    /// 2017-10-19 change for FavLink by Johnsing He
    /// </summary>
    public sealed class UserState {

        // 设置 COOKIE_USER_KEY 和 COOKIE_SESSION
        // 可设置为任意值
        private const string COOKIE_USER_KEY = "FLEXUSERKEY_2017FAVLINK";
        private const string COOKIE_SESSION = "FLEXSESSION_2017FAVLINK";
        private const string COOKIE_BROWSER_KEY = "FLEXBK_2017FAVLINK";

        /// <summary>
        /// 获取一个值，该值指示当前浏览器用户是否已经登录
        /// </summary>
        public static bool IsLogin {
            get {
                // 判断一个用户是否登录应该满足以下几个条件
                // 1: 客户端存在一个采用可逆加密的，存有用户ID的Cookie
                // 2. 客户端存在一个采用不可逆加密的，记录有当前浏览器特性的Cookie值
                if(GetLoginUser() == null)
                    return false;

                string enUnique = Utility.WebUtility.GetCookie(COOKIE_BROWSER_KEY);
                if(string.IsNullOrEmpty(enUnique))
                    return false;

                string currentUnique = new Utility.ClientUtility().GetUnique();
                try {
                    return Utility.CryptographyUtility.ToMD5(currentUnique) == enUnique;
                }
                catch(Exception) {
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取已经登录的用户，
        /// 该值可能为null
        /// </summary>
        public static UserBasicInfo GetLoginUser() {
            string cookieString = Utility.WebUtility.GetCookie(COOKIE_USER_KEY);
            if(string.IsNullOrEmpty(cookieString))
                return null;

            var info = new UserBasicInfo();
            if(info.Deserialize(cookieString)) {
                string session = Utility.WebUtility.GetCookie(COOKIE_SESSION);
                if(string.IsNullOrEmpty(session)) {
                    try {
                        // 更新最后登录时间
                        // new UserTable().UpdateLoginState(info.UserID);
                    }
                    catch { }
                    Utility.WebUtility.WriteCookie(COOKIE_SESSION, Guid.NewGuid().ToString("N"));
                }
                return info;
            }
            return null;
        }

        /// <summary>
        /// 获取当前用户的完整信息
        /// </summary>
        //public static Entity.Sys_UserEntity GetLoginUserUserInfo() {
        //    var currentUser = GetLoginUser();
        //    if(currentUser == null) { return null; }
        //    return Business.SysUser.SelectByAD(currentUser.UserName);
        //}
        public static tbl_user GetLoginUserUserInfo()
        {   
            var currEmail = UserEmail;
            if (string.IsNullOrEmpty(currEmail)) { return null; }

            using (FavLinkEntities context = new FavLinkEntities())
            {
                var people = from p in context.tbl_user
                             where (0==String.Compare(p.Email, currEmail, StringComparison.InvariantCultureIgnoreCase))
                             select p;
                if (people.Any())
                {
                    return people.First();
                }
            }
            return null;
        }

        public static void Login(UserBasicInfo user, bool autoLogin) {
            string cookieValue = user.Serialize();
            if(autoLogin) {
                Utility.WebUtility.WriteCookie(COOKIE_USER_KEY, cookieValue, DateTime.MaxValue);
            }
            else {
                Utility.WebUtility.WriteCookie(COOKIE_USER_KEY, cookieValue);
            }
            WriteBroswerUnique(autoLogin);
        }

        public static void Login(UserBasicInfo user) {
            Login(user, false);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public static void Logout() {
            Utility.WebUtility.RemoveCookie(COOKIE_USER_KEY);
            Utility.WebUtility.RemoveCookie(COOKIE_BROWSER_KEY);
            Utility.WebUtility.RemoveCookie(COOKIE_SESSION);
        }

        // 将当前浏览器唯一标识写入Cookie
        private static void WriteBroswerUnique(bool autoLogin) {
            string unique = new Utility.ClientUtility().GetUnique();
            if(!string.IsNullOrEmpty(unique)) {
                unique = Utility.CryptographyUtility.ToMD5(unique);
                if(autoLogin) {
                    Utility.WebUtility.WriteCookie(COOKIE_BROWSER_KEY, unique, DateTime.MaxValue);
                }
                else {
                    Utility.WebUtility.WriteCookie(COOKIE_BROWSER_KEY, unique);
                }
            }
        }

        public static int UserID {
            get {
                var user = GetLoginUser();
                if(user == null) {
                    return -1;
                }
                return user.UserID;
            }
        }

        public static string UserEmail
        {
            get
            {
                var user = GetLoginUser();
                if (user == null)
                {
                    return string.Empty;
                }
                return user.Email;
            }
        }
    }
}