using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GopY",
                columns: table => new
                {
                    MaGy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaKh = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayGY = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DienThoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CanTraLoi = table.Column<bool>(type: "bit", nullable: false),
                    TraLoi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayTL = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GopY", x => x.MaGy);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HashMatKhau = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    DiaChi = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    DienThoai = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Hinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Photo.gif"),
                    HieuLuc = table.Column<bool>(type: "bit", nullable: false),
                    VaiTro = table.Column<int>(type: "int", nullable: false),
                    ResetCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    RandomKey = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.MaKH);
                });

            migrationBuilder.CreateTable(
                name: "Loai",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenLoaiAlias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.MaLoai);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    MaNCC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenCongTy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NguoiLienLac = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DienThoai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.MaNCC);
                });

            migrationBuilder.CreateTable(
                name: "PhanQuyen",
                columns: table => new
                {
                    MaPQ = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPq = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanQuyen", x => x.MaPQ);
                });

            migrationBuilder.CreateTable(
                name: "TrangThai",
                columns: table => new
                {
                    MaTrangThai = table.Column<int>(type: "int", nullable: false),
                    TenTrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThai", x => x.MaTrangThai);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    VoucherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VoucherCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiscountVoucher = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(GETUTCDATE())"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(GETUTCDATE())"),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(GETUTCDATE())"),
                    UserLimit = table.Column<int>(type: "int", nullable: true),
                    OrderMinimum = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.VoucherId);
                    table.CheckConstraint("CK_Voucher_DiscountVoucher", "[DiscountVoucher] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                    table.ForeignKey(
                        name: "FK_History_Customers",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HangHoa",
                columns: table => new
                {
                    MaHH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoLuong = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    TenAlias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaLoai = table.Column<int>(type: "int", nullable: false),
                    MoTaDonVi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DonGia = table.Column<double>(type: "float", nullable: true, defaultValue: 0.0),
                    Hinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgaySX = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    GiamGia = table.Column<double>(type: "float", nullable: false),
                    SoLanXem = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNCC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.MaHH);
                    table.ForeignKey(
                        name: "FK_Products_Categories",
                        column: x => x.MaLoai,
                        principalTable: "Loai",
                        principalColumn: "MaLoai",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers",
                        column: x => x.MaNCC,
                        principalTable: "NhaCungCap",
                        principalColumn: "MaNCC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    MaNV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HashMatKhau = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaPQ = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVien", x => x.MaNV);
                    table.ForeignKey(
                        name: "FK_NhanVien_PhanQuyen",
                        column: x => x.MaPQ,
                        principalTable: "PhanQuyen",
                        principalColumn: "MaPQ");
                });

            migrationBuilder.CreateTable(
                name: "VoucherUsers",
                columns: table => new
                {
                    VoucherUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VoucherId = table.Column<int>(type: "int", nullable: false),
                    AcquiredDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(GETUTCDATE())"),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UsedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherUser", x => x.VoucherUserId);
                    table.ForeignKey(
                        name: "FK_VoucherUser_User",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoucherUser_Voucher",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "VoucherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHH = table.Column<int>(type: "int", nullable: false),
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CommentDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_Comment_Customers",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_Products",
                        column: x => x.MaHH,
                        principalTable: "HangHoa",
                        principalColumn: "MaHH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInteractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaHH = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(GETUTCDATE())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInteraction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInteraction_Customer",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInteraction_Product",
                        column: x => x.MaHH,
                        principalTable: "HangHoa",
                        principalColumn: "MaHH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YeuThich",
                columns: table => new
                {
                    MaYT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHH = table.Column<int>(type: "int", nullable: true),
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NgayChon = table.Column<DateTime>(type: "datetime", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.MaYT);
                    table.ForeignKey(
                        name: "FK_Favorites_Customers",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YeuThich_HangHoa",
                        column: x => x.MaHH,
                        principalTable: "HangHoa",
                        principalColumn: "MaHH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    MaHD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKH = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    NgayCan = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    NgayGiao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(((1)/(1))/(1900))"),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DienThoai = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CachThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Cash"),
                    CachVanChuyen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Airline"),
                    PhiVanChuyen = table.Column<double>(type: "float", nullable: false),
                    MaTrangThai = table.Column<int>(type: "int", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    MaNV = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.MaHD);
                    table.ForeignKey(
                        name: "FK_HoaDon_NhanVien",
                        column: x => x.MaNV,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDon_TrangThai",
                        column: x => x.MaTrangThai,
                        principalTable: "TrangThai",
                        principalColumn: "MaTrangThai");
                    table.ForeignKey(
                        name: "FK_Orders_Customers",
                        column: x => x.MaKH,
                        principalTable: "KhachHang",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHD",
                columns: table => new
                {
                    MaCT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHD = table.Column<int>(type: "int", nullable: false),
                    MaHH = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<double>(type: "float", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    GiamGia = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.MaCT);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders",
                        column: x => x.MaHD,
                        principalTable: "HoaDon",
                        principalColumn: "MaHD");
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products",
                        column: x => x.MaHH,
                        principalTable: "HangHoa",
                        principalColumn: "MaHH");
                });

            migrationBuilder.CreateTable(
                name: "InvoiceVouchers",
                columns: table => new
                {
                    InvoiceVoucherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHd = table.Column<int>(type: "int", nullable: false),
                    VoucherUserId = table.Column<int>(type: "int", nullable: true),
                    AppliedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(GETUTCDATE())"),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceVoucher", x => x.InvoiceVoucherId);
                    table.ForeignKey(
                        name: "FK_InvoiceVoucher_Invoice",
                        column: x => x.MaHd,
                        principalTable: "HoaDon",
                        principalColumn: "MaHD",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceVoucher_VoucherUser",
                        column: x => x.VoucherUserId,
                        principalTable: "VoucherUsers",
                        principalColumn: "VoucherUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHD_MaHD",
                table: "ChiTietHD",
                column: "MaHD");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHD_MaHH",
                table: "ChiTietHD",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MaHH",
                table: "Comments",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MaKH",
                table: "Comments",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_HangHoa_MaLoai",
                table: "HangHoa",
                column: "MaLoai");

            migrationBuilder.CreateIndex(
                name: "IX_HangHoa_MaNCC",
                table: "HangHoa",
                column: "MaNCC");

            migrationBuilder.CreateIndex(
                name: "IX_History_MaKH",
                table: "History",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaKH",
                table: "HoaDon",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaNV",
                table: "HoaDon",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_MaTrangThai",
                table: "HoaDon",
                column: "MaTrangThai");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceVouchers_MaHd",
                table: "InvoiceVouchers",
                column: "MaHd");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceVouchers_VoucherUserId",
                table: "InvoiceVouchers",
                column: "VoucherUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_MaPQ",
                table: "NhanVien",
                column: "MaPQ");

            migrationBuilder.CreateIndex(
                name: "IX_UserInteractions_MaHH",
                table: "UserInteractions",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_UserInteractions_MaKH",
                table: "UserInteractions",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherUsers_MaKH",
                table: "VoucherUsers",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherUsers_VoucherId",
                table: "VoucherUsers",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThich_MaHH",
                table: "YeuThich",
                column: "MaHH");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThich_MaKH",
                table: "YeuThich",
                column: "MaKH");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHD");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "GopY");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "InvoiceVouchers");

            migrationBuilder.DropTable(
                name: "UserInteractions");

            migrationBuilder.DropTable(
                name: "YeuThich");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "VoucherUsers");

            migrationBuilder.DropTable(
                name: "HangHoa");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "TrangThai");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "Loai");

            migrationBuilder.DropTable(
                name: "NhaCungCap");

            migrationBuilder.DropTable(
                name: "PhanQuyen");
        }
    }
}
