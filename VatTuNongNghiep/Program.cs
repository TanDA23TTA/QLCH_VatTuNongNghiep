using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders;
using VatTuNongNghiep.Helpers;
using VatTuNongNghiep.Models;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký MVC (Controller + View)
builder.Services.AddControllersWithViews();

// Giữ nguyên chữ tiếng Việt có dấu trong HTML (không mã hóa thành &#x...;)
builder.Services.Configure<WebEncoderOptions>(options =>
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

// Đăng ký DbContext, chuỗi kết nối đọc từ appsettings.json (khóa DefaultConnection)
builder.Services.AddDbContext<QlVatTuNongNghiepContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Băm mật khẩu bằng PasswordHasher (có sẵn trong ASP.NET Core, PBKDF2 + salt ngẫu nhiên)
builder.Services.AddSingleton<IPasswordHasher<Taikhoan>, PasswordHasher<Taikhoan>>();

// Đăng nhập bằng Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";              // chưa đăng nhập → chuyển tới đây
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied"; // không đủ quyền → chuyển tới đây
        options.Cookie.Name = "VatTuNongNghiep.DangNhap";
        options.Cookie.HttpOnly = true;                     // JavaScript không đọc được cookie
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;                   // còn thao tác thì tự gia hạn
    });

var app = builder.Build();

// Khi khởi động: kiểm tra kết nối database và tạo tài khoản Admin mặc định (nếu chưa có)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QlVatTuNongNghiepContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        if (db.Database.CanConnect())
        {
            logger.LogInformation("Kết nối database QL_VatTuNongNghiep thành công.");
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Taikhoan>>();
            await KhoiTaoDuLieu.TaoAdminMacDinhAsync(db, hasher, logger);
        }
        else
        {
            logger.LogError("Không kết nối được database QL_VatTuNongNghiep. Kiểm tra chuỗi kết nối.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Lỗi khi kết nối database.");
    }
}

// Cấu hình pipeline xử lý request
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();   // đọc cookie → biết ai đang đăng nhập (phải đứng trước UseAuthorization)
app.UseAuthorization();    // kiểm tra quyền theo [Authorize]

app.MapStaticAssets();

// Route cho Area (phải khai báo TRƯỚC route mặc định), ví dụ: /Admin/NhomSanPham/Index
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Route mặc định phía người dùng
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
