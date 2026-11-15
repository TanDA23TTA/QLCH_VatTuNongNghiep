using System.ComponentModel.DataAnnotations;

namespace VatTuNongNghiep.Areas.Admin.Models;

// Dữ liệu hiển thị/nhập cho Công dụng của phân bón
public class CongDungVM
{
    [Display(Name = "Mã công dụng")]
    public string? Macongdung { get; set; }      // do hệ thống tự sinh

    [Display(Name = "Tên công dụng")]
    [Required(ErrorMessage = "Vui lòng nhập tên công dụng.")]
    [StringLength(50, ErrorMessage = "Tên công dụng tối đa 50 ký tự.")]
    public string TenCongDung { get; set; } = "";

    [Display(Name = "Mô tả")]
    [StringLength(200, ErrorMessage = "Mô tả tối đa 200 ký tự.")]
    public string? MoTa { get; set; }

    [Display(Name = "Số sản phẩm")]
    public int SoSanPham { get; set; }           // số sản phẩm có công dụng này (bảng CO_CONG_DUNG)
}
