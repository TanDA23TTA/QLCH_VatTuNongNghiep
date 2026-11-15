using System.ComponentModel.DataAnnotations;

namespace VatTuNongNghiep.Areas.Admin.Models;

// Dữ liệu hiển thị/nhập cho Nhóm sản phẩm (tách khỏi class EF để thêm kiểm tra hợp lệ)
public class NhomSanPhamVM
{
    [Display(Name = "Mã nhóm")]
    public string? Manhom { get; set; }          // do hệ thống tự sinh, người dùng không nhập

    [Display(Name = "Tên nhóm")]
    [Required(ErrorMessage = "Vui lòng nhập tên nhóm.")]
    [StringLength(50, ErrorMessage = "Tên nhóm tối đa 50 ký tự.")]
    public string TenNhom { get; set; } = "";

    [Display(Name = "Mô tả")]
    [StringLength(200, ErrorMessage = "Mô tả tối đa 200 ký tự.")]
    public string? MoTa { get; set; }

    [Display(Name = "Số sản phẩm")]
    public int SoSanPham { get; set; }           // chỉ để hiển thị
}
