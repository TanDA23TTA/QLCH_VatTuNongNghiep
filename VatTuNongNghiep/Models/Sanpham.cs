using System;
using System.Collections.Generic;

namespace VatTuNongNghiep.Models;

public partial class Sanpham
{
    public string Masp { get; set; } = null!;

    public string Manhom { get; set; } = null!;

    public string Mathuonghieu { get; set; } = null!;

    public string Tensp { get; set; } = null!;

    public string Donvitinh { get; set; } = null!;

    public int Soluongton { get; set; }

    public decimal Giaban { get; set; }

    public string? Hinhanh { get; set; }

    public string? Huongdansudung { get; set; }

    public string? Mota { get; set; }

    public virtual ICollection<ApDungCho> ApDungChos { get; set; } = new List<ApDungCho>();

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual ICollection<CoThanhPhan> CoThanhPhans { get; set; } = new List<CoThanhPhan>();

    public virtual Nhomsanpham ManhomNavigation { get; set; } = null!;

    public virtual Thuonghieu MathuonghieuNavigation { get; set; } = null!;

    public virtual ICollection<Congdung> Macongdungs { get; set; } = new List<Congdung>();
}
