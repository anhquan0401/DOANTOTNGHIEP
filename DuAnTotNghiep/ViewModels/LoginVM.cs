using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class LoginVM
    {
        [Key]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        public string Email { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string MatKhau { get; set; }
    }
}
