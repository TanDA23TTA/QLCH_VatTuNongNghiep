namespace VatTuNongNghiep.Helpers;

// Sinh mã khóa chính dạng TIỀN TỐ + SỐ (vd: TK001, KH002, NH009)
public static class MaTuSinh
{
    /// <summary>
    /// Tìm số thứ tự lớn nhất trong các mã đang có rồi cộng 1.
    /// Ví dụ đang có NH001..NH008 → trả về NH009 (kể cả khi đã xóa NH005 thì vẫn không dùng lại).
    /// </summary>
    public static string TaoMaMoi(IEnumerable<string> dsMaHienCo, string tienTo, int soChuSo = 3)
    {
        int lonNhat = 0;
        foreach (var ma in dsMaHienCo)
        {
            // Chỉ xét mã đúng tiền tố và phần sau là số
            if (ma.StartsWith(tienTo) && int.TryParse(ma.Substring(tienTo.Length), out int so) && so > lonNhat)
                lonNhat = so;
        }
        return tienTo + (lonNhat + 1).ToString().PadLeft(soChuSo, '0');
    }
}
