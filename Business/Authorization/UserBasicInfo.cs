using System;
using FavLink.Business.Security;
using FavLink.Entity;

namespace FavLink.Business.Authorization {

    /// <summary>
    /// 用户信息实体，只包含最基本的用户信息。
    /// 这个实体通常被缓存在客户端。
    /// </summary>
    public class UserBasicInfo : TimeBasedCredential {

        private int _userID;
        private string _userName; //AD
        private string _nickName;
        //private string _pwd;
        private string _email;
        private bool _isAdmin;
        
        //private const char SERIALIZE_SP = '\n';

        public UserBasicInfo() { }

        public UserBasicInfo(tbl_user user)
        {
            _userID = user.id;
            _userName = user.ADAccount;
            //_niceName = user.FirstName + " " + user.LastName;
            _email = user.Email;
            _isAdmin = user.IsAdmin;
            //_Ref_DepartmentID = user.Ref_DepartmentID;
            //_Ref_SegmentID = user.Ref_SegmentID;
        }
        
        /// <summary>
        /// 获取或设置用户的AD用户名
        /// </summary>
        public string UserName {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// 获取或设置用户的ID
        /// </summary>
        public int UserID {
            get { return _userID; }
            set { _userID = value; }
        }

        /// <summary>
        /// 获取或设置用户密码
        /// </summary>
        //public string Password {
        //    get { return _pwd; }
        //    set { _pwd = value; }
        //}

        /// <summary>
        /// 获取或设置用户Email
        /// </summary>
        public string Email {
            get { return _email; }
            set { _email = value; }

        }

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; }
        }

        public string NickName
        {
            get { return _nickName; }
            set { _nickName = value; }
        }

        //public string ScreenName {
        //    get {
        //        return string.Format(
        //            "{0} <{1}>",
        //            NickName,
        //            UserName
        //            );
        //    }
        //}

        protected override string[] GetSerializeFields() {
            return new string[] {
                UserID.ToString(),
                UserName,
                Email,
                IsAdmin.ToString()
            };
        }
        
        public override bool Deserialize(string credential) {

            try {
                string[] values = base.GetSerializeValues(credential);
                int i = VALUE_FIELD_START_INDEX;
                this.UserID = int.Parse(values[i++]);
                this.UserName = values[i++];
                //this.NickName = values[VALUE_FIELD_START_INDEX + 2];
                //this.Password = values[VALUE_FIELD_START_INDEX + 3];
                this.Email = values[i++];
                this.IsAdmin = Boolean.Parse(values[i++]);
                
                return true;
            } catch {
                return false;
            }
        }
    }
}
