using System;
using System.Collections.Generic;

namespace WebApplication1.Data;

public partial class PhanQuyen
{
    public int MaPq { get; set; }

    public string? TenPq { get; set; }

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();

}
