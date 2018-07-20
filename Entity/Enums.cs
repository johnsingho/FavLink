using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FavLink.Entity
{
    public enum EnumShift
    {
        [Description("Morning Shift")]
        MorningShift =0,
        [Description("Middle Shift")]
        MiddleShift =1,
        [Description("Night Shift")]
        NightShift =2 
    }    
}