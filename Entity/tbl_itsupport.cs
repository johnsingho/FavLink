//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FavLink.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_itsupport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_itsupport()
        {
            this.tbl_itsupport_arrangment = new HashSet<tbl_itsupport_arrangment>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string phone_number { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_itsupport_arrangment> tbl_itsupport_arrangment { get; set; }
    }
}
