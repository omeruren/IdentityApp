using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password :")]
        [MinLength(6, ErrorMessage = "Password can not be less than 6 characters")]

        public string OldPassword { get; set; } = null!;


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "New Password is required")]
        [Display(Name = "New Password :")]
        [MinLength(6, ErrorMessage = "Password can not be less than 6 characters")]
        public string NewPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords did not match")]
        [Required(ErrorMessage = "Repeat New Password is required")]
        [Display(Name = "Repeat New Password :")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
