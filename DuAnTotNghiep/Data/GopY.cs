using System;
using System.Collections.Generic;

namespace WebApplication1.Data;

public partial class GopY
{
    public Guid MaGy { get; set; }

    public string? MaKh { get; set; }

    public string? NoiDung { get; set; }

    public DateOnly NgayGy { get; set; }

    public string? HoTen { get; set; }

    public string? Email { get; set; }

    public string? DienThoai { get; set; }

    public bool CanTraLoi { get; set; }

    public string? TraLoi { get; set; }

    public DateOnly? NgayTl { get; set; }
}
