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
    
    public partial class Phim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Phim()
        {
            this.BinhLuans = new HashSet<BinhLuan>();
            this.LichChieuPhims = new HashSet<LichChieuPhim>();
            this.NguoiDungs = new HashSet<NguoiDung>();
        }
    
        public int id_Phim { get; set; }
        public string tenPhim { get; set; }
        public Nullable<System.DateTime> ngayKhoiChieu { get; set; }
        public Nullable<int> thoiLuong { get; set; }
        public string theLoai { get; set; }
        public string tomTatPhim { get; set; }
        public string anhPhim { get; set; }
        public string daoDien { get; set; }
        public Nullable<double> danhGia { get; set; }
        public string quocGia { get; set; }
        public string trailer { get; set; }
        public string dienVien { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichChieuPhim> LichChieuPhims { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}
