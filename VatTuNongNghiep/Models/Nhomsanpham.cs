using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Nhomsanpham
{
    public string Manhom { get; set; } = null!;

    public string Tennhom { get; set; } = null!;

    public string? Mota { get; set; }

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
