using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data;

public partial class EcommerceContext : DbContext
{
    public EcommerceContext()
    {
    }

    public EcommerceContext(DbContextOptions<EcommerceContext> options): base(options)
    {
    }

    public virtual DbSet<ChiTietHd> ChiTietHds { get; set; }

    public virtual DbSet<Comments> Commentss { get; set; }

    public virtual DbSet<GopY> Gopies { get; set; }

    public virtual DbSet<HangHoa> HangHoas { get; set; }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<Loai> Loais { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

    public virtual DbSet<YeuThich> YeuThiches { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<VoucherUser> VoucherUsers { get; set; }

    public virtual DbSet<InvoiceVoucher> InvoiceVouchers { get; set; }

    public virtual DbSet<UserInteraction> UserInteractions { get; set; }



    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=Ecommerce;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<ChiTietHd>(entity =>
        {
            entity.HasKey(e => e.MaCt).HasName("PK_OrderDetails");

            entity.ToTable("ChiTietHD");

            entity.Property(e => e.MaCt).HasColumnName("MaCT");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.SoLuong).HasDefaultValue(1);

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.ChiTietHds)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Orders");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.ChiTietHds)
                .HasForeignKey(d => d.MaHh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetails_Products");
        });

        modelBuilder.Entity<Comments>(entity =>
        {
            entity.HasKey(e => e.CommentID).HasName("PK_Comments");

            entity.ToTable("Comments");

            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.CommentDescription)
                .HasMaxLength(250)
                .HasColumnName("CommentDescription");
            entity.Property(e => e.Rating).HasColumnName("Rating");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Commentss)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_Comment_Customers");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.Commentss)
                .HasForeignKey(d => d.MaHh)
                .HasConstraintName("FK_Comment_Products");
        });

        modelBuilder.Entity<GopY>(entity =>
        {
            entity.HasKey(e => e.MaGy);

            entity.ToTable("GopY");

            entity.Property(e => e.MaGy)
                 .ValueGeneratedOnAdd()
                 .HasColumnType("uniqueidentifier");

            entity.Property(e => e.DienThoai).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaKh).HasMaxLength(20);
            entity.Property(e => e.NgayGy)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("NgayGY");
            entity.Property(e => e.NgayTl).HasColumnName("NgayTL");
            entity.Property(e => e.TraLoi).HasMaxLength(50);
        });

        modelBuilder.Entity<HangHoa>(entity =>
        {
            entity.HasKey(e => e.MaHh).HasName("PK_Products");

            entity.ToTable("HangHoa");

            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.SoLuong)
            .HasMaxLength(1000)
            .HasColumnName("SoLuong");
            entity.Property(e => e.DonGia).HasDefaultValue(0.0);
            entity.Property(e => e.Hinh).HasMaxLength(50);
            entity.Property(e => e.MaNcc)
                .HasMaxLength(50)
                .HasColumnName("MaNCC");
            entity.Property(e => e.MoTaDonVi).HasMaxLength(50);
            entity.Property(e => e.NgaySx)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NgaySX");
            entity.Property(e => e.TenAlias).HasMaxLength(50);
            entity.Property(e => e.TenHh)
                .HasMaxLength(200)
                .HasColumnName("TenHH");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaLoai)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.HangHoas)
                .HasForeignKey(d => d.MaNcc)
                .HasConstraintName("FK_Products_Suppliers");
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_History");

            entity.ToTable("History");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.Keyword)
                .HasColumnName("Keyword")
                .HasMaxLength(200);
            entity.Property(e => e.Timestamp)
                .HasColumnName("Timestamp")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Histories)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_History_Customers");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK_Orders");

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.DienThoai).HasMaxLength(24);
            entity.Property(e => e.CachThanhToan)
                .HasMaxLength(50)
                .HasDefaultValue("Cash");
            entity.Property(e => e.CachVanChuyen)
                .HasMaxLength(50)
                .HasDefaultValue("Airline");
            entity.Property(e => e.DiaChi).HasMaxLength(60);
            entity.Property(e => e.GhiChu).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.DienThoai).HasMaxLength(24);
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");
            entity.Property(e => e.NgayCan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayCapNhat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayGiao)
                .HasDefaultValueSql("(((1)/(1))/(1900))")
                .HasColumnType("datetime");
            entity.Property(e => e.MaThamChieu)
                .HasMaxLength(50);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HoaDon_NhanVien");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaTrangThai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDon_TrangThai");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK_Customers");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.DiaChi).HasMaxLength(60);
            entity.Property(e => e.DienThoai).HasMaxLength(24);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Hinh)
                .HasMaxLength(50)
                .HasDefaultValue("Photo.gif");
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.HashMatKhau).HasMaxLength(200);
            entity.Property(e => e.NgaySinh)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ResetCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RandomKey)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Loai>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK_Categories");

            entity.ToTable("Loai");

            entity.Property(e => e.Hinh).HasMaxLength(50);
            entity.Property(e => e.TenLoai).HasMaxLength(50);
            entity.Property(e => e.TenLoaiAlias).HasMaxLength(50);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK_Suppliers");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNcc)
                .HasMaxLength(50)
                .HasColumnName("MaNCC");
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.DienThoai).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Logo).HasMaxLength(50);
            entity.Property(e => e.NguoiLienLac).HasMaxLength(50);
            entity.Property(e => e.TenCongTy).HasMaxLength(50);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv);

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv)
                .HasMaxLength(50)
                .HasColumnName("MaNV");

            entity.Property(e => e.HoTen)
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .HasMaxLength(50);

            entity.Property(e => e.HashMatKhau)
                .HasMaxLength(200);    // nên để >= 200 vì hash thường dài

            entity.Property(e => e.MaPq)
                .HasColumnName("MaPQ");

            entity.Property(e => e.XacNhan)
            .IsRequired();

            entity.HasOne(d => d.MaPqNavigation)
                .WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaPq)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NhanVien_PhanQuyen");
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.MaPq);

            entity.ToTable("PhanQuyen");

            entity.Property(e => e.MaPq)
                .HasColumnName("MaPQ");

            entity.Property(e => e.TenPq)
                .HasMaxLength(50);
        });


        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai);

            entity.ToTable("TrangThai");

            entity.Property(e => e.MaTrangThai).ValueGeneratedNever();
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenTrangThai).HasMaxLength(50);
        });


        modelBuilder.Entity<YeuThich>(entity =>
        {
            entity.HasKey(e => e.MaYt).HasName("PK_Favorites");

            entity.ToTable("YeuThich");

            entity.Property(e => e.MaYt).HasColumnName("MaYT");
            entity.Property(e => e.MaHh).HasColumnName("MaHH");
            entity.Property(e => e.MaKh)
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MoTa).HasMaxLength(255);
            entity.Property(e => e.NgayChon).HasColumnType("datetime");

            entity.HasOne(d => d.MaHhNavigation).WithMany(p => p.YeuThiches)
                .HasForeignKey(d => d.MaHh)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_YeuThich_HangHoa");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.YeuThiches)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Favorites_Customers");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK_Voucher");

            entity.Property(e => e.VoucherName)
            .IsRequired()
            .HasMaxLength(100);
            entity.Property(e => e.VoucherCode)
            .IsRequired()
            .HasMaxLength(50);
            entity.Property(e => e.DiscountVoucher)
            .IsRequired();
            entity.HasCheckConstraint("CK_Voucher_DiscountVoucher", "[DiscountVoucher] >= 0");
            entity.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(false);
            entity.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("(GETUTCDATE())");
            entity.Property(e => e.UpdatedDate)
            .HasDefaultValueSql("(GETUTCDATE())");
            entity.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(250);
            entity.Property(e => e.ExpirationDate)
            .IsRequired()
            .HasDefaultValueSql("(GETUTCDATE())");
            entity.Property(e => e.UserLimit);
            entity.Property(e => e.OrderMinimum)
            .HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<VoucherUser>(entity => 
        {
            entity.HasKey(e => e.VoucherUserId).HasName("PK_VoucherUser");

            entity.Property(e => e.AcquiredDate)
            .IsRequired()
            .HasDefaultValueSql("(GETUTCDATE())");
            entity.Property(e => e.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);
            entity.Property(e => e.MaKh)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("MaKH");
            entity.Property(e => e.VoucherId)
            .IsRequired();
            entity.Property(e => e.UsedDate);


            entity.HasOne(e => e.MaKhNavigation).WithMany(u => u.VoucherUsers)
            .HasForeignKey(e => e.MaKh)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_VoucherUser_User");

            entity.HasOne(e => e.VoucherIdNavigation).WithMany(v => v.VoucherUsers)
            .HasForeignKey(e => e.VoucherId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_VoucherUser_Voucher");
        });

        modelBuilder.Entity<InvoiceVoucher>(entity =>
        {
            entity.HasKey(e => e.InvoiceVoucherId).HasName("PK_InvoiceVoucher");

            entity.Property(e => e.MaHd)
                .IsRequired();

            entity.Property(e => e.VoucherUserId);

            entity.Property(e => e.AppliedDate)
                .IsRequired()
                .HasDefaultValueSql("(GETUTCDATE())");

            entity.Property(e => e.DiscountAmount)
                .IsRequired()
                .HasDefaultValue(0.0)
                .HasColumnType("decimal(18, 2)");

            // Cấu hình quan hệ với Invoice
            entity.HasOne(iv => iv.MaHdNavigation)
                .WithMany(i => i.InvoiceVouchers)
                .HasForeignKey(iv => iv.MaHd)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InvoiceVoucher_Invoice");

            // Cấu hình quan hệ với VoucherUser
            entity.HasOne(iv => iv.VoucherUserIdNavigation)
                .WithMany(i => i.InvoiceVouchers)
                .HasForeignKey(iv => iv.VoucherUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InvoiceVoucher_VoucherUser");
        });

        modelBuilder.Entity<UserInteraction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserInteraction");
            entity.Property(e => e.MaKh)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("MaKH");
            entity.Property(e => e.MaHh)
                .IsRequired()
                .HasColumnName("MaHH");
            entity.Property(e => e.Count)
                .IsRequired()
                .HasDefaultValue(0);
            entity.Property(e => e.Timestamp)
                .IsRequired()
                .HasDefaultValueSql("(GETUTCDATE())")
                .HasColumnType("datetime");
            entity.HasOne(e => e.MaKhNavigation).WithMany(u => u.UserInteractions)
                .HasForeignKey(e => e.MaKh)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserInteraction_Customer");
            entity.HasOne(e => e.MaHhNavigation).WithMany(p => p.UserInteractions)
                .HasForeignKey(e => e.MaHh)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserInteraction_Product");
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
