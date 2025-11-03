using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]

        [Display(Name = "New Password :")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords did not match")]
        [Required(ErrorMessage = "Repeat Password is required")]

        [Display(Name = "Repeat New Password :")]
        public string PasswordConfirm { get; set; }
    }
}
