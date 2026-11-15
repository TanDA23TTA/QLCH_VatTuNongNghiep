using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VatTuNongNghiep.Models;

public partial class QlVatTuNongNghiepContext : DbContext
{
    public QlVatTuNongNghiepContext(DbContextOptions<QlVatTuNongNghiepContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApDungCho> ApDungChos { get; set; }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

    public virtual DbSet<CoThanhPhan> CoThanhPhans { get; set; }

    public virtual DbSet<Congdung> Congdungs { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<Donvivanchuyen> Donvivanchuyens { get; set; }

    public virtual DbSet<Giaohang> Giaohangs { get; set; }

    public virtual DbSet<Hanhtrinhgiaohang> Hanhtrinhgiaohangs { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Nhacungcap> Nhacungcaps { get; set; }

    public virtual DbSet<Nhomcaytrong> Nhomcaytrongs { get; set; }

    public virtual DbSet<Nhomsanpham> Nhomsanphams { get; set; }

    public virtual DbSet<Phieunhap> Phieunhaps { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<Taikhoan> Taikhoans { get; set; }

    public virtual DbSet<Thanhphan> Thanhphans { get; set; }

    public virtual DbSet<Thuonghieu> Thuonghieus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApDungCho>(entity =>
        {
            entity.HasKey(e => new { e.Manhomcay, e.Masp });

            entity.ToTable("AP_DUNG_CHO");

            entity.Property(e => e.Manhomcay)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MANHOMCAY");
            entity.Property(e => e.Masp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MASP");
            entity.Property(e => e.Donvikhoiluong)
                .HasMaxLength(20)
                .HasColumnName("DONVIKHOILUONG");
            entity.Property(e => e.Khoiluongbon)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("KHOILUONGBON");
            entity.Property(e => e.Thoigianbon)
                .HasMaxLength(100)
                .HasColumnName("THOIGIANBON");

            entity.HasOne(d => d.ManhomcayNavigation).WithMany(p => p.ApDungChos)
                .HasForeignKey(d => d.Manhomcay)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FK_ADC_NHOMCAY");

            entity.HasOne(d => d.MaspNavigation).WithMany(p => p.ApDungChos)
                .HasForeignKey(d => d.Masp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AP_DUNG_CHO2");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => new { e.Masp, e.Madh });

            entity.ToTable("CHI_TIET_DON_HANG");

            entity.Property(e => e.Masp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MASP");
            entity.Property(e => e.Madh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MADH");
            entity.Property(e => e.Dongia)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DONGIA");
            entity.Property(e => e.Soluong).HasColumnName("SOLUONG");

            entity.HasOne(d => d.MadhNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.Madh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHI_TIET_DON_HANG2");

            entity.HasOne(d => d.MaspNavigation).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.Masp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FK_CTDH_SANPHAM");
        });

        modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
        {
            entity.HasKey(e => new { e.Maphieu, e.Masp });

            entity.ToTable("CHI_TIET_PHIEU_NHAP");

            entity.Property(e => e.Maphieu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MAPHIEU");
            entity.Property(e => e.Masp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MASP");
            entity.Property(e => e.Dongianhap)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DONGIANHAP");
            entity.Property(e => e.Soluongnhap).HasColumnName("SOLUONGNHAP");

            entity.HasOne(d => d.MaphieuNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.Maphieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FK_CTPN_PHIEUNHAP");

            entity.HasOne(d => d.MaspNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.Masp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHI_TIET_PHIEU_NHAP2");
        });

        modelBuilder.Entity<CoThanhPhan>(entity =>
        {
            entity.HasKey(e => new { e.Masp, e.Mathanhphan });

            entity.ToTable("CO_THANH_PHAN");

            entity.Property(e => e.Masp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MASP");
            entity.Property(e => e.Mathanhphan)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MATHANHPHAN");
            entity.Property(e => e.Hamluong)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("HAMLUONG");

            entity.HasOne(d => d.MaspNavigation).WithMany(p => p.CoThanhPhans)
                .HasForeignKey(d => d.Masp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FK_CTP_SANPHAM");

            entity.HasOne(d => d.MathanhphanNavigation).WithMany(p => p.CoThanhPhans)
                .HasForeignKey(d => d.Mathanhphan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CO_THANH_PHAN2");
        });

        modelBuilder.Entity<Congdung>(entity =>
        {
            entity.HasKey(e => e.Macongdung);

            entity.ToTable("CONGDUNG");

            entity.Property(e => e.Macongdung)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MACONGDUNG");
            entity.Property(e => e.Mota)
                .HasMaxLength(200)
                .HasColumnName("MOTA");
            entity.Property(e => e.Tencongdung)
                .HasMaxLength(50)
                .HasColumnName("TENCONGDUNG");
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.Madh);

            entity.ToTable("DONHANG");

            entity.Property(e => e.Madh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MADH");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(100)
                .HasColumnName("GHICHU");
            entity.Property(e => e.Makh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MAKH");
            entity.Property(e => e.Ngaydat)
                .HasColumnType("datetime")
                .HasColumnName("NGAYDAT");
            entity.Property(e => e.Ngaythanhtoan)
                .HasColumnType("datetime")
                .HasColumnName("NGAYTHANHTOAN");
            entity.Property(e => e.Phuongthucthanhtoan)
                .HasMaxLength(30)
                .HasColumnName("PHUONGTHUCTHANHTOAN");
            entity.Property(e => e.Tongtien)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("TONGTIEN");
            entity.Property(e => e.Trangthaidon)
                .HasMaxLength(30)
                .HasColumnName("TRANGTHAIDON");
            entity.Property(e => e.Trangthaithanhtoan)
                .HasMaxLength(30)
                .HasColumnName("TRANGTHAITHANHTOAN");

            entity.HasOne(d => d.MakhNavigation).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.Makh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DAT_HANG");
        });

        modelBuilder.Entity<Donvivanchuyen>(entity =>
        {
            entity.HasKey(e => e.Madvvc);

            entity.ToTable("DONVIVANCHUYEN");

            entity.Property(e => e.Madvvc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MADVVC");
            entity.Property(e => e.Sdt)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("SDT");
            entity.Property(e => e.Tendvvc)
                .HasMaxLength(50)
                .HasColumnName("TENDVVC");
        });

        modelBuilder.Entity<Giaohang>(entity =>
        {
            entity.HasKey(e => e.Magiao);

            entity.ToTable("GIAOHANG");

            entity.Property(e => e.Magiao)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MAGIAO");
            entity.Property(e => e.Chiphi)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("CHIPHI");
            entity.Property(e => e.Diachigiao)
                .HasMaxLength(150)
                .HasColumnName("DIACHIGIAO");
            entity.Property(e => e.Madh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MADH");
            entity.Property(e => e.Madvvc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MADVVC");
            entity.Property(e => e.Mavandon)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("MAVANDON");
            entity.Property(e => e.Ngaygiao)
                .HasColumnType("datetime")
                .HasColumnName("NGAYGIAO");
            entity.Property(e => e.Ngaygui)
                .HasColumnType("datetime")
                .HasColumnName("NGAYGUI");
            entity.Property(e => e.Trangthaigiao)
                .HasMaxLength(30)
                .HasColumnName("TRANGTHAIGIAO");

            entity.HasOne(d => d.MadhNavigation).WithMany(p => p.Giaohangs)
                .HasForeignKey(d => d.Madh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GIAO_DON");

            entity.HasOne(d => d.MadvvcNavigation).WithMany(p => p.Giaohangs)
                .HasForeignKey(d => d.Madvvc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VAN_CHUYEN_BOI");
        });

        modelBuilder.Entity<Hanhtrinhgiaohang>(entity =>
        {
            entity.HasKey(e => e.Maht);

            entity.ToTable("HANHTRINHGIAOHANG");

            entity.Property(e => e.Maht).HasColumnName("MAHT");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(100)
                .HasColumnName("GHICHU");
            entity.Property(e => e.Magiao)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MAGIAO");
            entity.Property(e => e.Thoigian)
                .HasColumnType("datetime")
                .HasColumnName("THOIGIAN");
            entity.Property(e => e.Trangthaihanhtrinh)
                .HasMaxLength(30)
                .HasColumnName("TRANGTHAIHANHTRINH");
            entity.Property(e => e.Vitri)
                .HasMaxLength(100)
                .HasColumnName("VITRI");

            entity.HasOne(d => d.MagiaoNavigation).WithMany(p => p.Hanhtrinhgiaohangs)
                .HasForeignKey(d => d.Magiao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CO_HANH_TRINH");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.Makh);

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.Makh)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MAKH");
            entity.Property(e => e.Diachi)
                .HasMaxLength(150)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Hoten)
                .HasMaxLength(50)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Matk)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MATK");
            entity.Property(e => e.Ngaydk)
                .HasColumnType("datetime")
                .HasColumnName("NGAYDK");
            entity.Property(e => e.Sodienthoai)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("SODIENTHOAI");

            entity.HasOne(d => d.MatkNavigation).WithMany(p => p.Khachhangs)
                .HasForeignKey(d => d.Matk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CO_TAI_KHOAN");
        });

        modelBuilder.Entity<Nhacungcap>(entity =>
        {
            entity.HasKey(e => e.Mancc);

            entity.ToTable("NHACUNGCAP");

            entity.Property(e => e.Mancc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MANCC");
            entity.Property(e => e.Diachi)
                .HasMaxLength(150)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Sodienthoai)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("SODIENTHOAI");
            entity.Property(e => e.Tenncc)
                .HasMaxLength(100)
                .HasColumnName("TENNCC");
        });

        modelBuilder.Entity<Nhomcaytrong>(entity =>
        {
            entity.HasKey(e => e.Manhomcay);

            entity.ToTable("NHOMCAYTRONG");

            entity.Property(e => e.Manhomcay)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MANHOMCAY");
            entity.Property(e => e.Mota)
                .HasMaxLength(200)
                .HasColumnName("MOTA");
            entity.Property(e => e.Tennhomcay)
                .HasMaxLength(50)
                .HasColumnName("TENNHOMCAY");
        });

        modelBuilder.Entity<Nhomsanpham>(entity =>
        {
            entity.HasKey(e => e.Manhom);

            entity.ToTable("NHOMSANPHAM");

            entity.Property(e => e.Manhom)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MANHOM");
            entity.Property(e => e.Mota)
                .HasMaxLength(200)
                .HasColumnName("MOTA");
            entity.Property(e => e.Tennhom)
                .HasMaxLength(50)
                .HasColumnName("TENNHOM");
        });

        modelBuilder.Entity<Phieunhap>(entity =>
        {
            entity.HasKey(e => e.Maphieu);

            entity.ToTable("PHIEUNHAP");

            entity.Property(e => e.Maphieu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MAPHIEU");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(100)
                .HasColumnName("GHICHU");
            entity.Property(e => e.Mancc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MANCC");
            entity.Property(e => e.Matk)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MATK");
            entity.Property(e => e.Ngaynhap)
                .HasColumnType("datetime")
                .HasColumnName("NGAYNHAP");
            entity.Property(e => e.Tongtiennhap)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("TONGTIENNHAP");

            entity.HasOne(d => d.ManccNavigation).WithMany(p => p.Phieunhaps)
                .HasForeignKey(d => d.Mancc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CUNG_CAP");

            entity.HasOne(d => d.MatkNavigation).WithMany(p => p.Phieunhaps)
                .HasForeignKey(d => d.Matk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LAP_PHIEU");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.Masp);

            entity.ToTable("SANPHAM");

            entity.Property(e => e.Masp)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MASP");
            entity.Property(e => e.Donvitinh)
                .HasMaxLength(20)
                .HasColumnName("DONVITINH");
            entity.Property(e => e.Giaban)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("GIABAN");
            entity.Property(e => e.Hinhanh)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HINHANH");
            entity.Property(e => e.Huongdansudung)
                .HasMaxLength(500)
                .HasColumnName("HUONGDANSUDUNG");
            entity.Property(e => e.Manhom)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MANHOM");
            entity.Property(e => e.Mathuonghieu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MATHUONGHIEU");
            entity.Property(e => e.Mota)
                .HasMaxLength(200)
                .HasColumnName("MOTA");
            entity.Property(e => e.Soluongton).HasColumnName("SOLUONGTON");
            entity.Property(e => e.Tensp)
                .HasMaxLength(100)
                .HasColumnName("TENSP");

            entity.HasOne(d => d.ManhomNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.Manhom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_THUOC_NHOM");

            entity.HasOne(d => d.MathuonghieuNavigation).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.Mathuonghieu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SAN_XUAT_BOI");

            entity.HasMany(d => d.Macongdungs).WithMany(p => p.Masps)
                .UsingEntity<Dictionary<string, object>>(
                    "CoCongDung",
                    r => r.HasOne<Congdung>().WithMany()
                        .HasForeignKey("Macongdung")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_CO_CONG_DUNG2"),
                    l => l.HasOne<Sanpham>().WithMany()
                        .HasForeignKey("Masp")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_FK_CCD_SANPHAM"),
                    j =>
                    {
                        j.HasKey("Masp", "Macongdung");
                        j.ToTable("CO_CONG_DUNG");
                        j.IndexerProperty<string>("Masp")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("MASP");
                        j.IndexerProperty<string>("Macongdung")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("MACONGDUNG");
                    });
        });

        modelBuilder.Entity<Taikhoan>(entity =>
        {
            entity.HasKey(e => e.Matk);

            entity.ToTable("TAIKHOAN");

            entity.HasIndex(e => e.Tendangnhap, "UQ_TAIKHOAN_TENDANGNHAP").IsUnique();

            entity.Property(e => e.Matk)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MATK");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Matkhau)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("MATKHAU");
            entity.Property(e => e.Tendangnhap)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TENDANGNHAP");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(30)
                .HasColumnName("TRANGTHAI");
            entity.Property(e => e.Vaitro)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("VAITRO");
        });

        modelBuilder.Entity<Thanhphan>(entity =>
        {
            entity.HasKey(e => e.Mathanhphan);

            entity.ToTable("THANHPHAN");

            entity.Property(e => e.Mathanhphan)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MATHANHPHAN");
            entity.Property(e => e.Donvihamluong)
                .HasMaxLength(10)
                .HasColumnName("DONVIHAMLUONG");
            entity.Property(e => e.Loaithanhphan)
                .HasMaxLength(30)
                .HasColumnName("LOAITHANHPHAN");
            entity.Property(e => e.Tenthanhphan)
                .HasMaxLength(50)
                .HasColumnName("TENTHANHPHAN");
        });

        modelBuilder.Entity<Thuonghieu>(entity =>
        {
            entity.HasKey(e => e.Mathuonghieu);

            entity.ToTable("THUONGHIEU");

            entity.Property(e => e.Mathuonghieu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MATHUONGHIEU");
            entity.Property(e => e.Diachi)
                .HasMaxLength(150)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Quocgia)
                .HasMaxLength(50)
                .HasColumnName("QUOCGIA");
            entity.Property(e => e.Tenthuonghieu)
                .HasMaxLength(50)
                .HasColumnName("TENTHUONGHIEU");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
