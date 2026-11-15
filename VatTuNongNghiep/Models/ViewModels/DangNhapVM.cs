using System.ComponentModel.DataAnnotations;

namespace VatTuNongNghiep.Models.ViewModels;

// Dữ liệu form Đăng nhập
public class DangNhapVM
{
    [Display(Name = "Tên đăng nhập")]
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
    public string TenDangNhap { get; set; } = "";

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    public string MatKhau { get; set; } = "";

    [Display(Name = "Ghi nhớ đăng nhập")]
    public bool GhiNho { get; set; }
}
