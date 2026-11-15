/* =====================================================================
   DỮ LIỆU MẪU – QL_VatTuNongNghiep (Tiến độ 2)
   Bảng: NHOMSANPHAM, THUONGHIEU, THANHPHAN, CONGDUNG, NHOMCAYTRONG
   - Chưa có SANPHAM (làm ở Tiến độ 3).
   - Chạy lại nhiều lần vẫn an toàn: dòng nào đã có mã thì bỏ qua (NOT EXISTS).
   - Quy ước mã: NH = nhóm SP, TH = thương hiệu, TP = thành phần,
                 CD = công dụng, NC = nhóm cây trồng.
   ===================================================================== */
USE QL_VatTuNongNghiep;
GO

SET XACT_ABORT ON;   -- có lỗi thì hủy toàn bộ giao dịch
BEGIN TRANSACTION;

/* ---------- 1. NHÓM SẢN PHẨM ---------- */
INSERT INTO NHOMSANPHAM (MANHOM, TENNHOM, MOTA)
SELECT v.MANHOM, v.TENNHOM, v.MOTA
FROM (VALUES
    ('NH001', N'Phân đạm',            N'Cung cấp đạm (N) giúp cây phát triển thân lá, ví dụ: Urê, SA.'),
    ('NH002', N'Phân lân',            N'Cung cấp lân (P2O5) giúp cây ra rễ, ra hoa, ví dụ: lân nung chảy, super lân.'),
    ('NH003', N'Phân kali',           N'Cung cấp kali (K2O) giúp cứng cây, nuôi trái, tăng chất lượng nông sản.'),
    ('NH004', N'Phân NPK',            N'Phân hỗn hợp chứa cả đạm, lân, kali theo nhiều tỷ lệ khác nhau.'),
    ('NH005', N'Phân hữu cơ',         N'Phân có nguồn gốc hữu cơ, giúp cải tạo đất, tăng độ tơi xốp.'),
    ('NH006', N'Phân vi sinh',        N'Chứa vi sinh vật có ích giúp phân giải dinh dưỡng, cải tạo đất.'),
    ('NH007', N'Phân bón lá',         N'Phân hòa tan phun lên lá, cây hấp thu nhanh.'),
    ('NH008', N'Phân trung vi lượng', N'Bổ sung canxi, magie, kẽm, bo... giúp cây khỏe, hạn chế rụng trái.')
) AS v (MANHOM, TENNHOM, MOTA)
WHERE NOT EXISTS (SELECT 1 FROM NHOMSANPHAM t WHERE t.MANHOM = v.MANHOM);

/* ---------- 2. THƯƠNG HIỆU ---------- */
INSERT INTO THUONGHIEU (MATHUONGHIEU, TENTHUONGHIEU, QUOCGIA, DIACHI)
SELECT v.MATHUONGHIEU, v.TENTHUONGHIEU, v.QUOCGIA, v.DIACHI
FROM (VALUES
    ('TH001', N'Đạm Phú Mỹ',              N'Việt Nam', N'TP. Hồ Chí Minh'),
    ('TH002', N'Đạm Cà Mau',              N'Việt Nam', N'Tỉnh Cà Mau'),
    ('TH003', N'Bình Điền (Đầu Trâu)',    N'Việt Nam', N'TP. Hồ Chí Minh'),
    ('TH004', N'Phân bón Miền Nam',       N'Việt Nam', N'TP. Hồ Chí Minh'),
    ('TH005', N'Lâm Thao',                N'Việt Nam', N'Tỉnh Phú Thọ'),
    ('TH006', N'Văn Điển',                N'Việt Nam', N'TP. Hà Nội'),
    ('TH007', N'Baconco',                 N'Việt Nam', N'TP. Hồ Chí Minh'),
    ('TH008', N'Yara',                    N'Na Uy',    N'Oslo, Na Uy')
) AS v (MATHUONGHIEU, TENTHUONGHIEU, QUOCGIA, DIACHI)
WHERE NOT EXISTS (SELECT 1 FROM THUONGHIEU t WHERE t.MATHUONGHIEU = v.MATHUONGHIEU);

/* ---------- 3. THÀNH PHẦN ---------- */
-- LOAITHANHPHAN: Đa lượng / Trung lượng / Vi lượng / Hữu cơ
INSERT INTO THANHPHAN (MATHANHPHAN, TENTHANHPHAN, DONVIHAMLUONG, LOAITHANHPHAN)
SELECT v.MATHANHPHAN, v.TENTHANHPHAN, v.DONVIHAMLUONG, v.LOAITHANHPHAN
FROM (VALUES
    ('TP001', N'Đạm (N)',          N'%',   N'Đa lượng'),
    ('TP002', N'Lân (P2O5)',       N'%',   N'Đa lượng'),
    ('TP003', N'Kali (K2O)',       N'%',   N'Đa lượng'),
    ('TP004', N'Canxi (CaO)',      N'%',   N'Trung lượng'),
    ('TP005', N'Magie (MgO)',      N'%',   N'Trung lượng'),
    ('TP006', N'Kẽm (Zn)',         N'ppm', N'Vi lượng'),
    ('TP007', N'Bo (B)',           N'ppm', N'Vi lượng'),
    ('TP008', N'Chất hữu cơ',      N'%',   N'Hữu cơ')
) AS v (MATHANHPHAN, TENTHANHPHAN, DONVIHAMLUONG, LOAITHANHPHAN)
WHERE NOT EXISTS (SELECT 1 FROM THANHPHAN t WHERE t.MATHANHPHAN = v.MATHANHPHAN);

