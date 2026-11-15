# Website quản lý cửa hàng vật tư nông nghiệp (phân bón)

Đồ án chuyên ngành – Đại học Trà Vinh. Xây dựng bằng **ASP.NET Core MVC (.NET 9)**, **SQL Server**, **Entity Framework Core** (Database First).

## Tiến độ 2 (09/11 – 15/11) – đã hoàn thành

- Khung dự án MVC, solution `.sln` mở bằng Visual Studio.
- Hai giao diện riêng: người dùng (`_Layout`) và quản trị (`Area Admin` – `_AdminLayout`).
- Đăng nhập / đăng xuất / đăng ký khách hàng; phân quyền **Admin** – **KhachHang** (Cookie Authentication, mật khẩu băm bằng `PasswordHasher`).
- Quản lý danh mục (Thêm / Sửa / Xóa / Tìm kiếm không dấu / Phân trang): **Nhóm sản phẩm, Thương hiệu, Thành phần, Công dụng**.
  Mã tự sinh không trùng (NH001, TH001, TP001, CD001…), báo lỗi dễ hiểu khi xóa danh mục đang được sản phẩm sử dụng.

## Yêu cầu

| Phần mềm | Phiên bản |
|---|---|
| Visual Studio 2022 (workload *ASP.NET and web development*) | 17.12 trở lên (hỗ trợ .NET 9) |
| .NET SDK | 9.0 |
| SQL Server + SSMS | 2019 trở lên (bản Express cũng được) |

## Cách chạy

**1. Tạo cơ sở dữ liệu (trong SSMS, chạy lần lượt):**

1. `Database/QL_VatTuNongNghiep.sql` – tạo database `QL_VatTuNongNghiep` và 19 bảng.
2. `Database/DuLieuMau.sql` – dữ liệu mẫu cho các danh mục (chạy lại nhiều lần vẫn an toàn).

**2. Kiểm tra chuỗi kết nối** trong `VatTuNongNghiep/appsettings.json`:

```json
"DefaultConnection": "Server=.;Database=QL_VatTuNongNghiep;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

`Server=.` là SQL Server mặc định trên máy. Nếu máy dùng tên khác, thay bằng đúng ô **Server name** khi đăng nhập SSMS, ví dụ:

- SQL Server Express: `Server=.\\SQLEXPRESS;`
- LocalDB: `Server=(localdb)\\MSSQLLocalDB;`

(Trong file JSON, dấu `\` phải viết thành `\\`.)

**3. Chạy chương trình:** mở `VatTuNongNghiep.sln` bằng Visual Studio → nhấn **F5**.
Hoặc dùng dòng lệnh: `dotnet run --project VatTuNongNghiep` rồi mở http://localhost:5050.

Khi khởi động, cửa sổ console in dòng **"Kết nối database QL_VatTuNongNghiep thành công."** và tự tạo tài khoản Admin nếu chưa có.

## Tài khoản dùng thử

| Vai trò | Tên đăng nhập | Mật khẩu |
|---|---|---|
| Quản trị | `admin` | `Admin@123` |
| Khách hàng | bấm **Đăng ký** trên trang web để tạo | |

Trang Đăng nhập có ô **"Tài khoản dùng thử"** với nút *Điền sẵn* (chỉ hiện khi chạy ở chế độ Development).

## Các chức năng nên thử

1. Vào `/Admin` khi chưa đăng nhập → bị chuyển về trang Đăng nhập.
2. Đăng nhập `admin` → trang Tổng quan → menu trái: Nhóm sản phẩm, Thương hiệu, Thành phần, Công dụng.
3. Mỗi danh mục: tìm kiếm (gõ không dấu, vd. `phan dam`), thêm (mã tự sinh), sửa, xóa; thêm tên trùng → báo lỗi.
4. Đăng ký một khách hàng → đăng nhập bằng khách hàng → vào `/Admin` → trang **403 – Không có quyền truy cập**.

## Cấu trúc thư mục

```
├── VatTuNongNghiep.sln
├── Database/                     Script tạo CSDL + dữ liệu mẫu
└── VatTuNongNghiep/              Project ASP.NET Core MVC
    ├── Areas/Admin/              Phần quản trị (Controllers, Models, Views, _AdminLayout)
    ├── Controllers/              Phần người dùng (Home, Account)
    ├── Helpers/                  Hằng số, sinh mã tự động, tạo tài khoản Admin
    ├── Models/                   Class sinh từ database (scaffold) + ViewModels
    ├── Views/                    Giao diện người dùng (_Layout)
    ├── wwwroot/                  CSS, JS, Bootstrap
    └── Program.cs, appsettings.json
```
