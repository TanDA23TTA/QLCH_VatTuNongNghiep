namespace VatTuNongNghiep.Areas.Admin.Models;

// Thông tin phân trang dùng chung cho các trang danh sách quản trị
public class PhanTrangVM
{
    public int Trang { get; set; }        // trang hiện tại (bắt đầu từ 1)
    public int TongTrang { get; set; }
    public int TongSo { get; set; }       // tổng số dòng tìm được
    public string? TuKhoa { get; set; }   // giữ lại từ khóa khi chuyển trang
    public int SoDongMoiTrang { get; set; }

    // Các bộ lọc khác cần giữ lại khi chuyển trang (vd: loai = "Đa lượng")
    public Dictionary<string, string?> ThamSoLoc { get; } = new();

    // Số thứ tự của dòng đầu tiên trên trang hiện tại trừ 1 (dùng để đánh STT)
    public int ViTriBatDau => (Trang - 1) * SoDongMoiTrang;

    // Tham số URL cho link sang trang số "trang" (từ khóa + bộ lọc + số trang)
    public Dictionary<string, string> ThamSoUrl(int trang)
    {
        var ts = new Dictionary<string, string> { ["trang"] = trang.ToString() };
        if (!string.IsNullOrEmpty(TuKhoa))
            ts["tuKhoa"] = TuKhoa;
        foreach (var (ten, giaTri) in ThamSoLoc)
            if (!string.IsNullOrEmpty(giaTri))
                ts[ten] = giaTri;
        return ts;
    }

    // Tính tổng số trang và đưa số trang về khoảng hợp lệ
    public static PhanTrangVM Tao(int tongSo, int trang, int soDongMoiTrang, string? tuKhoa)
    {
        int tongTrang = Math.Max(1, (int)Math.Ceiling(tongSo / (double)soDongMoiTrang));
        return new PhanTrangVM
        {
            TongSo = tongSo,
            TongTrang = tongTrang,
            Trang = Math.Clamp(trang, 1, tongTrang),
            TuKhoa = tuKhoa,
            SoDongMoiTrang = soDongMoiTrang
        };
    }
}