/* ---------- 4. CÔNG DỤNG ---------- */
INSERT INTO CONGDUNG (MACONGDUNG, TENCONGDUNG, MOTA)
SELECT v.MACONGDUNG, v.TENCONGDUNG, v.MOTA
FROM (VALUES
    ('CD001', N'Kích thích ra rễ',            N'Giúp cây ra rễ mới, phục hồi sau trồng hoặc sau ngập úng.'),
    ('CD002', N'Phát triển thân lá',          N'Giúp cây xanh lá, đẻ nhánh, sinh trưởng mạnh giai đoạn đầu.'),
    ('CD003', N'Kích thích ra hoa',           N'Hỗ trợ phân hóa mầm hoa, ra hoa đồng loạt.'),
    ('CD004', N'Chống rụng hoa, rụng trái',   N'Tăng khả năng đậu trái, hạn chế rụng hoa và trái non.'),
    ('CD005', N'Nuôi trái lớn',               N'Giúp trái lớn nhanh, chắc hạt, tăng năng suất.'),
    ('CD006', N'Tăng chất lượng nông sản',    N'Cải thiện màu sắc, độ ngọt, mẫu mã sản phẩm.'),
    ('CD007', N'Cải tạo đất',                 N'Tăng độ tơi xốp, giữ ẩm, cân bằng độ pH của đất.'),
    ('CD008', N'Tăng sức đề kháng',           N'Giúp cây cứng cáp, chống chịu sâu bệnh và thời tiết bất lợi.')
) AS v (MACONGDUNG, TENCONGDUNG, MOTA)
WHERE NOT EXISTS (SELECT 1 FROM CONGDUNG t WHERE t.MACONGDUNG = v.MACONGDUNG);

/* ---------- 5. NHÓM CÂY TRỒNG ---------- */
INSERT INTO NHOMCAYTRONG (MANHOMCAY, TENNHOMCAY, MOTA)
SELECT v.MANHOMCAY, v.TENNHOMCAY, v.MOTA
FROM (VALUES
    ('NC001', N'Cây lúa',                       N'Lúa nước các vụ Đông Xuân, Hè Thu, Thu Đông.'),
    ('NC002', N'Cây ăn trái',                   N'Sầu riêng, xoài, bưởi, cam, chôm chôm...'),
    ('NC003', N'Cây công nghiệp lâu năm',       N'Cà phê, hồ tiêu, điều, cao su...'),
    ('NC004', N'Cây công nghiệp ngắn ngày',     N'Mía, bắp, khoai mì, đậu phộng...'),
    ('NC005', N'Rau màu',                       N'Rau ăn lá, rau ăn quả, dưa leo, ớt, cà chua...'),
    ('NC006', N'Cây dừa',                       N'Dừa ta, dừa xiêm, dừa sáp – cây chủ lực ở Trà Vinh, Bến Tre.'),
    ('NC007', N'Hoa kiểng',                     N'Mai, cúc, lan, hoa hồng và cây cảnh.')
) AS v (MANHOMCAY, TENNHOMCAY, MOTA)
WHERE NOT EXISTS (SELECT 1 FROM NHOMCAYTRONG t WHERE t.MANHOMCAY = v.MANHOMCAY);

COMMIT TRANSACTION;
GO

/* ---------- Kiểm tra số dòng mỗi bảng ---------- */
SELECT N'NHOMSANPHAM'  AS Bang, COUNT(*) AS SoDong FROM NHOMSANPHAM
UNION ALL SELECT N'THUONGHIEU',   COUNT(*) FROM THUONGHIEU
UNION ALL SELECT N'THANHPHAN',    COUNT(*) FROM THANHPHAN
UNION ALL SELECT N'CONGDUNG',     COUNT(*) FROM CONGDUNG
UNION ALL SELECT N'NHOMCAYTRONG', COUNT(*) FROM NHOMCAYTRONG;
GO

/* =====================================================================
   (TÙY CHỌN) XÓA DỮ LIỆU MẪU trước khi nhập dữ liệu thật.
   Bỏ dấu chú thích rồi chạy riêng đoạn này. Chỉ xóa được những dòng
   CHƯA được sản phẩm nào tham chiếu (SQL Server sẽ báo lỗi khóa ngoại).
   =====================================================================
DELETE FROM NHOMCAYTRONG WHERE MANHOMCAY    IN ('NC001','NC002','NC003','NC004','NC005','NC006','NC007');
DELETE FROM CONGDUNG     WHERE MACONGDUNG   IN ('CD001','CD002','CD003','CD004','CD005','CD006','CD007','CD008');
DELETE FROM THANHPHAN    WHERE MATHANHPHAN  IN ('TP001','TP002','TP003','TP004','TP005','TP006','TP007','TP008');
DELETE FROM THUONGHIEU   WHERE MATHUONGHIEU IN ('TH001','TH002','TH003','TH004','TH005','TH006','TH007','TH008');
DELETE FROM NHOMSANPHAM  WHERE MANHOM       IN ('NH001','NH002','NH003','NH004','NH005','NH006','NH007','NH008');
*/
