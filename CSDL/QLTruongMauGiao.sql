USE master
GO
CREATE DATABASE QLTruongMauGiao
GO
USE QLTruongMauGiao
GO
CREATE TABLE LOP
(
	MaLop varchar(5) PRIMARY KEY,
	TenLop nvarchar(50),
	SiSo int NOT NULL,
	DoTuoi int NOT NULL,
	Khoi nvarchar(20)
)
GO
INSERT INTO LOP(MaLop,TenLop,SiSo,DoTuoi,Khoi)
VALUES ('L0001', N'Mầm 1',2, 3, N'Mầm'),
		('L0002', N'Mầm 2', 2, 3, N'Mầm'),
		('L0003', N'Chồi 1', 2, 4, N'Chồi'),
		('L0004', N'Chồi 2', 1, 4, N'Chồi'),
		('L0005', N'Lá 1', 8, 5, N'Lá'),
		('L0006', N'Lá 2', 1, 5, N'Lá')
GO
CREATE TABLE TAIKHOAN
(
	TenTK varchar(20) PRIMARY KEY,
	MatKhau varchar(30) NOT NULL,
	PhanQuyen nvarchar(30) NOT NULL,
	TrangThaiHD bit NOT NULL,
	AnhDaiDien varchar(100)
)
GO
INSERT INTO TAIKHOAN(TenTK,MatKhau,PhanQuyen,TrangThaiHD,AnhDaiDien)
VALUES ('Admin', '123456', N'Quản lý', 0, 'default.png'),
		('gv001', '123456', N'Giáo viên', 0, 'default.png'),
		('gv002', '123456', N'Giáo viên', 0, 'default.png'),
		('gv003', '123456', N'Giáo viên', 0, 'default.png'),
		('gv004', '123456', N'Giáo viên', 0, 'default.png'),
		('gv005', '123456', N'Giáo viên', 0, 'default.png'),
		('gv006', '123456', N'Giáo viên', 0, 'default.png'),
		('gv007', '123456', N'Giáo viên', 0, 'default.png'),
		('gv008', '123456', N'Giáo viên', 0, 'default.png'),
		('gv009', '123456', N'Giáo viên', 0, 'default.png'),
		('gv010', '123456', N'Giáo viên', 0, 'default.png'),
		('gv011', '123456', N'Giáo viên', 0, 'default.png'),
		('gv012', '123456', N'Giáo viên', 0, 'default.png'),
		('ph001', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph002', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph003', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph004', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph005', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph006', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph007', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph008', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph009', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph010', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph011', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph012', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph013', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph014', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph015', '123456', N'Phụ huynh', 0, 'default.png'),
		('ph016', '123456', N'Phụ huynh', 0, 'default.png')
