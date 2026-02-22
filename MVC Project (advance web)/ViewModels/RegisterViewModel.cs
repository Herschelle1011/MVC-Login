using System.ComponentModel.DataAnnotations;

namespace MVC_Project__advance_web_.ViewModels
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Must be a valid format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length must be 6")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length must be 6")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
