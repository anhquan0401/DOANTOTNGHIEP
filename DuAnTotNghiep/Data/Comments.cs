namespace WebApplication1.Data
{
    public class Comments
    {
        public int CommentID { get; set; }
        public int MaHh { get; set; }
        public string MaKh { get; set; } = null!;
        public string? CommentDescription { get; set; }
        public int Rating { get; set; }
        public DateTime CommentDate { get; set; }

        public virtual KhachHang MaKhNavigation { get; set; } = null!;
        public virtual HangHoa MaHhNavigation { get; set; } = null!;
    }
}
