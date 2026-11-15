using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Thuonghieu
{
    public string Mathuonghieu { get; set; } = null!;

    public string Tenthuonghieu { get; set; } = null!;

    public string? Quocgia { get; set; }

    public string? Diachi { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
