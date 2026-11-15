using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VatTuNongNghiep.Models;

namespace VatTuNongNghiep.Helpers;

// Tạo dữ liệu bắt buộc khi khởi động ứng dụng
public static class KhoiTaoDuLieu
{
    public const string TenDangNhapAdmin = "admin";
    public const string MatKhauAdmin = "Admin@123";

    /// <summary>
    /// Nếu database chưa có tài khoản "admin" thì tạo mới với mật khẩu đã BĂM.
    /// (Không tạo bằng file SQL được vì SQL không biết thuật toán băm của PasswordHasher.)
    /// </summary>
    public static async Task TaoAdminMacDinhAsync(QlVatTuNongNghiepContext db, IPasswordHasher<Taikhoan> hasher, ILogger logger)
    {
        if (await db.Taikhoans.AnyAsync(t => t.Tendangnhap == TenDangNhapAdmin))
            return;

        var dsMa = await db.Taikhoans.Select(t => t.Matk).ToListAsync();
        var admin = new Taikhoan
        {
            Matk = MaTuSinh.TaoMaMoi(dsMa, "TK"),
            Tendangnhap = TenDangNhapAdmin,
            Email = "admin@vattunongnghiep.local",
            Trangthai = TrangThaiTaiKhoan.HoatDong,
            Vaitro = VaiTro.Admin
        };
        admin.Matkhau = hasher.HashPassword(admin, MatKhauAdmin);

        db.Taikhoans.Add(admin);
        await db.SaveChangesAsync();
        logger.LogInformation("Đã tạo tài khoản Admin mặc định: {Ten} (mã {Ma}).", admin.Tendangnhap, admin.Matk);
    }
}
