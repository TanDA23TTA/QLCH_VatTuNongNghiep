using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class CoThanhPhan
{
    public string Masp { get; set; } = null!;

    public string Mathanhphan { get; set; } = null!;

    public decimal? Hamluong { get; set; }

    public virtual Sanpham MaspNavigation { get; set; } = null!;

    public virtual Thanhphan MathanhphanNavigation { get; set; } = null!;
}
