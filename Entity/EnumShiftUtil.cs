using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility;

namespace FavLink.Entity
{
    public class EnumShiftUtil
    {
        public static string GetShiftStr(int nshift)
        {
            try
            {
                var e = (EnumShift)Enum.Parse(typeof(EnumShift), nshift.ToString());
                return e.ToName();
            }
            catch (Exception)
            {
                return EnumShift.MorningShift.ToName();
            }
        }
    }
}