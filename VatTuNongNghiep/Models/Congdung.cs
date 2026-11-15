using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Congdung
{
    public string Macongdung { get; set; } = null!;

    public string Tencongdung { get; set; } = null!;

    public string? Mota { get; set; }

    public virtual ICollection<Sanpham> Masps { get; set; } = new List<Sanpham>();
}
