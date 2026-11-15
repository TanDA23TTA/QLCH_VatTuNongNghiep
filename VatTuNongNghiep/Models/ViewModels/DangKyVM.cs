using System.ComponentModel.DataAnnotations;

namespace VatTuNongNghiep.Models.ViewModels;

// Dữ liệu form Đăng ký khách hàng (ghi vào 2 bảng TAIKHOAN + KHACHHANG)
public class DangKyVM
{
    [Display(Name = "Tên đăng nhập")]
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Tên đăng nhập dài từ 3 đến 20 ký tự.")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập chỉ gồm chữ không dấu, số và dấu gạch dưới.")]
    public string TenDangNhap { get; set; } = "";

    [Display(Name = "Mật khẩu")]
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
    [DataType(DataType.Password)]
    public string MatKhau { get; set; } = "";

    [Display(Name = "Nhập lại mật khẩu")]
    [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
    [Compare(nameof(MatKhau), ErrorMessage = "Mật khẩu nhập lại không khớp.")]
    [DataType(DataType.Password)]
    public string XacNhanMatKhau { get; set; } = "";

    [Display(Name = "Họ tên")]
    [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
    [StringLength(50, ErrorMessage = "Họ tên tối đa 50 ký tự.")]
    public string HoTen { get; set; } = "";

    [Display(Name = "Số điện thoại")]
    [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
    [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại bắt đầu bằng 0, gồm 10–11 chữ số.")]
    public string SoDienThoai { get; set; } = "";

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    [StringLength(100, ErrorMessage = "Email tối đa 100 ký tự.")]
    public string? Email { get; set; }

    [Display(Name = "Địa chỉ")]
    [StringLength(150, ErrorMessage = "Địa chỉ tối đa 150 ký tự.")]
    public string? DiaChi { get; set; }
}
