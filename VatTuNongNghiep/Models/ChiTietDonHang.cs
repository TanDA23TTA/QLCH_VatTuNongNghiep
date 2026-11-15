using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class ChiTietDonHang
{
    public string Masp { get; set; } = null!;

    public string Madh { get; set; } = null!;

    public int Soluong { get; set; }

    public decimal Dongia { get; set; }

    public virtual Donhang MadhNavigation { get; set; } = null!;

    public virtual Sanpham MaspNavigation { get; set; } = null!;
}
