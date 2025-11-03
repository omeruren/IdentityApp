using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {

        }
        public SignUpViewModel(string userName, string email, string phone, string password)
        {
            UserName = userName;
            Email = email;
            Phone = phone;
            Password = password;
        }

        [Required( ErrorMessage ="User name is required")]
        [Display(Name = "User Name :")]
        public string UserName { get; set; }

        [Required( ErrorMessage ="Email is required")]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required( ErrorMessage ="Phone is required")]
        
        [Display(Name = "Phone :")]
        public string Phone { get; set; }

        [Required( ErrorMessage ="Password is required")]
        
        [Display(Name = "Password :")]
        public string Password { get; set; }

        [Required( ErrorMessage ="Repeat Password is required")]

        [Display(Name = "Repeat Password :")]
        public string PasswordConfirm { get; set; }
    }
}
