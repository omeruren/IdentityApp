using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid format")]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email :")]
        public string Email { get; set; }
    }
}
