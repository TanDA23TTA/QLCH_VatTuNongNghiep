using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Taikhoan
{
    public string Matk { get; set; } = null!;

    public string Tendangnhap { get; set; } = null!;

    public string Matkhau { get; set; } = null!;

    public string? Email { get; set; }

    public string? Trangthai { get; set; }

    public string Vaitro { get; set; } = null!;

    public virtual ICollection<Khachhang> Khachhangs { get; set; } = new List<Khachhang>();

    public virtual ICollection<Phieunhap> Phieunhaps { get; set; } = new List<Phieunhap>();
}
