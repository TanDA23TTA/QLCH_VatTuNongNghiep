using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VatTuNongNghiep.Areas.Admin.Models;
using VatTuNongNghiep.Helpers;
using VatTuNongNghiep.Models;

namespace VatTuNongNghiep.Areas.Admin.Controllers;

// Quản lý danh mục NHÓM SẢN PHẨM: danh sách + tìm kiếm, thêm, sửa, xóa
// Kế thừa AdminBaseController → chỉ Admin truy cập được. URL: /Admin/NhomSanPham
public class NhomSanPhamController : AdminBaseController
{
    private const string TienToMa = "NH";      // mã dạng NH001, NH002...
    private const int SoDongMoiTrang = 10;

    private readonly QlVatTuNongNghiepContext _db;

    public NhomSanPhamController(QlVatTuNongNghiepContext db)
    {
        _db = db;
    }

    // ===================== DANH SÁCH + TÌM KIẾM =====================
    // GET /Admin/NhomSanPham?tuKhoa=phan&trang=1
    public async Task<IActionResult> Index(string? tuKhoa, int trang = 1)
    {
        var truyVan = _db.Nhomsanphams.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            tuKhoa = tuKhoa.Trim();
            // COLLATE ..._CI_AI: không phân biệt hoa/thường và có dấu/không dấu
            // → gõ "phan dam" vẫn tìm ra "Phân đạm". Tìm theo cả tên và mã.
            truyVan = truyVan.Where(n =>
                EF.Functions.Collate(n.Tennhom, "Latin1_General_100_CI_AI").Contains(tuKhoa)
                || n.Manhom.Contains(tuKhoa));
        }

        var phanTrang = PhanTrangVM.Tao(await truyVan.CountAsync(), trang, SoDongMoiTrang, tuKhoa);

        var ds = await truyVan
            .OrderBy(n => n.Manhom)
            .Skip(phanTrang.ViTriBatDau)
            .Take(SoDongMoiTrang)
            .Select(n => new NhomSanPhamVM
            {
                Manhom = n.Manhom,
                TenNhom = n.Tennhom,
                MoTa = n.Mota,
                SoSanPham = n.Sanphams.Count()     // đếm sản phẩm thuộc nhóm
            })
            .ToListAsync();

