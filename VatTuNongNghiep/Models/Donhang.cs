using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Donhang
{
    public string Madh { get; set; } = null!;

    public string Makh { get; set; } = null!;

    public DateTime Ngaydat { get; set; }

    public decimal Tongtien { get; set; }

    public string? Ghichu { get; set; }

    public string Phuongthucthanhtoan { get; set; } = null!;

    public string Trangthaithanhtoan { get; set; } = null!;

    public DateTime? Ngaythanhtoan { get; set; }

    public string Trangthaidon { get; set; } = null!;

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<Giaohang> Giaohangs { get; set; } = new List<Giaohang>();

    public virtual Khachhang MakhNavigation { get; set; } = null!;
}
