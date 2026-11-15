namespace VatTuNongNghiep.Helpers;

// Giá trị cột TAIKHOAN.VAITRO – dùng hằng số để tránh gõ sai chính tả
public static class VaiTro
{
    public const string Admin = "Admin";
    public const string KhachHang = "KhachHang";
}

// Giá trị cột THANHPHAN.LOAITHANHPHAN (dùng cho ô chọn khi thêm/sửa thành phần)
public static class LoaiThanhPhan
{
    public static readonly string[] DanhSach = { "Đa lượng", "Trung lượng", "Vi lượng", "Hữu cơ", "Vi sinh" };
}

// Gợi ý đơn vị hàm lượng (người dùng vẫn được gõ đơn vị khác)
public static class DonViHamLuong
{
    public static readonly string[] GoiY = { "%", "ppm", "mg/kg", "CFU/g" };
}

// Giá trị cột TAIKHOAN.TRANGTHAI
public static class TrangThaiTaiKhoan
{
    public const string HoatDong = "Hoạt động";
    public const string BiKhoa = "Bị khóa";
}
