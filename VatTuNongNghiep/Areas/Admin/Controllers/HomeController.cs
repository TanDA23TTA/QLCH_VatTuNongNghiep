using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VatTuNongNghiep.Models;

namespace VatTuNongNghiep.Areas.Admin.Controllers;

// Kế thừa AdminBaseController → thuộc Area Admin và chỉ Admin truy cập được. URL: /Admin
public class HomeController : AdminBaseController
{
    private readonly QlVatTuNongNghiepContext _db;

    public HomeController(QlVatTuNongNghiepContext db)
    {
        _db = db;
    }

    // Trang tổng quan: đếm số bản ghi của 4 danh mục
    public async Task<IActionResult> Index()
    {
        ViewBag.SoNhom = await _db.Nhomsanphams.CountAsync();
        ViewBag.SoThuongHieu = await _db.Thuonghieus.CountAsync();
        ViewBag.SoThanhPhan = await _db.Thanhphans.CountAsync();
        ViewBag.SoCongDung = await _db.Congdungs.CountAsync();
        return View();
    }
}
