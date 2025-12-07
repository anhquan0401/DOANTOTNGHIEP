    namespace WebApplication1.Data
    {
        public class UserInteraction
        {
            public int Id { get; set; }

            public string MaKh { get; set; } = null!;

            public int MaHh { get; set; }

            public int Count { get; set; }

            public DateTime Timestamp { get; set; }

            public virtual KhachHang MaKhNavigation { get; set; } = null!;

            public virtual HangHoa MaHhNavigation { get; set; } = null!;
    }
    }
