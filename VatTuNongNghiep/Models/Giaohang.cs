using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Giaohang
{
    public string Magiao { get; set; } = null!;

    public string Madvvc { get; set; } = null!;

    public string Madh { get; set; } = null!;

    public decimal Chiphi { get; set; }

    public DateTime? Ngaygiao { get; set; }

    public string? Trangthaigiao { get; set; }

    public string Diachigiao { get; set; } = null!;

    public string Mavandon { get; set; } = null!;

    public DateTime Ngaygui { get; set; }

    public virtual ICollection<Hanhtrinhgiaohang> Hanhtrinhgiaohangs { get; set; } = new List<Hanhtrinhgiaohang>();

    public virtual Donhang MadhNavigation { get; set; } = null!;

    public virtual Donvivanchuyen MadvvcNavigation { get; set; } = null!;
}
