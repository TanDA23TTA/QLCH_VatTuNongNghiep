using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VatTuNongNghiep.Areas.Admin.Models;
using VatTuNongNghiep.Helpers;
using VatTuNongNghiep.Models;

namespace VatTuNongNghiep.Areas.Admin.Controllers;

// Quản lý danh mục THƯƠNG HIỆU: danh sách + tìm kiếm, thêm, sửa, xóa
// (làm theo mẫu NhomSanPhamController). URL: /Admin/ThuongHieu
public class ThuongHieuController : AdminBaseController
{
    private const string TienToMa = "TH";      // mã dạng TH001, TH002...
    private const int SoDongMoiTrang = 10;

    private readonly QlVatTuNongNghiepContext _db;

    public ThuongHieuController(QlVatTuNongNghiepContext db)
    {
        _db = db;
    }

    // ===================== DANH SÁCH + TÌM KIẾM =====================
    // GET /Admin/ThuongHieu?tuKhoa=binh dien&trang=1
    public async Task<IActionResult> Index(string? tuKhoa, int trang = 1)
    {
        var truyVan = _db.Thuonghieus.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            tuKhoa = tuKhoa.Trim();
            // Tìm không dấu theo tên hoặc quốc gia, và theo mã
            truyVan = truyVan.Where(t =>
                EF.Functions.Collate(t.Tenthuonghieu, "Latin1_General_100_CI_AI").Contains(tuKhoa)
                || EF.Functions.Collate(t.Quocgia!, "Latin1_General_100_CI_AI").Contains(tuKhoa)
                || t.Mathuonghieu.Contains(tuKhoa));
        }

        var phanTrang = PhanTrangVM.Tao(await truyVan.CountAsync(), trang, SoDongMoiTrang, tuKhoa);

        var ds = await truyVan
            .OrderBy(t => t.Mathuonghieu)
            .Skip(phanTrang.ViTriBatDau)
            .Take(SoDongMoiTrang)
            .Select(t => new ThuongHieuVM
            {
                Mathuonghieu = t.Mathuonghieu,
                TenThuongHieu = t.Tenthuonghieu,
                QuocGia = t.Quocgia,
                DiaChi = t.Diachi,
                SoSanPham = t.Sanphams.Count()
            })
            .ToListAsync();

