namespace WebApplication1.Data
{
    public class History
    {
        public int Id { get; set; }

        public string MaKh { get; set; } = null!;

        public string? Keyword { get; set; }

        public DateTime? Timestamp { get; set; }

        public virtual KhachHang MaKhNavigation { get; set; } = null!;

    }
}