GO
CREATE TABLE GIAOVIEN
(
	MaGV varchar(5) PRIMARY KEY,
	TenGV nvarchar(100),
	NgaySinh date NOT NULL,
	GioiTinh bit NOT NULL,
	QueQuan nvarchar(30),
	DienThoai varchar(15),
	Email varchar(100),
	LoaiHopDong nvarchar(30),
	KinhNghiem nvarchar(30),
	TrinhDo nvarchar(30),
	ChuyenNganh nvarchar(30),
	LoaiTotNghiep nvarchar(30),
	Anh varchar(100),
	TenTK varchar(20) NOT NULL UNIQUE, 
	CONSTRAINT fk_giaovien_taikhoan FOREIGN KEY(TenTK) REFERENCES TAIKHOAN(TenTK),
)
GO
INSERT INTO GIAOVIEN VALUES ('admin',N'Nguyễn Thu Huyền','1990/05/20',0,N'Hà Nội','0395231832','thuhuyen@gmail.com',N'Biên chế',N'5 năm',N'Cao đẳng',N'Mầm non',N'Giỏi','default.png','Admin')
INSERT INTO GIAOVIEN(MaGV,TenGV,NgaySinh,GioiTinh,QueQuan,DienThoai,Email,LoaiHopDong,KinhNghiem,TrinhDo,ChuyenNganh,LoaiTotNghiep,Anh,TenTK)
VALUES	('gv001',N'Nguyễn Thu Huyền','1990/05/20',0,N'Hà Nội','0395231832','thuhuyen@gmail.com',N'Biên chế',N'5 năm',N'Cao đẳng',N'Mầm non',N'Giỏi','default.png','gv001'),
		('gv002',N'Nguyễn Thị Hà','1991/07/10',0,N'Hà Nội','0395482752','NguyenHa@gmail.com',N'Biên chế',N'4 năm',N'Cao đẳng',N'Mầm non',N'Giỏi','default.png','gv002'),
		('gv003',N'Đặng Thị Thu Trang','1990/10/11',0,N'Hà Nội','0983059483','thuTrang@gmail.com',N'Biên chế',N'10 năm',N'Cao đẳng',N'Mầm non',N'Khá','default.png','gv003'),
		('gv004',N'Lê Thị Hà','1992/07/20',0,N'Hà Nội','09862871582','HaLe2007@gmail.com',N'Biên chế',N'3 năm',N'Cao đẳng',N'Mầm non',N'Giỏi','default.png','gv004'),
		('gv005',N'Đặng Thiên Kim','1993/08/20',0,N'Hà Nội','098628562182','DTK98@gmail.com',N'Biên chế',N'4 năm',N'Cao đẳng',N'Mầm non',N'Giỏi','default.png','gv005'),
		('gv006',N'Hoàng Ngọc Hạnh','1994/10/21',0,N'Hà Nội','09862621582','ngochanh@gmail.com',N'Biên chế',N'7 năm',N'Cao đẳng',N'Mầm non',N'Khá','default.png','gv006'),
		('gv007',N'Ngô Ngọc Diễm','1990/05/09',0,N'Hà Nội','09862871561','NND1998@gmail.com',N'Biên chế',N'10 năm',N'Cao đẳng',N'Mầm non',N'Khá','default.png','gv007'),
		('gv008',N'Liêu Anh Ðào','1991/04/20',0,N'Hà Nội','09862871121','DaoAnh@gmail.com',N'Biên chế',N'9 năm',N'Cao đẳng',N'Mầm non',N'Giỏi','default.png','gv008'),
		('gv009',N'Đoạn Bảo Trúc','1992/03/18',0,N'Hà Nội','09862871761','Baotruc@gmail.com',N'Biên chế',N'8 năm',N'Cao đẳng',N'Mầm non',N'Khá','default.png','gv009'),
		('gv010',N'Đinh Huệ Lan','1993/02/16',0,N'Hà Nội','09862871191','huelan@gmail.com',N'Biên chế',N'7 năm',N'Cao đẳng',N'Mầm non',N'Giỏi','default.png','gv010'),
		('gv011',N'Trương Minh Vy','1994/01/14',0,N'Hà Nội','09862201561','minhvi@gmail.com',N'Biên chế',N'6 năm',N'Cao đẳng',N'Mầm non',N'Khá','default.png','gv011'),
		('gv012',N'Phan Thạch Thảo','1995/12/12',0,N'Hà Nội','09820871561','thachthao@gmail.com',N'Biên chế',N'5 năm',N'Cao đẳng',N'Mầm non',N'Khá','default.png','gv012')
GO
CREATE TABLE PHANCONGGIAOVIEN
(
	MaPC varchar(5) PRIMARY KEY,
	MaGV varchar(5) NOT NULL,
	MaLop varchar(5) NOT NULL,
	NamHoc varchar(5) NOT NULL,
	CONSTRAINT fk_pcgv_gv FOREIGN KEY (MaGV) REFERENCES GIAOVIEN(MaGV),
	CONSTRAINT fk_pcgv_lop FOREIGN KEY (MaLop) REFERENCES LOP(MaLop)
)
GO
INSERT INTO PHANCONGGIAOVIEN(MaPC,MaGV,MaLop,NamHoc)
VALUES ('PC001','gv001','L0001','2021'),
		('PC002','gv002','L0001','2021'),
		('PC003','gv003','L0002','2021'),
		('PC004','gv004','L0002','2021'),
		('PC005','gv005','L0003','2021'),
		('PC006','gv006','L0003','2021'),
		('PC007','gv007','L0004','2021'),
		('PC008','gv008','L0004','2021'),
		('PC009','gv009','L0005','2021'),
		('PC010','gv010','L0005','2021')
