using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Nhacungcap
{
    public string Mancc { get; set; } = null!;

    public string Tenncc { get; set; } = null!;

    public string? Diachi { get; set; }

    public string? Sodienthoai { get; set; }

    public virtual ICollection<Phieunhap> Phieunhaps { get; set; } = new List<Phieunhap>();
}
