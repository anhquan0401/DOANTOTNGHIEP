using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class forgotPasswordVM
    {
        [Required(ErrorMessage ="Email không được để trống")]
        [EmailAddress(ErrorMessage ="Email không hợp lệ")]
        public string Email { get; set; }
    }
}
