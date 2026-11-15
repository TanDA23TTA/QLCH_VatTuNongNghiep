using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Khachhang
{
    public string Makh { get; set; } = null!;

    public string Matk { get; set; } = null!;

    public string Hoten { get; set; } = null!;

    public string Sodienthoai { get; set; } = null!;

    public string? Diachi { get; set; }

    public DateTime? Ngaydk { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual Taikhoan MatkNavigation { get; set; } = null!;
}
