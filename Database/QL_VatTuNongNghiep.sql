/* =====================================================================
   Script tạo CSDL: Quản lý cửa hàng bán vật tư nông nghiệp (phân bón)
   Sinh từ mô hình PDM: MoHinhVatTuNongNghiepPDM (SQL Server)
   Chạy trong SSMS: bôi đen toàn bộ rồi bấm Execute (F5)
   ===================================================================== */
USE master;
GO
IF DB_ID(N'QL_VatTuNongNghiep') IS NULL
    CREATE DATABASE QL_VatTuNongNghiep;
GO
USE QL_VatTuNongNghiep;
GO

/* ===== NhomSanPham ===== */
CREATE TABLE NHOMSANPHAM (
    MANHOM                varchar(10) NOT NULL,
    TENNHOM               nvarchar(50) NOT NULL,
    MOTA                  nvarchar(200) NULL,
    CONSTRAINT PK_NHOMSANPHAM PRIMARY KEY (MANHOM)
);
GO

/* ===== ThuongHieu ===== */
CREATE TABLE THUONGHIEU (
    MATHUONGHIEU          varchar(10) NOT NULL,
    TENTHUONGHIEU         nvarchar(50) NOT NULL,
    QUOCGIA               nvarchar(50) NULL,
    DIACHI                nvarchar(150) NULL,
    CONSTRAINT PK_THUONGHIEU PRIMARY KEY (MATHUONGHIEU)
);
GO

/* ===== SanPham ===== */
CREATE TABLE SANPHAM (
    MASP                  varchar(10) NOT NULL,
    MANHOM                varchar(10) NOT NULL,
    MATHUONGHIEU          varchar(10) NOT NULL,
    TENSP                 nvarchar(100) NOT NULL,
    DONVITINH             nvarchar(20) NOT NULL,
    SOLUONGTON            int NOT NULL,
    GIABAN                decimal(18,0) NOT NULL,
    HINHANH               varchar(100) NULL,
    HUONGDANSUDUNG        nvarchar(500) NULL,
    MOTA                  nvarchar(200) NULL,
    CONSTRAINT PK_SANPHAM PRIMARY KEY (MASP)
);
GO

/* ===== ThanhPhan ===== */
CREATE TABLE THANHPHAN (
    MATHANHPHAN           varchar(10) NOT NULL,
    TENTHANHPHAN          nvarchar(50) NOT NULL,
    DONVIHAMLUONG         nvarchar(10) NULL,
    LOAITHANHPHAN         nvarchar(30) NOT NULL,
    CONSTRAINT PK_THANHPHAN PRIMARY KEY (MATHANHPHAN)
);
GO

/* ===== CongDung ===== */
CREATE TABLE CONGDUNG (
    MACONGDUNG            varchar(10) NOT NULL,
    TENCONGDUNG           nvarchar(50) NOT NULL,
    MOTA                  nvarchar(200) NULL,
    CONSTRAINT PK_CONGDUNG PRIMARY KEY (MACONGDUNG)
);
GO

/* ===== NhomCayTrong ===== */
CREATE TABLE NHOMCAYTRONG (
    MANHOMCAY             varchar(10) NOT NULL,
    TENNHOMCAY            nvarchar(50) NOT NULL,
    MOTA                  nvarchar(200) NULL,
    CONSTRAINT PK_NHOMCAYTRONG PRIMARY KEY (MANHOMCAY)
);
GO

/* ===== TaiKhoan ===== */
CREATE TABLE TAIKHOAN (
    MATK                  varchar(10) NOT NULL,
    TENDANGNHAP           varchar(20) NOT NULL,
    MATKHAU               varchar(255) NOT NULL,
    EMAIL                 varchar(100) NULL,
    TRANGTHAI             nvarchar(30) NULL,
    VAITRO                varchar(20) NOT NULL,
    CONSTRAINT PK_TAIKHOAN PRIMARY KEY (MATK),
    CONSTRAINT UQ_TAIKHOAN_TENDANGNHAP UNIQUE (TENDANGNHAP)
);
GO

/* ===== KhachHang ===== */
CREATE TABLE KHACHHANG (
    MAKH                  varchar(10) NOT NULL,
    MATK                  varchar(10) NOT NULL,
    HOTEN                 nvarchar(50) NOT NULL,
    SODIENTHOAI           varchar(12) NOT NULL,
    DIACHI                nvarchar(150) NULL,
    NGAYDK                datetime NULL,
    CONSTRAINT PK_KHACHHANG PRIMARY KEY (MAKH)
);
GO

