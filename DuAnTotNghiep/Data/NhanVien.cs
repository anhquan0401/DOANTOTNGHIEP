using System;
using System.Collections.Generic;

namespace WebApplication1.Data;

public partial class NhanVien
{
    public string MaNv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? HashMatKhau { get; set; }

    public int MaPq { get; set; }

    public bool XacNhan { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual PhanQuyen? MaPqNavigation { get; set; }




}
