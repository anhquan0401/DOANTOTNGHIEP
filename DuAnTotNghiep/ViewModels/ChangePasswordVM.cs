using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class ChangePasswordVM
    {
        [Display(Name = "Nhập mật khẩu cũ")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu cũ")]
        public string oldMatKhau { get; set; }

        [Display(Name = "Nhập mật khẩu mới")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu mới")]
        public string newMatKhau { get; set; }

        [Display(Name = "Nhập lại mật khẩu mới")]
        [Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu mới")]
        public string renewMatKhau { get; set; }
    }
}