/* ===== DonHang ===== */
CREATE TABLE DONHANG (
    MADH                  varchar(10) NOT NULL,
    MAKH                  varchar(10) NOT NULL,
    NGAYDAT               datetime NOT NULL,
    TONGTIEN              decimal(18,0) NOT NULL,
    GHICHU                nvarchar(100) NULL,
    PHUONGTHUCTHANHTOAN   nvarchar(30) NOT NULL,
    TRANGTHAITHANHTOAN    nvarchar(30) NOT NULL,
    NGAYTHANHTOAN         datetime NULL,
    TRANGTHAIDON          nvarchar(30) NOT NULL,
    CONSTRAINT PK_DONHANG PRIMARY KEY (MADH)
);
GO

/* ===== NhaCungCap ===== */
CREATE TABLE NHACUNGCAP (
    MANCC                 varchar(10) NOT NULL,
    TENNCC                nvarchar(100) NOT NULL,
    DIACHI                nvarchar(150) NULL,
    SODIENTHOAI           varchar(12) NULL,
    CONSTRAINT PK_NHACUNGCAP PRIMARY KEY (MANCC)
);
GO

/* ===== PhieuNhap ===== */
CREATE TABLE PHIEUNHAP (
    MAPHIEU               varchar(10) NOT NULL,
    MANCC                 varchar(10) NOT NULL,
    MATK                  varchar(10) NOT NULL,
    NGAYNHAP              datetime NOT NULL,
    TONGTIENNHAP          decimal(18,0) NOT NULL,
    GHICHU                nvarchar(100) NULL,
    CONSTRAINT PK_PHIEUNHAP PRIMARY KEY (MAPHIEU)
);
GO

/* ===== DonViVanChuyen ===== */
CREATE TABLE DONVIVANCHUYEN (
    MADVVC                varchar(10) NOT NULL,
    TENDVVC               nvarchar(50) NOT NULL,
    SDT                   varchar(12) NULL,
    CONSTRAINT PK_DONVIVANCHUYEN PRIMARY KEY (MADVVC)
);
GO

/* ===== Co_Thanh_Phan ===== */
CREATE TABLE CO_THANH_PHAN (
    MASP                  varchar(10) NOT NULL,
    MATHANHPHAN           varchar(10) NOT NULL,
    HAMLUONG              decimal(5,2) NULL,
    CONSTRAINT PK_CO_THANH_PHAN PRIMARY KEY (MASP, MATHANHPHAN)
);
GO

/* ===== Co_Cong_Dung ===== */
CREATE TABLE CO_CONG_DUNG (
    MASP                  varchar(10) NOT NULL,
    MACONGDUNG            varchar(10) NOT NULL,
    CONSTRAINT PK_CO_CONG_DUNG PRIMARY KEY (MASP, MACONGDUNG)
);
GO

/* ===== Ap_Dung_Cho ===== */
CREATE TABLE AP_DUNG_CHO (
    MANHOMCAY             varchar(10) NOT NULL,
    MASP                  varchar(10) NOT NULL,
    THOIGIANBON           nvarchar(100) NOT NULL,
    KHOILUONGBON          decimal(8,2) NOT NULL,
    DONVIKHOILUONG        nvarchar(20) NULL,
    CONSTRAINT PK_AP_DUNG_CHO PRIMARY KEY (MANHOMCAY, MASP)
);
GO

/* ===== Chi_Tiet_Don_Hang ===== */
CREATE TABLE CHI_TIET_DON_HANG (
    MASP                  varchar(10) NOT NULL,
    MADH                  varchar(10) NOT NULL,
    SOLUONG               int NOT NULL,
    DONGIA                decimal(18,0) NOT NULL,
    CONSTRAINT PK_CHI_TIET_DON_HANG PRIMARY KEY (MASP, MADH)
);
GO

/* ===== Chi_Tiet_Phieu_Nhap ===== */
CREATE TABLE CHI_TIET_PHIEU_NHAP (
    MAPHIEU               varchar(10) NOT NULL,
    MASP                  varchar(10) NOT NULL,
    SOLUONGNHAP           int NOT NULL,
    DONGIANHAP            decimal(18,0) NOT NULL,
    CONSTRAINT PK_CHI_TIET_PHIEU_NHAP PRIMARY KEY (MAPHIEU, MASP)
);
GO

/* ===== GiaoHang ===== */
CREATE TABLE GIAOHANG (
    MAGIAO                varchar(10) NOT NULL,
    MADVVC                varchar(10) NOT NULL,
    MADH                  varchar(10) NOT NULL,
    CHIPHI                decimal(18,0) NOT NULL,
    NGAYGIAO              datetime NULL,
    TRANGTHAIGIAO         nvarchar(30) NULL,
    DIACHIGIAO            nvarchar(150) NOT NULL,
    MAVANDON              varchar(30) NOT NULL,
    NGAYGUI               datetime NOT NULL,
    CONSTRAINT PK_GIAOHANG PRIMARY KEY (MAGIAO)
);
GO

