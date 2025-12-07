using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class ConfirmCodeVM
    {
        [Required(ErrorMessage ="Bạn chưa nhập mã")]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassWord { get; set; }
    }
}
