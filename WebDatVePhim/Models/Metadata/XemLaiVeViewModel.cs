using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDatVePhim.Models.Metadata
{
    public partial class XemLaiVeViewModel
    {
        public int VeId { get; set; }
        public string ViTriGhe { get; set; }
        public double GiaVe { get; set; }
        public List<BapNuocDetail> BapNuocDetails { get; set; }
        public double TongSoTien { get; set; }
    }
    public class BapNuocDetail
    {
        public string TenBapNuoc { get; set; }
        public double GiaTien { get; set; }
        public int SoLuong { get; set; }
    }
}