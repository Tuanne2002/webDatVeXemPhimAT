//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebDatVePhim.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ghe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ghe()
        {
            this.Ves = new HashSet<Ve>();
        }
    
        public int id_Ghe { get; set; }
        public Nullable<int> id_PhongChieu { get; set; }
        public Nullable<int> soHangGhe { get; set; }
        public Nullable<int> soGheTrongHang { get; set; }
        public string tinhTrang { get; set; }
    
        public virtual PhongChieu PhongChieu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ve> Ves { get; set; }
    }
}