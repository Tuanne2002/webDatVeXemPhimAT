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
    
    public partial class LichChieuPhim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LichChieuPhim()
        {
            this.Ves = new HashSet<Ve>();
        }
    
        public int id_LichChieuPhim { get; set; }
        public Nullable<int> id_Phim { get; set; }
        public Nullable<int> id_PhongChieu { get; set; }
        public Nullable<System.TimeSpan> thoiGianBatDau { get; set; }
        public Nullable<System.TimeSpan> thoiGianKetThuc { get; set; }
        public Nullable<System.DateTime> ngayChieu { get; set; }
        public Nullable<int> id_Ghe { get; set; }
    
        public virtual Ghe Ghe { get; set; }
        public virtual Phim Phim { get; set; }
        public virtual PhongChieu PhongChieu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ve> Ves { get; set; }
    }
}
