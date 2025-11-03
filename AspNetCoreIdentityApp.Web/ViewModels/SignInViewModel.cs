using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {
            
        }

        public SignInViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [EmailAddress(ErrorMessage ="Invalid format")]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]

        [Display(Name = "Password :")]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
