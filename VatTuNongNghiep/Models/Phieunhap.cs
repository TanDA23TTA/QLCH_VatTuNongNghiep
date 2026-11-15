using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Phieunhap
{
    public string Maphieu { get; set; } = null!;

    public string Mancc { get; set; } = null!;

    public string Matk { get; set; } = null!;

    public DateTime Ngaynhap { get; set; }

    public decimal Tongtiennhap { get; set; }

    public string? Ghichu { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual Nhacungcap ManccNavigation { get; set; } = null!;

    public virtual Taikhoan MatkNavigation { get; set; } = null!;
}
