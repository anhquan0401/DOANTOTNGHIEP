using System;
using System.Collections.Generic;

namespace WebApplication1.Data;

public partial class KhachHang
{
    public string MaKh { get; set; } = null!;

    public string? HashMatKhau { get; set; }

    public string HoTen { get; set; } = null!;

    public bool GioiTinh { get; set; }

    public DateTime NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public string Email { get; set; } = null!;

    public string? Hinh { get; set; }

    public bool HieuLuc { get; set; }

    public int VaiTro { get; set; }

    public string? ResetCode { get; set; }

    public string? RandomKey { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();

    public virtual ICollection<Comments> Commentss { get; set; } = new List<Comments>();

    public virtual ICollection<History> Histories { get; set; } = new List<History>();

    public virtual ICollection<VoucherUser> VoucherUsers { get; set; } = new List<VoucherUser>();
    public virtual ICollection<UserInteraction> UserInteractions { get; set; } = new List<UserInteraction>();


}
