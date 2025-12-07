namespace WebApplication1.Areas.Admin.Models
{
    public class CommentAdminVM
    {
        public int CommentID { get; set; }
        public int MaHh { get; set; }
        public string MaKh { get; set; } = null!;
        public string? CommentDescription { get; set; }
        public int Rating { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
