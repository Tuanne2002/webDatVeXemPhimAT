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
    
    public partial class BinhLuan
    {
        public int id_BinhLuan { get; set; }
        public Nullable<int> id_NguoiDung { get; set; }
        public Nullable<int> id_Phim { get; set; }
        public string noiDung { get; set; }
        public Nullable<int> soSao { get; set; }
        public Nullable<int> tongBinhLuan { get; set; }
    
        public virtual NguoiDung NguoiDung { get; set; }
        public virtual Phim Phim { get; set; }
    }
}
