using System.ComponentModel.DataAnnotations;

namespace VatTuNongNghiep.Areas.Admin.Models;

// Dữ liệu hiển thị/nhập cho Thành phần (dinh dưỡng trong phân bón)
public class ThanhPhanVM
{
    [Display(Name = "Mã thành phần")]
    public string? Mathanhphan { get; set; }     // do hệ thống tự sinh

    [Display(Name = "Tên thành phần")]
    [Required(ErrorMessage = "Vui lòng nhập tên thành phần.")]
    [StringLength(50, ErrorMessage = "Tên thành phần tối đa 50 ký tự.")]
    public string TenThanhPhan { get; set; } = "";

    [Display(Name = "Đơn vị hàm lượng")]
    [StringLength(10, ErrorMessage = "Đơn vị tối đa 10 ký tự.")]
    public string? DonViHamLuong { get; set; }

    [Display(Name = "Loại thành phần")]
    [Required(ErrorMessage = "Vui lòng chọn loại thành phần.")]
    [StringLength(30)]
    public string LoaiThanhPhan { get; set; } = "";

    [Display(Name = "Số sản phẩm")]
    public int SoSanPham { get; set; }           // số sản phẩm có chứa thành phần này (bảng CO_THANH_PHAN)
}
