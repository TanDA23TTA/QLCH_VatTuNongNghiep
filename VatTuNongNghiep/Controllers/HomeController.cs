using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VatTuNongNghiep.Models;

namespace VatTuNongNghiep.Controllers;

public class HomeController : Controller
{
    private readonly QlVatTuNongNghiepContext _db;

    // DbContext được ASP.NET Core tự truyền vào (Dependency Injection)
    public HomeController(QlVatTuNongNghiepContext db)
    {
        _db = db;
    }

    // Trang chủ: hiển thị các nhóm sản phẩm đọc từ database
    public async Task<IActionResult> Index()
    {
        var dsNhom = await _db.Nhomsanphams.OrderBy(n => n.Manhom).ToListAsync();
        return View(dsNhom);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
