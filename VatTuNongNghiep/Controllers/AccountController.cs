using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VatTuNongNghiep.Helpers;
using VatTuNongNghiep.Models;
using VatTuNongNghiep.Models.ViewModels;

namespace VatTuNongNghiep.Controllers;

// Đăng nhập / Đăng ký / Đăng xuất – dùng Cookie Authentication + bảng TAIKHOAN
public class AccountController : Controller
{
    private readonly QlVatTuNongNghiepContext _db;
    private readonly IPasswordHasher<Taikhoan> _hasher;

    public AccountController(QlVatTuNongNghiepContext db, IPasswordHasher<Taikhoan> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    // ===================== ĐĂNG NHẬP =====================
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(DangNhapVM vm, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
            return View(vm);

        var tk = await _db.Taikhoans.FirstOrDefaultAsync(t => t.Tendangnhap == vm.TenDangNhap);

        // Không báo riêng "sai tên" hay "sai mật khẩu" để tránh lộ thông tin tài khoản
        if (tk == null || !KiemTraMatKhau(tk, vm.MatKhau))
        {
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(vm);
        }

        if (tk.Trangthai == TrangThaiTaiKhoan.BiKhoa)
        {
            ModelState.AddModelError(string.Empty, "Tài khoản đã bị khóa. Vui lòng liên hệ cửa hàng.");
            return View(vm);
        }

        await GhiCookieDangNhapAsync(tk, vm.GhiNho);

        // Chỉ chuyển về returnUrl nội bộ (chống chuyển hướng sang trang lạ)
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return tk.Vaitro == VaiTro.Admin
            ? RedirectToAction("Index", "Home", new { area = "Admin" })
            : RedirectToAction("Index", "Home");
    }

    // ===================== ĐĂNG KÝ (chỉ cho khách hàng) =====================
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(DangKyVM vm)
    {
        // Kiểm tra trùng tên đăng nhập (cột TENDANGNHAP là UNIQUE)
        if (await _db.Taikhoans.AnyAsync(t => t.Tendangnhap == vm.TenDangNhap))
            ModelState.AddModelError(nameof(vm.TenDangNhap), "Tên đăng nhập đã có người sử dụng.");

        if (!ModelState.IsValid)
            return View(vm);

        // Tự sinh mã không trùng: TKxxx cho tài khoản, KHxxx cho khách hàng
        var maTK = MaTuSinh.TaoMaMoi(await _db.Taikhoans.Select(t => t.Matk).ToListAsync(), "TK");
        var maKH = MaTuSinh.TaoMaMoi(await _db.Khachhangs.Select(k => k.Makh).ToListAsync(), "KH");

        var tk = new Taikhoan
        {
            Matk = maTK,
            Tendangnhap = vm.TenDangNhap.Trim(),
            Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email.Trim(),
            Trangthai = TrangThaiTaiKhoan.HoatDong,
            Vaitro = VaiTro.KhachHang
        };
        tk.Matkhau = _hasher.HashPassword(tk, vm.MatKhau);   // lưu mật khẩu đã băm

        var kh = new Khachhang
        {
            Makh = maKH,
            Matk = maTK,
            Hoten = vm.HoTen.Trim(),
            Sodienthoai = vm.SoDienThoai.Trim(),
            Diachi = string.IsNullOrWhiteSpace(vm.DiaChi) ? null : vm.DiaChi.Trim(),
            Ngaydk = DateTime.Now
        };

        // Một lần SaveChanges = một giao dịch: thêm cả 2 bảng hoặc không thêm gì
        _db.Taikhoans.Add(tk);
        _db.Khachhangs.Add(kh);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Trường hợp hiếm: 2 người đăng ký cùng lúc bị trùng mã/tên
            ModelState.AddModelError(string.Empty, "Không thể tạo tài khoản lúc này, vui lòng thử lại.");
            return View(vm);
        }

        // Đăng ký xong thì đăng nhập luôn
        await GhiCookieDangNhapAsync(tk, false, kh.Hoten);
        TempData["ThongBao"] = $"Đăng ký thành công. Chào mừng {kh.Hoten}!";
        return RedirectToAction("Index", "Home");
    }

    // ===================== ĐĂNG XUẤT =====================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home", new { area = "" });
    }

    // Trang báo khi khách hàng cố vào khu vực quản trị
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    // ===================== HÀM HỖ TRỢ =====================

    // So khớp mật khẩu nhập với chuỗi băm trong database
    private bool KiemTraMatKhau(Taikhoan tk, string matKhau)
    {
        try
        {
            return _hasher.VerifyHashedPassword(tk, tk.Matkhau, matKhau) != PasswordVerificationResult.Failed;
        }
        catch (FormatException)
        {
            // Cột MATKHAU không phải chuỗi băm hợp lệ (vd: ai đó nhập mật khẩu thô bằng SQL)
            return false;
        }
    }

    // Tạo "thẻ thông tin" (Claims) của người dùng và ghi vào cookie
    private async Task GhiCookieDangNhapAsync(Taikhoan tk, bool ghiNho, string? hoTen = null)
    {
        // Lấy họ tên khách hàng để hiển thị lời chào (Admin không có trong bảng KHACHHANG)
        hoTen ??= await _db.Khachhangs.Where(k => k.Matk == tk.Matk).Select(k => k.Hoten).FirstOrDefaultAsync();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, tk.Matk),
            new(ClaimTypes.Name, tk.Tendangnhap),
            new(ClaimTypes.Role, tk.Vaitro),          // dùng cho [Authorize(Roles = "Admin")]
            new("HoTen", hoTen ?? tk.Tendangnhap)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var thuocTinh = new AuthenticationProperties
        {
            IsPersistent = ghiNho,                                  // true: giữ đăng nhập sau khi tắt trình duyệt
            ExpiresUtc = ghiNho ? DateTimeOffset.UtcNow.AddDays(7) : null
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity), thuocTinh);
    }
}