/* ===== HanhTrinhGiaoHang ===== */
CREATE TABLE HANHTRINHGIAOHANG (
    MAHT                  int IDENTITY(1,1) NOT NULL,
    MAGIAO                varchar(10) NOT NULL,
    VITRI                 nvarchar(100) NOT NULL,
    THOIGIAN              datetime NOT NULL,
    GHICHU                nvarchar(100) NULL,
    TRANGTHAIHANHTRINH    nvarchar(30) NOT NULL,
    CONSTRAINT PK_HANHTRINHGIAOHANG PRIMARY KEY (MAHT)
);
GO

/* ===== KHÓA NGOẠI ===== */
ALTER TABLE SANPHAM ADD CONSTRAINT FK_THUOC_NHOM FOREIGN KEY (MANHOM)
    REFERENCES NHOMSANPHAM (MANHOM);
GO

ALTER TABLE SANPHAM ADD CONSTRAINT FK_SAN_XUAT_BOI FOREIGN KEY (MATHUONGHIEU)
    REFERENCES THUONGHIEU (MATHUONGHIEU);
GO

ALTER TABLE KHACHHANG ADD CONSTRAINT FK_CO_TAI_KHOAN FOREIGN KEY (MATK)
    REFERENCES TAIKHOAN (MATK);
GO

ALTER TABLE DONHANG ADD CONSTRAINT FK_DAT_HANG FOREIGN KEY (MAKH)
    REFERENCES KHACHHANG (MAKH);
GO

ALTER TABLE PHIEUNHAP ADD CONSTRAINT FK_CUNG_CAP FOREIGN KEY (MANCC)
    REFERENCES NHACUNGCAP (MANCC);
GO

ALTER TABLE PHIEUNHAP ADD CONSTRAINT FK_LAP_PHIEU FOREIGN KEY (MATK)
    REFERENCES TAIKHOAN (MATK);
GO

ALTER TABLE GIAOHANG ADD CONSTRAINT FK_VAN_CHUYEN_BOI FOREIGN KEY (MADVVC)
    REFERENCES DONVIVANCHUYEN (MADVVC);
GO

ALTER TABLE HANHTRINHGIAOHANG ADD CONSTRAINT FK_CO_HANH_TRINH FOREIGN KEY (MAGIAO)
    REFERENCES GIAOHANG (MAGIAO);
GO

ALTER TABLE CO_THANH_PHAN ADD CONSTRAINT FK_FK_CTP_SANPHAM FOREIGN KEY (MASP)
    REFERENCES SANPHAM (MASP);
GO

ALTER TABLE CO_THANH_PHAN ADD CONSTRAINT FK_CO_THANH_PHAN2 FOREIGN KEY (MATHANHPHAN)
    REFERENCES THANHPHAN (MATHANHPHAN);
GO

ALTER TABLE CO_CONG_DUNG ADD CONSTRAINT FK_FK_CCD_SANPHAM FOREIGN KEY (MASP)
    REFERENCES SANPHAM (MASP);
GO

ALTER TABLE CO_CONG_DUNG ADD CONSTRAINT FK_CO_CONG_DUNG2 FOREIGN KEY (MACONGDUNG)
    REFERENCES CONGDUNG (MACONGDUNG);
GO

ALTER TABLE AP_DUNG_CHO ADD CONSTRAINT FK_FK_ADC_NHOMCAY FOREIGN KEY (MANHOMCAY)
    REFERENCES NHOMCAYTRONG (MANHOMCAY);
GO

ALTER TABLE AP_DUNG_CHO ADD CONSTRAINT FK_AP_DUNG_CHO2 FOREIGN KEY (MASP)
    REFERENCES SANPHAM (MASP);
GO

ALTER TABLE CHI_TIET_DON_HANG ADD CONSTRAINT FK_FK_CTDH_SANPHAM FOREIGN KEY (MASP)
    REFERENCES SANPHAM (MASP);
GO

ALTER TABLE CHI_TIET_DON_HANG ADD CONSTRAINT FK_CHI_TIET_DON_HANG2 FOREIGN KEY (MADH)
    REFERENCES DONHANG (MADH);
GO

ALTER TABLE CHI_TIET_PHIEU_NHAP ADD CONSTRAINT FK_FK_CTPN_PHIEUNHAP FOREIGN KEY (MAPHIEU)
    REFERENCES PHIEUNHAP (MAPHIEU);
GO

ALTER TABLE CHI_TIET_PHIEU_NHAP ADD CONSTRAINT FK_CHI_TIET_PHIEU_NHAP2 FOREIGN KEY (MASP)
    REFERENCES SANPHAM (MASP);
GO

ALTER TABLE GIAOHANG ADD CONSTRAINT FK_GIAO_DON FOREIGN KEY (MADH)
    REFERENCES DONHANG (MADH);
GO

PRINT N'Tao CSDL QL_VatTuNongNghiep thanh cong.';
GO