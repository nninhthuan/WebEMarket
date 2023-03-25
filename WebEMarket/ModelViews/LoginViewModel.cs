using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebEMarket.ModelViews
{
    public class LoginViewModel
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại / Hoặc Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Điạ chỉ Email")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Bạn cần cài đặt mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; }
    }
}
