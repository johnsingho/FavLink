using System;
using System.Configuration;

namespace FavLink.Business {

    public static class AppSettings {

        public const int MaxExcelRowExportable = 65000;

        /// <summary>
        /// 获取网站域名，该域名通常用于邮件等需要使用到网站绝对地址的地方
        /// </summary>
        public static string SiteDomain {
            get { return ConfigurationManager.AppSettings["SiteDomain"]; }
        }

        /// <summary>
        /// 获取一个值，该值指示当前网站是否在调试模式下运行
        /// </summary>
        public static bool DebugRun {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["DebugRun"]); }
        }

        /// <summary>
        /// 获取SMTP连接串
        /// </summary>
        public static string SmtpConnection {
            get { return ConfigurationManager.ConnectionStrings["SMTPConnection"].ConnectionString; }
        }

        /// <summary>
        /// 文件上传保存根路径
        /// </summary>
        public static string UploadPath {
            get { return "~/Upload/"; }
        }

    }
}
