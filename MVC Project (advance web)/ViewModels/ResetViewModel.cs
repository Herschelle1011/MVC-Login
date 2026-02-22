using System.ComponentModel.DataAnnotations;

namespace MVC_Project__advance_web_.ViewModels
{
    public class ResetViewModel
    
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length must be 6")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Password doesn't match")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length must be 6")]
        [DataType(DataType.Password)]

        public string newPassword { get; set; }

    }
}
