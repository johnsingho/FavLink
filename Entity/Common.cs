using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FavLink.Business.Authorization;

namespace FavLink.Entity
{
    public class Common
    {
        public static string GetLinkPageTitle(string cateID)
        {
            switch (cateID)
            {
                case "1": return "Production Links";
                case "2": return "Management  Links";
                default: return "Searched Links";
            }
        }

        public static tbl_user GetUserInfoByAd(string sAdName)
        {
            tbl_user user = null;
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var people = from p in context.tbl_user
                             where (0 == String.Compare(p.ADAccount, sAdName, StringComparison.InvariantCultureIgnoreCase))
                             select p;
                if (people.Any())
                {
                    user = people.First();
                }
            }
            return user;
        }

        public static bool HasOtherUsers()
        {
            //tbl_user user = null;
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var peoples =context.tbl_user;
                if (peoples.Any())
                {
                    return true;
                }
            }
            return false;
        }

        public static DomainUserInfo GetAdInfo(string inputad, out string msg)
        {
            Business.Authorization.ActiveDirectoryHelper adh = new Business.Authorization.ActiveDirectoryHelper();
            var adUser = adh.GetDomainUserByAD(inputad, out msg);
            return adUser;
        }

        public static bool InsertUserInfo(string inputad, ref string errmsg)
        {
            bool bOk = false;
            var adUser = GetAdInfo(inputad, out errmsg);
            if (adUser == null)
            {
                errmsg = "AD login failed!";
                return false;
            }
            var adInfo = GetUserInfoByAd(inputad);
            if (adInfo != null)
            {
                errmsg = "You had been registered!";
                return false;
            }
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var entity = new tbl_user()
                {
                    ADAccount = adUser.ADAccount,
                    Email = adUser.Email,
                    FullName = adUser.FirstName + ' ' + adUser.LastName,
                    IsAdmin = false,
                    IsValid = true
                };
                try
                {
                    context.tbl_user.AddObject(entity);
                    context.SaveChanges();
                    bOk = true;
                    errmsg = string.Empty;
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
            }
            return bOk;
        }
        public static bool DeleteUser(int id, out string errmsg)
        {
            bool bOk = false;
            errmsg = string.Empty;
            using (var mContext = new FavLinkEntities())
            {
                var persons = from p in mContext.tbl_user
                              where p.id == id
                              select p;
                if (persons.Any())
                {
                    var obj = persons.First();
                    var data = obj.tbl_link_data.ToList();
                    foreach (var link in data)
                    {
                        mContext.tbl_link_data.DeleteObject(link);
                    }
                    mContext.tbl_user.DeleteObject(obj);
                    try
                    {
                        mContext.SaveChanges();
                        bOk = true;                        
                    }
                    catch (Exception ex)
                    {
                        errmsg = ex.Message;
                    }
                }
            }
            return bOk;
        }

        public static void UpdateUserLoginTimeByAd(string sAdName)
        {
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var people = from p in context.tbl_user
                             where (0 == String.Compare(p.ADAccount, sAdName, StringComparison.InvariantCultureIgnoreCase))
                             select p;
                if (people.Any())
                {
                    var user = people.First();
                    user.LastLogon = DateTime.Now;
                    context.ObjectStateManager.ChangeObjectState(user, System.Data.EntityState.Modified);
                    context.SaveChanges();
                }
            }
        }
        
        public static List<tbl_category> LoadCategories()
        {
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var recs = from p in context.tbl_category select p;
                return recs.ToList();
            }
        }

        public static List<tbl_link_data> LoadLinksByCateID(string cateID)
        {
            int ncateID = int.Parse(cateID);
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var recs = from p in context.tbl_link_data
                           where p.ref_categoryID.HasValue
                                && p.ref_categoryID.Value == ncateID
                           select p;
                if (recs.Any())
                {
                    return recs.ToList();
                }
                return new List<tbl_link_data>();
            }
        }

        public static List<tbl_link_data> LoadLinksByName(string searchName)
        {
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var recs = from p in context.tbl_link_data
                           where p.name.Contains(searchName)
                           select p;
                if (recs.Any())
                {
                    return recs.ToList();
                }
                return new List<tbl_link_data>();
            }
        }
        

        public static dynamic LoadContractInfo(int month)
        {
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var recs = from p in context.tbl_itsupport_arrangment
                    where p.month.HasValue 
                        && p.month.Value == month
                           select new
                            {
                                p.id,
                                name= p.tbl_itsupport.name,
                                phone_number = p.tbl_itsupport.phone_number,
                                p.project,
                                p.shift
                            };
                if (recs.Any())
                {
                    return recs.ToList();
                }
                return null;
            }
        }

        public static bool InsertItSupport(string sname, string sphone, out string errmsg)
        {
            bool bOk = false;
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var items = from x in context.tbl_itsupport
                            where 0 == x.name.CompareTo(sname)
                            select x;
                if (items.Any())
                {
                    errmsg = string.Format("{0} already exist", sname);
                    return false;
                }

                //insert
                var entity = new tbl_itsupport()
                {
                    name = sname,
                    phone_number=sphone
                };
                try
                {
                    context.tbl_itsupport.AddObject(entity);
                    context.SaveChanges();
                    bOk = true;
                    errmsg = string.Empty;
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
            }
            return bOk;        
        }

        public static bool InsertItSupportArrangement(int userID, int shiftID, string sProject, int nMonth, out string errmsg)
        {
            bool bOk = false;
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var items = from x in context.tbl_itsupport_arrangment
                            where x.ref_personID==userID
                                && x.shift==shiftID
                                && x.month==nMonth
                                && 0==x.project.CompareTo(sProject)
                            select x;   
                if (items.Any())
                {
                    errmsg = string.Format("The arrangement already exist!");
                    return false;
                }

                //insert
                var entity = new tbl_itsupport_arrangment()
                {
                    ref_personID=userID,
                    shift=shiftID,
                    project=sProject,
                    month=nMonth,
                };
                try
                {
                    context.tbl_itsupport_arrangment.AddObject(entity);
                    context.SaveChanges();
                    bOk = true;
                    errmsg = string.Empty;
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
            }
            return bOk;
        }

        public static bool UpdateItSupportArrangement(int oldID, int userID, int shiftID, string sProject, int nMonth, out string errmsg)
        {
            bool bOk = false;
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var items = from x in context.tbl_itsupport_arrangment
                            where x.id == oldID
                            select x;
                if (!items.Any())
                {
                    //not exist before, insert it
                    return InsertItSupportArrangement(userID, shiftID, sProject, nMonth, out errmsg);
                }

                //避免更新之后出现重复项
                var targets = from x in context.tbl_itsupport_arrangment
                              where x.ref_personID == userID
                                  && x.shift == shiftID
                                  && x.month == nMonth
                                  && 0==string.Compare(sProject, x.project, true)
                              select x;
                if (targets.Any())
                {
                    var item = targets.First();
                    errmsg = string.Format("This update was abandoned for it will cause duplicate items, the ID is: {0}", item.id);
                    return false;
                }

                //update
                var entity = items.First();
                entity.ref_personID = userID;
                entity.shift = shiftID;
                entity.project = sProject;
                entity.month = nMonth;

                try
                {
                    context.SaveChanges();
                    bOk = true;
                    errmsg = string.Empty;
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
            }
            return bOk;
        }

        // userID == -1 for all
        public static dynamic LoadUserLinks(int userID)
        {
            using (FavLinkEntities context = new FavLinkEntities())
            {
                if (userID < 0)
                {
                    //select all
                    var recs = from p in context.tbl_link_data
                               orderby p.ref_categoryID, p.id
                               select new
                               {
                                   p.id,
                                   p.name,
                                   p.url,
                                   categoryName = p.tbl_category.name,
                                   p.icon,
                                   bgColor = p.bg_color
                               };
                    if (recs.Any())
                    {
                        return recs.ToList();
                    }
                }
                else
                {
                    //select by user
                    var recs = from p in context.tbl_link_data
                               where p.ref_userID == userID
                               orderby p.ref_categoryID, p.id
                               select new
                               {
                                   p.id,
                                   p.name,
                                   p.url,
                                   categoryName = p.tbl_category.name,
                                   p.icon,
                                   bgColor = p.bg_color
                               };
                    if (recs.Any())
                    {
                        return recs.ToList();
                    }
                }
                return null;
            }
        }

        public static bool InsertLinkData(int userID, 
                                        string linkName, 
                                        string linkUrl, 
                                        string linkIcon, 
                                        string linkColor, 
                                        string linkCate,
                                        out string errmsg)
        {
            bool bOk = false;
            using (FavLinkEntities context = new FavLinkEntities())
            {
                int ncate = int.Parse(linkCate);
                var items = from x in context.tbl_link_data
                    where x.ref_userID == userID
                          && x.ref_categoryID==ncate
                          && (0==string.Compare(linkUrl, x.url, true))// 0 == x.url.CompareTo(linkUrl)
                    select x;
                if (items.Any())
                {
                    var item=items.First();
                    errmsg = string.Format("The URL was already exist, the name is: {0}", item.name);
                    return false;
                }

                //insert
                var entity = new tbl_link_data()
                {
                    ref_userID = userID,
                    name = linkName,
                    url=linkUrl,
                    icon = linkIcon,
                    bg_color = linkColor,
                    ref_categoryID = ncate
                };
                try
                {
                    context.tbl_link_data.AddObject(entity);
                    context.SaveChanges();
                    bOk = true;
                    errmsg = string.Empty;
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
            }
            return bOk;
        }

        public static bool DeleteLinkByID(int lid, out string errmsg)
        {
            bool bOk = false;
            errmsg = string.Empty;
            using (var mContext = new FavLinkEntities())
            {
                var its = from p in mContext.tbl_link_data
                          where p.id == lid
                          select p;
                if (its.Any())
                {
                    var obj = its.First();
                    try
                    {
                        mContext.tbl_link_data.DeleteObject(obj);
                        mContext.SaveChanges();
                        bOk = true;
                        errmsg = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        errmsg = ex.Message;
                    }
                }
                else
                {
                    errmsg = "the Link already deleted";
                }
            }
            return bOk;
        }

        public static bool UpdateLinkData(int userID, int lid, string linkName, string linkUrl, 
                            string linkIcon, string linkColor, string linkCate, out string errmsg)
        {
            bool bOk = false;
            using (FavLinkEntities context = new FavLinkEntities())
            {
                int ncate = int.Parse(linkCate);
                var items = from x in context.tbl_link_data
                            where x.id==lid
                            select x;
                if (!items.Any())
                {
                    errmsg = string.Format("The URL not exist!");
                    return false;
                }

                //避免更新之后出现重复项
                //var targets = from x in context.tbl_link_data
                //                where x.ref_userID == userID
                //                      && x.ref_categoryID == ncate
                //                      && (0 == string.Compare(linkUrl, x.url, true))
                //                select x;
                //if (targets.Any())
                //{
                //    var item = targets.First();
                //    errmsg = string.Format("This update was abandoned for it will cause duplicate items, the previous name is: {0}", item.name);
                //    return false;
                //}

                //update
                var entity = items.First();
                entity.name = linkName;
                entity.url = linkUrl;
                entity.icon = linkIcon;
                entity.bg_color = linkColor;
                entity.ref_categoryID = int.Parse(linkCate);

                try{
                    context.SaveChanges();
                    bOk = true;
                    errmsg = string.Empty;
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
            }
            return bOk;
        }

        public static bool DeleteItSupport(int id, out string errmsg)
        {
            var bOk = false;
            using (var mContext = new FavLinkEntities())
            {
                var its = from p in mContext.tbl_itsupport
                          where p.id == id
                          select p;
                if (its.Any())
                {
                    var obj = its.First();
                    var data = obj.tbl_itsupport_arrangment.ToList();
                    foreach (var ita in data)
                    {
                        mContext.tbl_itsupport_arrangment.DeleteObject(ita);
                    }
                    mContext.tbl_itsupport.DeleteObject(obj);
                }
                try
                {
                    mContext.SaveChanges();
                    bOk = true;
                    errmsg = string.Empty;
                }
                catch (Exception ex)
                {
                    errmsg = "IT support was arranged, can not delete";
                }
            }
            return bOk;
        }

        public static List<tbl_itsupport> GetItSupports()
        {
            using (var mContext = new FavLinkEntities())
            {
                var items = from x in mContext.tbl_itsupport 
                            select x;
                return items.ToList();
            }
        }

        public static bool EnableUser(int uid, bool bEnabled, out string errmsg)
        {
            bool bOk = false;
            using (var mContext = new FavLinkEntities())
            {
                var persons = from p in mContext.tbl_user
                              where p.id == uid
                              select p;
                foreach (var obj in persons)
                {
                    obj.IsValid = bEnabled;
                }
                try
                {
                    mContext.SaveChanges();
                    bOk = true;
                    errmsg = string.Empty;
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }                
            }
            return bOk;
        }

        public static dynamic GetItSupportArrangement(int shiftID)
        {
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var recs = from p in context.tbl_itsupport_arrangment
                           where p.id == shiftID
                           select new
                           {
                            suser=p.tbl_itsupport.name,
                            sproject=p.project,
                            shift=p.shift.Value,
                            month=p.month.Value
                           };
                if (recs.Any())
                {
                    return recs.First();
                }
                return null;
            }
        }

        public static List<tbl_hotline> LoadHotlines()
        {
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var recs = from p in context.tbl_hotline
                           orderby p.category,p.id
                           select p;
                if (recs.Any())
                {
                    return recs.ToList();
                }
                return null;
            }
        }

        public static bool DeleteHotline(int id, out string errmsg)
        {
            var bOk = false;
            using (var mContext = new FavLinkEntities())
            {
                var its = from p in mContext.tbl_hotline
                          where p.id == id
                          select p;
                if (its.Any())
                {
                    var obj = its.First();
                    mContext.tbl_hotline.DeleteObject(obj);
                }
                try
                {
                    mContext.SaveChanges();
                    bOk = true;
                    errmsg = string.Empty;
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
            }
            return bOk;
        }

        public class THotlineCategories
        {
            public int id { get; set; }
            public string name { get; set; }
        }
        public static List<THotlineCategories> LoadHotlineCategories()
        {
            var lst = new List<THotlineCategories>();
            lst.Add(new THotlineCategories { id = 1, name = "Hot Line" });
            lst.Add(new THotlineCategories { id = 2, name = "Escalation" });
            return lst;
        }

        public static bool InsertHotline(string sname, string sphone, int ncate, out string errmsg)
        {
            bool bOk = false;
            using (FavLinkEntities context = new FavLinkEntities())
            {
                var items = from x in context.tbl_hotline
                            where 0 == x.name.CompareTo(sname)
                            select x;
                if (items.Any())
                {
                    errmsg = string.Format("{0} already exist", sname);
                    return false;
                }

                //insert
                var entity = new tbl_hotline()
                {
                    name = sname,
                    phone_number = sphone,
                    category= ncate
                };
                try
                {
                    context.tbl_hotline.AddObject(entity);
                    context.SaveChanges();
                    bOk = true;
                    errmsg = string.Empty;
                }
                catch (Exception ex)
                {
                    errmsg = ex.Message;
                }
            }
            return bOk; 
        }
    }
}