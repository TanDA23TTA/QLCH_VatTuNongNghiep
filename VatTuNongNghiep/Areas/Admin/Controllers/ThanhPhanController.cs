using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VatTuNongNghiep.Areas.Admin.Models;
using VatTuNongNghiep.Helpers;
using VatTuNongNghiep.Models;

namespace VatTuNongNghiep.Areas.Admin.Controllers;

// Quản lý danh mục THÀNH PHẦN (N, P2O5, K2O, Ca, Mg, Zn...): danh sách + tìm kiếm + lọc theo loại, thêm, sửa, xóa
// (làm theo mẫu NhomSanPhamController). URL: /Admin/ThanhPhan
public class ThanhPhanController : AdminBaseController
{
    private const string TienToMa = "TP";      // mã dạng TP001, TP002...
    private const int SoDongMoiTrang = 10;

    private readonly QlVatTuNongNghiepContext _db;

    public ThanhPhanController(QlVatTuNongNghiepContext db)
    {
        _db = db;
    }

    // ===================== DANH SÁCH + TÌM KIẾM + LỌC =====================
    // GET /Admin/ThanhPhan?tuKhoa=kali&loai=Đa lượng&trang=1
    public async Task<IActionResult> Index(string? tuKhoa, string? loai, int trang = 1)
    {
        var truyVan = _db.Thanhphans.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            tuKhoa = tuKhoa.Trim();
            // Tìm không dấu theo tên, và theo mã
            truyVan = truyVan.Where(t =>
                EF.Functions.Collate(t.Tenthanhphan, "Latin1_General_100_CI_AI").Contains(tuKhoa)
                || t.Mathanhphan.Contains(tuKhoa));
        }

        // Lọc theo loại thành phần (chọn từ danh sách)
        if (!string.IsNullOrWhiteSpace(loai))
            truyVan = truyVan.Where(t => t.Loaithanhphan == loai);

        var phanTrang = PhanTrangVM.Tao(await truyVan.CountAsync(), trang, SoDongMoiTrang, tuKhoa);
        phanTrang.ThamSoLoc["loai"] = loai;       // giữ bộ lọc khi chuyển trang

        var ds = await truyVan
            .OrderBy(t => t.Mathanhphan)
            .Skip(phanTrang.ViTriBatDau)
            .Take(SoDongMoiTrang)
            .Select(t => new ThanhPhanVM
            {
                Mathanhphan = t.Mathanhphan,
                TenThanhPhan = t.Tenthanhphan,
                DonViHamLuong = t.Donvihamluong,
                LoaiThanhPhan = t.Loaithanhphan,
                SoSanPham = t.CoThanhPhans.Count()
            })
            .ToListAsync();

