using System.ComponentModel.DataAnnotations;

namespace MVC_Project__advance_web_.ViewModels
{
    public class LoginViewModel 
    {
      
        [Required(ErrorMessage ="Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(6, ErrorMessage = "Password length must be 6")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare( nameof(Password), ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
