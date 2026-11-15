using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Nhomcaytrong
{
    public string Manhomcay { get; set; } = null!;

    public string Tennhomcay { get; set; } = null!;

    public string? Mota { get; set; }

    public virtual ICollection<ApDungCho> ApDungChos { get; set; } = new List<ApDungCho>();
}