        ViewBag.PhanTrang = phanTrang;
        ViewBag.Loai = loai;
        return View(ds);
    }

    // ===================== THÊM =====================
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View(new ThanhPhanVM { Mathanhphan = await SinhMaMoiAsync(), DonViHamLuong = "%" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ThanhPhanVM vm)
    {
        await KiemTraHopLeAsync(vm, null);

        if (!ModelState.IsValid)
        {
            vm.Mathanhphan = await SinhMaMoiAsync();
            return View(vm);
        }

        // Sinh mã lại ngay lúc lưu để chắc chắn không trùng
        var tp = new Thanhphan
        {
            Mathanhphan = await SinhMaMoiAsync(),
            Tenthanhphan = vm.TenThanhPhan,
            Donvihamluong = ChuanHoa(vm.DonViHamLuong),
            Loaithanhphan = vm.LoaiThanhPhan
        };
        _db.Thanhphans.Add(tp);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty, "Không lưu được (có thể mã vừa bị người khác dùng). Vui lòng bấm Lưu lại.");
            vm.Mathanhphan = await SinhMaMoiAsync();
            return View(vm);
        }

        TempData["ThongBao"] = $"Đã thêm thành phần {tp.Mathanhphan} – {tp.Tenthanhphan}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== SỬA =====================
    [HttpGet]
    public async Task<IActionResult> Edit(string? id)
    {
        var tp = await TimThanhPhanAsync(id);
        if (tp == null)
            return KhongTimThay(id);

        return View(new ThanhPhanVM
        {
            Mathanhphan = tp.Mathanhphan,
            TenThanhPhan = tp.Tenthanhphan,
            DonViHamLuong = tp.Donvihamluong,
            LoaiThanhPhan = tp.Loaithanhphan
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, ThanhPhanVM vm)
    {
        var tp = await TimThanhPhanAsync(id);
        if (tp == null)
            return KhongTimThay(id);

        vm.Mathanhphan = id;                          // mã không cho sửa
        await KiemTraHopLeAsync(vm, id);

        if (!ModelState.IsValid)
            return View(vm);

        tp.Tenthanhphan = vm.TenThanhPhan;
        tp.Donvihamluong = ChuanHoa(vm.DonViHamLuong);
        tp.Loaithanhphan = vm.LoaiThanhPhan;
        await _db.SaveChangesAsync();

        TempData["ThongBao"] = $"Đã cập nhật thành phần {tp.Mathanhphan} – {tp.Tenthanhphan}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== XÓA =====================
    // GET: trang xác nhận xóa
    [HttpGet]
    public async Task<IActionResult> Delete(string? id)
    {
        var vm = await _db.Thanhphans
            .Where(t => t.Mathanhphan == id)
            .Select(t => new ThanhPhanVM
            {
                Mathanhphan = t.Mathanhphan,
                TenThanhPhan = t.Tenthanhphan,
                DonViHamLuong = t.Donvihamluong,
                LoaiThanhPhan = t.Loaithanhphan,
                SoSanPham = t.CoThanhPhans.Count()
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
        var tp = await TimThanhPhanAsync(id);
        if (tp == null)
            return KhongTimThay(id);

        // Thành phần đang có trong sản phẩm (bảng CO_THANH_PHAN) thì không cho xóa
        int soSanPham = await _db.CoThanhPhans.CountAsync(c => c.Mathanhphan == id);
        if (soSanPham > 0)
        {
            TempData["Loi"] = $"Không thể xóa thành phần “{tp.Tenthanhphan}” vì đang có {soSanPham} sản phẩm chứa thành phần này. " +
                              "Hãy bỏ thành phần này khỏi các sản phẩm đó trước.";
            return RedirectToAction(nameof(Index));
        }

        _db.Thanhphans.Remove(tp);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Loi"] = $"Không thể xóa thành phần “{tp.Tenthanhphan}” vì đang được sản phẩm sử dụng.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ThongBao"] = $"Đã xóa thành phần {tp.Mathanhphan} – {tp.Tenthanhphan}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== HÀM HỖ TRỢ =====================

    // Kiểm tra thêm ngoài DataAnnotations: trùng tên, loại phải nằm trong danh sách
    private async Task KiemTraHopLeAsync(ThanhPhanVM vm, string? boQuaMa)
    {
        vm.TenThanhPhan = vm.TenThanhPhan?.Trim() ?? "";

        if (await _db.Thanhphans.AnyAsync(t => t.Tenthanhphan == vm.TenThanhPhan && t.Mathanhphan != boQuaMa))
            ModelState.AddModelError(nameof(vm.TenThanhPhan), "Tên thành phần này đã tồn tại.");

        // Không tin giá trị gửi từ form: chỉ chấp nhận loại có trong danh sách
        if (!string.IsNullOrEmpty(vm.LoaiThanhPhan) && !LoaiThanhPhan.DanhSach.Contains(vm.LoaiThanhPhan))
            ModelState.AddModelError(nameof(vm.LoaiThanhPhan), "Loại thành phần không hợp lệ.");
    }

    // Sinh mã kế tiếp: TP + (số lớn nhất hiện có + 1)
    private async Task<string> SinhMaMoiAsync()
    {
        var dsMa = await _db.Thanhphans.Select(t => t.Mathanhphan).ToListAsync();
        return MaTuSinh.TaoMaMoi(dsMa, TienToMa);
    }

    // Tìm thành phần theo mã; thiếu mã trên URL thì coi như không tìm thấy
    private async Task<Thanhphan?> TimThanhPhanAsync(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;
        return await _db.Thanhphans.FindAsync(id);
    }

    // Chuỗi rỗng/khoảng trắng → null, ngược lại cắt khoảng trắng 2 đầu
    private static string? ChuanHoa(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    private IActionResult KhongTimThay(string? id)
    {
        TempData["Loi"] = $"Không tìm thấy thành phần có mã “{id}”. Có thể thành phần đã bị xóa.";
        return RedirectToAction(nameof(Index));
    }
}
