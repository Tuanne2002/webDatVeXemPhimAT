using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDatVePhim.Model
{
    [MetadataTypeAttribute(typeof(PhimMetadata))]
    public partial class Phim
    {
        internal sealed class PhimMetadata
        {
            [Required(ErrorMessage = "Không được để trống giá trị này")]
            [Display(Name = "Mã phim")]
            public int id_Phim { get; set; }
            [Required(ErrorMessage = "Không được để trống giá trị này")]
            [Display(Name = "Tên phim")]
            public string tenPhim { get; set; }
            [Required(ErrorMessage = "Không được để trống giá trị này")]

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            [Display(Name = "Ngày khởi chiếu")]
            public Nullable<System.DateTime> ngayKhoiChieu { get; set; }
            [Required(ErrorMessage = "Không được để trống giá trị này")]
            [Display(Name = "Thời lượng")]
            public Nullable<int> thoiLuong { get; set; }
            [Required(ErrorMessage = "Không được để trống giá trị này")]
            [Display(Name = "Thể loại")]
            public string theLoai { get; set; }
            [Required(ErrorMessage = "Không được để trống giá trị này")]
            [Display(Name = "Tóm tắt phim")]
            public string tomTatPhim { get; set; }

            [Display(Name = "Ảnh bìa")]
            public string anhPhim { get; set; }
            [Required(ErrorMessage = "Không được để trống giá trị này")]
            [Display(Name = "Đạo diễn")]
            public string daoDien { get; set; }
            [Required(ErrorMessage = "Không được để trống giá trị này")]
            [Display(Name = "Đánh giá")]
            public Nullable<double> danhGia { get; set; }
            [Required(ErrorMessage = "Không được để trống giá trị này")]
            [Display(Name = "Quốc gia")]
            public string quocGia { get; set; }
            [Required(ErrorMessage = "Không được để trống giá trị này")]
            [Display(Name = "Trailer")]
            public string trailer { get; set; }
            [Required(ErrorMessage = "Không được để trống giá trị này")]
            [Display(Name = "Diễn viên")]
            public string dienVien { get; set; }



        }
    }
}