        ViewBag.PhanTrang = phanTrang;
        return View(ds);
    }

    // ===================== THÊM =====================
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        // Hiển thị trước mã sẽ được cấp (chỉ để xem)
        return View(new NhomSanPhamVM { Manhom = await SinhMaMoiAsync() });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NhomSanPhamVM vm)
    {
        vm.TenNhom = vm.TenNhom?.Trim() ?? "";
        if (await TrungTenAsync(vm.TenNhom, null))
            ModelState.AddModelError(nameof(vm.TenNhom), "Tên nhóm này đã tồn tại.");

        if (!ModelState.IsValid)
        {
            vm.Manhom = await SinhMaMoiAsync();
            return View(vm);
        }

        // Sinh mã lại ngay lúc lưu (không tin mã gửi từ form) để chắc chắn không trùng
        var nhom = new Nhomsanpham
        {
            Manhom = await SinhMaMoiAsync(),
            Tennhom = vm.TenNhom,
            Mota = string.IsNullOrWhiteSpace(vm.MoTa) ? null : vm.MoTa.Trim()
        };
        _db.Nhomsanphams.Add(nhom);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(string.Empty, "Không lưu được (có thể mã vừa bị người khác dùng). Vui lòng bấm Lưu lại.");
            vm.Manhom = await SinhMaMoiAsync();
            return View(vm);
        }

        TempData["ThongBao"] = $"Đã thêm nhóm sản phẩm {nhom.Manhom} – {nhom.Tennhom}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== SỬA =====================
    [HttpGet]
    public async Task<IActionResult> Edit(string? id)
    {
        var nhom = await TimNhomAsync(id);
        if (nhom == null)
            return KhongTimThay(id);

        return View(new NhomSanPhamVM { Manhom = nhom.Manhom, TenNhom = nhom.Tennhom, MoTa = nhom.Mota });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? id, NhomSanPhamVM vm)
    {
        var nhom = await TimNhomAsync(id);
        if (nhom == null)
            return KhongTimThay(id);

        vm.Manhom = id;                               // mã không cho sửa
        vm.TenNhom = vm.TenNhom?.Trim() ?? "";
        if (await TrungTenAsync(vm.TenNhom, id))
            ModelState.AddModelError(nameof(vm.TenNhom), "Tên nhóm này đã tồn tại.");

        if (!ModelState.IsValid)
            return View(vm);

        nhom.Tennhom = vm.TenNhom;
        nhom.Mota = string.IsNullOrWhiteSpace(vm.MoTa) ? null : vm.MoTa.Trim();
        await _db.SaveChangesAsync();

        TempData["ThongBao"] = $"Đã cập nhật nhóm sản phẩm {nhom.Manhom} – {nhom.Tennhom}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== XÓA =====================
    // GET: trang xác nhận xóa (cho biết nhóm có đang chứa sản phẩm không)
    [HttpGet]
    public async Task<IActionResult> Delete(string? id)
    {
        var vm = await _db.Nhomsanphams
            .Where(n => n.Manhom == id)
            .Select(n => new NhomSanPhamVM
            {
                Manhom = n.Manhom,
                TenNhom = n.Tennhom,
                MoTa = n.Mota,
                SoSanPham = n.Sanphams.Count()
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
        var nhom = await TimNhomAsync(id);
        if (nhom == null)
            return KhongTimThay(id);

        // Kiểm tra trước: nhóm còn sản phẩm thì không cho xóa (khóa ngoại SANPHAM.MANHOM)
        int soSanPham = await _db.Sanphams.CountAsync(s => s.Manhom == id);
        if (soSanPham > 0)
        {
            TempData["Loi"] = $"Không thể xóa nhóm “{nhom.Tennhom}” vì đang có {soSanPham} sản phẩm thuộc nhóm này. " +
                              "Hãy chuyển các sản phẩm sang nhóm khác hoặc xóa chúng trước.";
            return RedirectToAction(nameof(Index));
        }

        _db.Nhomsanphams.Remove(nhom);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            // Phòng trường hợp vừa có sản phẩm được thêm vào nhóm giữa lúc kiểm tra và lúc xóa
            TempData["Loi"] = $"Không thể xóa nhóm “{nhom.Tennhom}” vì đang được sản phẩm sử dụng.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ThongBao"] = $"Đã xóa nhóm sản phẩm {nhom.Manhom} – {nhom.Tennhom}.";
        return RedirectToAction(nameof(Index));
    }

    // ===================== HÀM HỖ TRỢ =====================

    // Sinh mã kế tiếp: NH + (số lớn nhất hiện có + 1)
    private async Task<string> SinhMaMoiAsync()
    {
        var dsMa = await _db.Nhomsanphams.Select(n => n.Manhom).ToListAsync();
        return MaTuSinh.TaoMaMoi(dsMa, TienToMa);
    }

    // Tìm nhóm theo mã; thiếu mã trên URL thì coi như không tìm thấy
    private async Task<Nhomsanpham?> TimNhomAsync(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;
        return await _db.Nhomsanphams.FindAsync(id);
    }

    // Kiểm tra trùng tên (không phân biệt hoa/thường); khi sửa thì bỏ qua chính nó
    private Task<bool> TrungTenAsync(string ten, string? boQuaMa)
    {
        return _db.Nhomsanphams.AnyAsync(n => n.Tennhom == ten && n.Manhom != boQuaMa);
    }

    // Mã không tồn tại (vd: đã bị xóa ở tab khác) → quay về danh sách và báo lỗi
    private IActionResult KhongTimThay(string? id)
    {
        TempData["Loi"] = $"Không tìm thấy nhóm sản phẩm có mã “{id}”. Có thể nhóm đã bị xóa.";
        return RedirectToAction(nameof(Index));
    }
}
