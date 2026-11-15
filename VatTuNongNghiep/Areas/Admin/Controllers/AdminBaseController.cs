using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VatTuNongNghiep.Helpers;

namespace VatTuNongNghiep.Areas.Admin.Controllers;

// Lớp cha cho MỌI controller quản trị:
// - [Area("Admin")]: thuộc khu vực /Admin/...
// - [Authorize(Roles = "Admin")]: chỉ tài khoản có VAITRO = Admin mới vào được.
//   Chưa đăng nhập → chuyển tới /Account/Login; đăng nhập bằng khách hàng → /Account/AccessDenied.
[Area("Admin")]
[Authorize(Roles = VaiTro.Admin)]
public abstract class AdminBaseController : Controller
{
}
