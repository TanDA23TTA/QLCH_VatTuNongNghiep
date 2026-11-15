using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class ApDungCho
{
    public string Manhomcay { get; set; } = null!;

    public string Masp { get; set; } = null!;

    public string Thoigianbon { get; set; } = null!;

    public decimal Khoiluongbon { get; set; }

    public string? Donvikhoiluong { get; set; }

    public virtual Nhomcaytrong ManhomcayNavigation { get; set; } = null!;

    public virtual Sanpham MaspNavigation { get; set; } = null!;
}
