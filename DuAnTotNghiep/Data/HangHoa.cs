using System;
using System.Collections.Generic;
using WebApplication1.ViewModels;

namespace WebApplication1.Data;

public partial class HangHoa
{
    public int MaHh { get; set; }
    public string TenHh { get; set; } = null!;
    public int SoLuong { get; set; }

    public string? TenAlias { get; set; }

    public int MaLoai { get; set; }

    public string? MoTaDonVi { get; set; }

    public double? DonGia { get; set; }

    public string? Hinh { get; set; }

    public DateTime NgaySx { get; set; }

    public double GiamGia { get; set; }

    public int SoLanXem { get; set; }

    public string? MoTa { get; set; }

    public string MaNcc { get; set; } = null!;

    public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();

    public virtual ICollection<Comments> Commentss { get; set; } = new List<Comments>();

    public virtual Loai MaLoaiNavigation { get; set; } = null!;

    public virtual NhaCungCap MaNccNavigation { get; set; } = null!;

    public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();

    public virtual ICollection<UserInteraction> UserInteractions { get; set; } = new List<UserInteraction>();


    public static implicit operator HangHoa(CartVM v)
    {
        throw new NotImplementedException();
    }
}
