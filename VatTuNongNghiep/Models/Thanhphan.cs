using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Thanhphan
{
    public string Mathanhphan { get; set; } = null!;

    public string Tenthanhphan { get; set; } = null!;

    public string? Donvihamluong { get; set; }

    public string Loaithanhphan { get; set; } = null!;

    public virtual ICollection<CoThanhPhan> CoThanhPhans { get; set; } = new List<CoThanhPhan>();
}
