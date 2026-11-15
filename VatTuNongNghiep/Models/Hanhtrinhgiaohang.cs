using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Hanhtrinhgiaohang
{
    public int Maht { get; set; }

    public string Magiao { get; set; } = null!;

    public string Vitri { get; set; } = null!;

    public DateTime Thoigian { get; set; }

    public string? Ghichu { get; set; }

    public string Trangthaihanhtrinh { get; set; } = null!;

    public virtual Giaohang MagiaoNavigation { get; set; } = null!;
}
