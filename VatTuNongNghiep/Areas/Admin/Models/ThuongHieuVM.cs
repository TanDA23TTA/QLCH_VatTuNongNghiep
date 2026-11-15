using System.ComponentModel.DataAnnotations;

namespace VatTuNongNghiep.Areas.Admin.Models;

// Dữ liệu hiển thị/nhập cho Thương hiệu
public class ThuongHieuVM
{
    [Display(Name = "Mã thương hiệu")]
    public string? Mathuonghieu { get; set; }    // do hệ thống tự sinh

    [Display(Name = "Tên thương hiệu")]
    [Required(ErrorMessage = "Vui lòng nhập tên thương hiệu.")]
    [StringLength(50, ErrorMessage = "Tên thương hiệu tối đa 50 ký tự.")]
    public string TenThuongHieu { get; set; } = "";

    [Display(Name = "Quốc gia")]
    [StringLength(50, ErrorMessage = "Quốc gia tối đa 50 ký tự.")]
    public string? QuocGia { get; set; }

    [Display(Name = "Địa chỉ")]
    [StringLength(150, ErrorMessage = "Địa chỉ tối đa 150 ký tự.")]
    public string? DiaChi { get; set; }

    [Display(Name = "Số sản phẩm")]
    public int SoSanPham { get; set; }           // chỉ để hiển thị
}
