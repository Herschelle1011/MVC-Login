using System.ComponentModel.DataAnnotations;

namespace MVC_Project__advance_web_.ViewModels
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
