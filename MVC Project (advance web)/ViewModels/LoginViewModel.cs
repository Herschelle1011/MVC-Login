using System.ComponentModel.DataAnnotations;

namespace MVC_Project__advance_web_.ViewModels
{
    public class LoginViewModel 
    {
      
        //VALIDATION ALL INPUTS FOR THE VIEW
        [Required(ErrorMessage ="Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        //[Required(ErrorMessage = "Confirm Password is required")]
        //[DataType(DataType.Password)]
        //[Compare( nameof(Password), ErrorMessage = "Password and Confirm Password do not match")]
        //public string ConfirmPassword { get; set; } = string.Empty;
    }
}