GO
CREATE TABLE PHUHUYNH
(
	MaPH varchar(5) PRIMARY KEY,
	TenPH nvarchar(50),
	NamSinh date NOT NULL,
	GioiTinh bit NOT NULL,
	DiaChi nvarchar(100),
	DienThoai varchar(15),
	Email varchar(100),
	TenTK varchar(20) NOT NULL UNIQUE,
	CONSTRAINT fk_phuhuynh_taikhoan FOREIGN KEY(TenTK) REFERENCES TAIKHOAN(TenTK)
)
GO
INSERT INTO PHUHUYNH(MaPH,TenPH,NamSinh,GioiTinh,DiaChi,DienThoai,Email,TenTK)
VALUES ('ph001',N'Đặng Văn Sơn','1980/03/1',1,N'Hà Nội', '0391865281','','ph001'),
		('ph002',N'Đặng Văn Tùng','1981/04/12',1,N'Hà Nội', '0391265281','','ph002'),
		('ph003',N'Đặng Văn Thắng','1982/05/13',1,N'Hà Nội', '0391165281','','ph003'),
		('ph004',N'Đặng Văn Tú','1983/06/14',1,N'Hà Nội', '0391865121','','ph004'),
		('ph005',N'Đặng Văn Bá','1984/07/15',1,N'Hà Nội', '03918657211','','ph005'),
		('ph006',N'Đặng Văn Hạnh','1985/09/16',1,N'Hà Nội', '0391862281','','ph006'),
		('ph007',N'Đặng Văn Tài','1986/08/17',1,N'Hà Nội', '0391812781','','ph007'),
		('ph008',N'Đặng Văn Toàn','1987/12/18',1,N'Hà Nội', '0391261281','','ph008'),
		('ph009',N'Đặng Văn Quang','1988/11/19',1,N'Hà Nội', '0391861271','','ph009'),
		('ph010',N'Nguyễn Quang Đại','1988/07/20',1,N'Hà Nội', '0477349827','','ph010'),
		('ph011',N'Nguyễn Văn Tú','1980/06/01',1,N'Hà Nội', '0477349827','','ph011'),
		('ph012',N'Nguyễn Trung Phú','1980/07/10',1,N'Hà Nội', '0477349827','','ph012'),
		('ph013',N'Nguyễn Thị Hoa','1982/07/11',1,N'Hà Nội', '0477349827','','ph013'),
		('ph014',N'Bùi Thị Xuân','1980/07/12',1,N'Hà Nội', '0477349827','','ph014'),
		('ph015',N'Trần Quang Đại','1983/07/15',1,N'Hà Nội', '0477349827','','ph015'),
		('ph016',N'Trần Trung Hưng','1987/07/20',1,N'Hà Nội', '0477349827','','ph016')
