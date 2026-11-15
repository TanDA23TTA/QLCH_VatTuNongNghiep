using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Donvivanchuyen
{
    public string Madvvc { get; set; } = null!;

    public string Tendvvc { get; set; } = null!;

    public string? Sdt { get; set; }

    public virtual ICollection<Giaohang> Giaohangs { get; set; } = new List<Giaohang>();
}
