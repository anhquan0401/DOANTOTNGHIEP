using WebApplication1.Data;

namespace WebApplication1.ViewModels
{
    public class CommentVM
    {
        public int CommentID { get; set; }
        public int MaHh { get; set; }
        public string MaKh { get; set; } = null!;
        public string? CommentDescription { get; set; }
        public int Rating { get; set; }
        public DateTime CommentDate { get; set; }

    }
}