GO
CREATE TABLE THUCDONNGAY
(
	MaTDN varchar(10) PRIMARY KEY,
	Ngay date NOT NULL UNIQUE,
	BuaSang nvarchar(500),
	BuaTrua nvarchar(500),
	BuaXe nvarchar(500)
)
GO
INSERT INTO THUCDONNGAY(MaTDN,Ngay,BuaSang,BuaTrua,BuaXe)
VALUES ('TDN001','2021/09/06',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN002','2021/09/07',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN003','2021/09/08',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN004','2021/09/09',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN005','2021/09/10',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN006','2021/09/11',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN007','2021/09/12',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN008','2021/09/13',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN009','2021/09/14',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN010','2021/09/15',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN011','2021/09/16',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN012','2021/09/17',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN013','2021/09/18',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN014','2021/09/19',N'Bánh mì',N'Cơm',N'Sữa'),
		('TDN015','2021/09/20',N'Bánh mì',N'Cơm',N'Sữa')
GO
CREATE TABLE TRE
(
	MaTre varchar(6) PRIMARY KEY,
	MaLop varchar(5) NOT NULL,
	MaPH varchar(5) NOT NULL,
	TenTre nvarchar(100),
	NgaySinh date NOT NULL,
	GioiTinh bit NOT NULL,
	QueQuan nvarchar(50),
	DanToc nvarchar(30),
	NgayNhapHoc datetime NOT NULL,
	Anh varchar(100),
	CONSTRAINT fk_TRE_LOP FOREIGN KEY (MaLop) REFERENCES LOP(MaLop),
	CONSTRAINT fk_TRE_PH FOREIGN KEY (MaPH) REFERENCES PHUHUYNH(MaPH) 
)
GO
INSERT INTO TRE(MaTre,MaLop,MaPH,TenTre,NgaySinh,GioiTinh,QueQuan,DanToc,NgayNhapHoc,Anh)
VALUES ('TRE001','L0001','ph001',N'Nguyễn Văn Tuấn','2018/02/01',1,N'Hà Nội',N'Kinh','2021/01/10','default.png'),
		('TRE002','L0001','ph002',N'Nguyễn Văn Tài','2018/10/02',1,N'Hà Nội',N'Kinh','2021/01/10','default.png'),
		('TRE003','L0002','ph003',N'Trần Văn An','2018/08/03',1,N'Hà Nội',N'Kinh','2021/01/10','default.png'),
		('TRE004','L0002','ph004',N'Hoàng Văn Tỉnh','2018/06/04',1,N'Hà Nội',N'Kinh','2021/01/10','default.png'),
		('TRE005','L0003','ph005',N'Đinh Mạnh Tiến','2018/04/05',1,N'Hà Nội',N'Kinh','2021/01/10','default.png'),
		('TRE006','L0003','ph006',N'Nguyễn Thành Công','2018/03/07',1,N'Hà Nội',N'Kinh','2021/01/10','default.png'),
		('TRE007','L0004','ph007',N'Nguyễn Tiến Lên','2019/10/21',1,N'Hà Nội',N'Kinh','2021/01/10','default.png'),
		('TRE008','L0005','ph008',N'Nguyễn Văn Long','2019/11/22',1,N'Hà Nội',N'Kinh','2021/01/10','default.png'),
		('TRE009','L0006','ph009',N'Đặng Thị Thu Hà','2019/12/23',0,N'Hà Nội',N'Kinh','2021/01/10','default.png'),
		('TRE010','L0005','ph010',N'Nguyễn Hà Vi','2019/10/24',0,N'Hà Nội',N'Kinh','2020/01/10','default.png'),
		('TRE011','L0005','ph011',N'Đặng Thị Thảo','2019/09/11',0,N'Hà Nội',N'Kinh','2020/01/10','default.png'),
		('TRE012','L0005','ph012',N'Nguyễn Hoàng Mai','2019/08/25',0,N'Hà Nội',N'Kinh','2020/01/10','default.png'),
		('TRE013','L0005','ph013',N'Đặng Thanh Mai','2019/07/28',0,N'Hà Nội',N'Kinh','2020/01/10','default.png'),
		('TRE014','L0005','ph014',N'Nguyễn Thanh Mai','2017/11/15',0,N'Hà Nội',N'Kinh','2020/01/10','default.png'),
		('TRE015','L0005','ph015',N'Lê Thị Thắm','2017/12/17',0,N'Hà Nội',N'Kinh','2020/01/10','default.png'),
		('TRE016','L0005','ph016',N'Lê Thị Thơm','2017/06/13',0,N'Hà Nội',N'Kinh','2020/01/10','default.png')
GO
CREATE TABLE CHIPHI
(
	MaChiPhi varchar(5) PRIMARY KEY,
	NoiDung nvarchar(100) NOT NULL,
	DonGia money NOT NULL,
)
GO
INSERT INTO CHIPHI(MaChiPhi,NoiDung,DonGia)
VALUES ('CP001',N'Học phí',2000000),
		('CP002',N'Ăn',1000000),
		('CP003',N'Vật tư',500000)
GO
CREATE TABLE PHIEUTHUTIEN
(
	MaPhieu varchar(5) PRIMARY KEY,
	MaTre varchar(6) NOT NULL,
	NgayLapPhieu datetime NOT NULL,
	TrangThai bit NOT NULL,
	CONSTRAINT fk_phieu_tre FOREIGN KEY (MaTre) REFERENCES Tre(MaTre)
)
GO
CREATE TABLE DONGCHIPHI
(
	MaPhieu varchar(5) NOT NULL,
	MaChiPhi varchar(5) NOT NULL,
	SoLuong int NOT NULL,
	PRIMARY KEY (MaPhieu, MaChiPhi),
	CONSTRAINT fk_dong_phieu FOREIGN KEY (MaPhieu) REFERENCES PHIEUTHUTIEN(MaPhieu),
	CONSTRAINT fk_dong_chiphi FOREIGN KEY (MaChiPhi) REFERENCES CHIPHI(MaChiPhi),
)
GO
CREATE TABLE DiemDanhVaDangKyBuaAn
(
	ID varchar(5) PRIMARY KEY,
	MaTre varchar(6) NOT NULL,
	Ngay date NOT NULL,
	DiemDanh bit NOT NULL,
	DangKiBuaAn bit NOT NULL,
	CONSTRAINT fk_dd_tre FOREIGN KEY (MaTre) REFERENCES TRE(MaTre)
)
GO
CREATE TABLE PHIEUDANHGIA
(
	MaPhieu varchar(5) PRIMARY KEY,
	MaTre varchar(6) NOT NULL,
	MaGV varchar(5) NOT NULL,
	NgayTao date NOT NULL,
	NamHoc varchar(5),
	CONSTRAINT fk_phieudg_tre FOREIGN KEY (MaTre) REFERENCES TRE(MaTre),
	CONSTRAINT fk_kq_gb FOREIGN KEY (MaGV) REFERENCES GIAOVIEN(MaGV),
)
GO
CREATE TABLE NOIDUNGDANHGIA
(
	MaNDDG varchar(5) PRIMARY KEY,
	NoiDungDanhGia nvarchar(300) NOT NULL
)
GO
INSERT INTO NOIDUNGDANHGIA(MaNDDG,NoiDungDanhGia)
VALUES('ND001',N'Cân nặng theo tuổi.'),
		('ND002',N'Chiều cao theo tuổi.'),
		('ND003',N'Đi/chạy thay đổi tốc độ theo đúng hiệu lệnh.'),
		('ND004',N'Tung - bắt bóng với người đối diện (khoảng cách 2,5 m).'),
		('ND005',N'Chạy liên tục theo hướng thẳng 15m.'),
		('ND006',N'Cắt thẳng được một đoạn 10 cm.'),
		('ND007',N'Xếp chồng 8 - 10 khối.'),
		('ND008',N'Nói đúng tên một số thực phẩm quen thuộc khi nhìn thấy vật thật hoặc tranh ảnh.'),
		('ND009',N'Thực hiện được một số việc đơn giản với sự giúp đỡ của người lớn : rửa tay, lau mặt, súc miệng, tháo tất, cởi quần, áo....'),
		('ND010',N'Sử dụng bát, thìa , cốc đúng cách.'),
		('ND011',N'Biết nói với người lớn khi bị đau, chảy máu.'),
		('ND012',N'Phân loại các đối tượng theo một dấu hiệu nổi bật.'),
		('ND013',N'Đếm trên các đối tượng giống nhau và đếm đến 5.'),
		('ND014',N'So sánh số lượng hai nhóm đối tượng trong phạm vi 5; nói được các từ : bằng nhau, nhiều hơn, ít hơn.'),
		('ND015',N'So sánh hai đối tượng về kích thước và nói được các từ : to hơn/nhỏ hơn; dài hơn/ngắn hơn; cao hơn/thấp hơn; bằng nhau.'),
		('ND016',N'Nhận dạng và gọi tên các hình: tròn, vuông, tam giác, chữ nhật.'),
		('ND017',N'Mô tả những dấu hiệu nổi bật của đối tượng được quan sát với sự gợi mở của cô giáo.')
GO
CREATE TABLE KETQUADANHGIA
(
	MaPhieu varchar(5) NOT NULL,
	MaNDDG varchar(5) NOT NULL,
	KetQua bit NOT NULL,
	PRIMARY KEY (MaPhieu, MaNDDG),
	CONSTRAINT fk_kq_phieu FOREIGN KEY (MaPhieu) REFERENCES PHIEUDANHGIA(MaPhieu),
	CONSTRAINT fk_kq_nd FOREIGN KEY (MaNDDG) REFERENCES NOIDUNGDANHGIA(MaNDDG),
)
GO
CREATE TABLE THONGBAO
(
	MaTB varchar(5) PRIMARY KEY,
	TenTB nvarchar(200),
	NoiDung nvarchar(500),
	NgayTB datetime NOT NULL
)
GO
CREATE TABLE THONGBAO_TK
(
	MaTB varchar(5) NOT NULL,
	TenTK varchar(20) NOT NULL,
	DaXem bit NOT NULL,
	PRIMARY KEY(MaTB,TenTK),
	CONSTRAINT fk_thongbao_tk_taikhoan FOREIGN KEY (TenTK) REFERENCES TAIKHOAN(TenTK),
	CONSTRAINT fk_thongbao_tk_thongbao FOREIGN KEY (MaTB) REFERENCES THONGBAO(MaTB)
)