        ViewBag.PhanTrang = phanTrang;
        return View(ds);
    }

    // ===================== THÊM =====================
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View(new ThuongHieuVM { Mathuonghieu = await SinhMaMoiAsync(), QuocGia = "Việt Nam" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ThuongHieuVM vm)
    {
        vm.TenThuongHieu = vm.TenThuongHieu?.Trim() ?? "";
        if (await TrungTenAsync(vm.TenThuongHieu, null))
            ModelState.AddModelError(nameof(vm.TenThuongHieu), "Tên thương hiệu này đã tồn tại.");

        if (!ModelState.IsValid)
        {
            vm.Mathuonghieu = await SinhMaMoiAsync();
            return View(vm);
        }

        // Sinh mã lại ngay lúc lưu để chắc chắn không trùng
        var th = new Thuonghieu
        {
            Mathuonghieu = await SinhMaMoiAsync(),
            Tenthuonghieu = vm.TenThuongHieu,
            Quocgia = ChuanHoa(vm.QuocGia),
            Diachi = ChuanHoa(vm.DiaChi)
        };
        _db.Thuonghieus.Add(th);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty, "Không lưu được (có thể mã vừa bị người khác dùng). Vui lòng bấm Lưu lại.");
            vm.Mathuonghieu = await SinhMaMoiAsync();
            return View(vm);
        }

        TempData["ThongBao"] = $"Đã thêm thương hiệu {th.Mathuonghieu} – {th.Tenthuonghieu}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== SỬA =====================
    [HttpGet]
    public async Task<IActionResult> Edit(string? id)
    {
        var th = await TimThuongHieuAsync(id);
        if (th == null)
            return KhongTimThay(id);

        return View(new ThuongHieuVM
        {
            Mathuonghieu = th.Mathuonghieu,
            TenThuongHieu = th.Tenthuonghieu,
            QuocGia = th.Quocgia,
            DiaChi = th.Diachi
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, ThuongHieuVM vm)
    {
        var th = await TimThuongHieuAsync(id);
        if (th == null)
            return KhongTimThay(id);

        vm.Mathuonghieu = id;                         // mã không cho sửa
        vm.TenThuongHieu = vm.TenThuongHieu?.Trim() ?? "";
        if (await TrungTenAsync(vm.TenThuongHieu, id))
            ModelState.AddModelError(nameof(vm.TenThuongHieu), "Tên thương hiệu này đã tồn tại.");

        if (!ModelState.IsValid)
            return View(vm);

        th.Tenthuonghieu = vm.TenThuongHieu;
        th.Quocgia = ChuanHoa(vm.QuocGia);
        th.Diachi = ChuanHoa(vm.DiaChi);
        await _db.SaveChangesAsync();

        TempData["ThongBao"] = $"Đã cập nhật thương hiệu {th.Mathuonghieu} – {th.Tenthuonghieu}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== XÓA =====================
    // GET: trang xác nhận xóa
    [HttpGet]
    public async Task<IActionResult> Delete(string? id)
    {
        var vm = await _db.Thuonghieus
            .Where(t => t.Mathuonghieu == id)
            .Select(t => new ThuongHieuVM
            {
                Mathuonghieu = t.Mathuonghieu,
                TenThuongHieu = t.Tenthuonghieu,
                QuocGia = t.Quocgia,
                DiaChi = t.Diachi,
                SoSanPham = t.Sanphams.Count()
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
        var th = await TimThuongHieuAsync(id);
        if (th == null)
            return KhongTimThay(id);

        // Thương hiệu còn sản phẩm thì không cho xóa (khóa ngoại SANPHAM.MATHUONGHIEU)
        int soSanPham = await _db.Sanphams.CountAsync(s => s.Mathuonghieu == id);
        if (soSanPham > 0)
        {
            TempData["Loi"] = $"Không thể xóa thương hiệu “{th.Tenthuonghieu}” vì đang có {soSanPham} sản phẩm thuộc thương hiệu này. " +
                              "Hãy chuyển các sản phẩm sang thương hiệu khác hoặc xóa chúng trước.";
            return RedirectToAction(nameof(Index));
        }

        _db.Thuonghieus.Remove(th);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Loi"] = $"Không thể xóa thương hiệu “{th.Tenthuonghieu}” vì đang được sản phẩm sử dụng.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ThongBao"] = $"Đã xóa thương hiệu {th.Mathuonghieu} – {th.Tenthuonghieu}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== HÀM HỖ TRỢ =====================

    // Sinh mã kế tiếp: TH + (số lớn nhất hiện có + 1)
    private async Task<string> SinhMaMoiAsync()
    {
        var dsMa = await _db.Thuonghieus.Select(t => t.Mathuonghieu).ToListAsync();
        return MaTuSinh.TaoMaMoi(dsMa, TienToMa);
    }

    // Tìm thương hiệu theo mã; thiếu mã trên URL thì coi như không tìm thấy
    private async Task<Thuonghieu?> TimThuongHieuAsync(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;
        return await _db.Thuonghieus.FindAsync(id);
    }

    // Kiểm tra trùng tên (không phân biệt hoa/thường); khi sửa thì bỏ qua chính nó
    private Task<bool> TrungTenAsync(string ten, string? boQuaMa)
    {
        return _db.Thuonghieus.AnyAsync(t => t.Tenthuonghieu == ten && t.Mathuonghieu != boQuaMa);
    }

    // Chuỗi rỗng/khoảng trắng → null, ngược lại cắt khoảng trắng 2 đầu
    private static string? ChuanHoa(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    private IActionResult KhongTimThay(string? id)
    {
        TempData["Loi"] = $"Không tìm thấy thương hiệu có mã “{id}”. Có thể thương hiệu đã bị xóa.";
        return RedirectToAction(nameof(Index));
    }
}
