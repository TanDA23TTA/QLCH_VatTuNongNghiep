using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class ChiTietPhieuNhap
{
    public string Maphieu { get; set; } = null!;

    public string Masp { get; set; } = null!;

    public int Soluongnhap { get; set; }

    public decimal Dongianhap { get; set; }

    public virtual Phieunhap MaphieuNavigation { get; set; } = null!;

    public virtual Sanpham MaspNavigation { get; set; } = null!;
}
