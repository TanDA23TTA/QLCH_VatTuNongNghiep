using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VatTuNongNghiep.Areas.Admin.Models;
using VatTuNongNghiep.Helpers;
using VatTuNongNghiep.Models;

namespace VatTuNongNghiep.Areas.Admin.Controllers;

// Quản lý danh mục CÔNG DỤNG (ra rễ, nuôi trái, cải tạo đất...): danh sách + tìm kiếm, thêm, sửa, xóa
// (làm theo mẫu NhomSanPhamController). URL: /Admin/CongDung
public class CongDungController : AdminBaseController
{
    private const string TienToMa = "CD";      // mã dạng CD001, CD002...
    private const int SoDongMoiTrang = 10;

    private readonly QlVatTuNongNghiepContext _db;

    public CongDungController(QlVatTuNongNghiepContext db)
    {
        _db = db;
    }

    // ===================== DANH SÁCH + TÌM KIẾM =====================
    // GET /Admin/CongDung?tuKhoa=ra hoa&trang=1
    public async Task<IActionResult> Index(string? tuKhoa, int trang = 1)
    {
        var truyVan = _db.Congdungs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            tuKhoa = tuKhoa.Trim();
            // Tìm không dấu theo tên, và theo mã
            truyVan = truyVan.Where(c =>
                EF.Functions.Collate(c.Tencongdung, "Latin1_General_100_CI_AI").Contains(tuKhoa)
                || c.Macongdung.Contains(tuKhoa));
        }

        var phanTrang = PhanTrangVM.Tao(await truyVan.CountAsync(), trang, SoDongMoiTrang, tuKhoa);

        var ds = await truyVan
            .OrderBy(c => c.Macongdung)
            .Skip(phanTrang.ViTriBatDau)
            .Take(SoDongMoiTrang)
            .Select(c => new CongDungVM
            {
                Macongdung = c.Macongdung,
                TenCongDung = c.Tencongdung,
                MoTa = c.Mota,
                SoSanPham = c.Masps.Count()        // đếm qua bảng trung gian CO_CONG_DUNG
            })
            .ToListAsync();

        ViewBag.PhanTrang = phanTrang;
        return View(ds);
    }

    // ===================== THÊM =====================
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View(new CongDungVM { Macongdung = await SinhMaMoiAsync() });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CongDungVM vm)
    {
        vm.TenCongDung = vm.TenCongDung?.Trim() ?? "";
        if (await TrungTenAsync(vm.TenCongDung, null))
            ModelState.AddModelError(nameof(vm.TenCongDung), "Tên công dụng này đã tồn tại.");

        if (!ModelState.IsValid)
        {
            vm.Macongdung = await SinhMaMoiAsync();
            return View(vm);
        }

        // Sinh mã lại ngay lúc lưu để chắc chắn không trùng
        var cd = new Congdung
        {
            Macongdung = await SinhMaMoiAsync(),
            Tencongdung = vm.TenCongDung,
            Mota = string.IsNullOrWhiteSpace(vm.MoTa) ? null : vm.MoTa.Trim()
        };
        _db.Congdungs.Add(cd);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty, "Không lưu được (có thể mã vừa bị người khác dùng). Vui lòng bấm Lưu lại.");
            vm.Macongdung = await SinhMaMoiAsync();
            return View(vm);
        }

        TempData["ThongBao"] = $"Đã thêm công dụng {cd.Macongdung} – {cd.Tencongdung}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== SỬA =====================
    [HttpGet]
    public async Task<IActionResult> Edit(string? id)
    {
        var cd = await TimCongDungAsync(id);
        if (cd == null)
            return KhongTimThay(id);

        return View(new CongDungVM { Macongdung = cd.Macongdung, TenCongDung = cd.Tencongdung, MoTa = cd.Mota });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, CongDungVM vm)
    {
        var cd = await TimCongDungAsync(id);
        if (cd == null)
            return KhongTimThay(id);

        vm.Macongdung = id;                           // mã không cho sửa
        vm.TenCongDung = vm.TenCongDung?.Trim() ?? "";
        if (await TrungTenAsync(vm.TenCongDung, id))
            ModelState.AddModelError(nameof(vm.TenCongDung), "Tên công dụng này đã tồn tại.");

        if (!ModelState.IsValid)
            return View(vm);

        cd.Tencongdung = vm.TenCongDung;
        cd.Mota = string.IsNullOrWhiteSpace(vm.MoTa) ? null : vm.MoTa.Trim();
        await _db.SaveChangesAsync();

        TempData["ThongBao"] = $"Đã cập nhật công dụng {cd.Macongdung} – {cd.Tencongdung}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== XÓA =====================
    // GET: trang xác nhận xóa
    [HttpGet]
    public async Task<IActionResult> Delete(string? id)
    {
        var vm = await _db.Congdungs
            .Where(c => c.Macongdung == id)
            .Select(c => new CongDungVM
            {
                Macongdung = c.Macongdung,
                TenCongDung = c.Tencongdung,
                MoTa = c.Mota,
                SoSanPham = c.Masps.Count()
            })
            .FirstOrDefaultAsync();

        if (vm == null)
            return KhongTimThay(id);

        return View(vm);
    }

    // POST: thực hiện xóa
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? id)
    {
        var cd = await TimCongDungAsync(id);
        if (cd == null)
            return KhongTimThay(id);

        // Công dụng đang gắn với sản phẩm (bảng CO_CONG_DUNG) thì không cho xóa
        int soSanPham = await _db.Congdungs.Where(c => c.Macongdung == id).Select(c => c.Masps.Count()).FirstAsync();
        if (soSanPham > 0)
        {
            TempData["Loi"] = $"Không thể xóa công dụng “{cd.Tencongdung}” vì đang có {soSanPham} sản phẩm mang công dụng này. " +
                              "Hãy bỏ công dụng này khỏi các sản phẩm đó trước.";
            return RedirectToAction(nameof(Index));
        }

        _db.Congdungs.Remove(cd);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Loi"] = $"Không thể xóa công dụng “{cd.Tencongdung}” vì đang được sản phẩm sử dụng.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ThongBao"] = $"Đã xóa công dụng {cd.Macongdung} – {cd.Tencongdung}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== HÀM HỖ TRỢ =====================

    // Sinh mã kế tiếp: CD + (số lớn nhất hiện có + 1)
    private async Task<string> SinhMaMoiAsync()
    {
        var dsMa = await _db.Congdungs.Select(c => c.Macongdung).ToListAsync();
        return MaTuSinh.TaoMaMoi(dsMa, TienToMa);
    }

    // Tìm công dụng theo mã; thiếu mã trên URL thì coi như không tìm thấy
    private async Task<Congdung?> TimCongDungAsync(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;
        return await _db.Congdungs.FindAsync(id);
    }

    // Kiểm tra trùng tên (không phân biệt hoa/thường); khi sửa thì bỏ qua chính nó
    private Task<bool> TrungTenAsync(string ten, string? boQuaMa)
    {
        return _db.Congdungs.AnyAsync(c => c.Tencongdung == ten && c.Macongdung != boQuaMa);
    }

    private IActionResult KhongTimThay(string? id)
    {
        TempData["Loi"] = $"Không tìm thấy công dụng có mã “{id}”. Có thể công dụng đã bị xóa.";
        return RedirectToAction(nameof(Index));
    }
}
