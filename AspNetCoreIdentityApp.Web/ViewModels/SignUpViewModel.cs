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

        [EmailAddress(ErrorMessage ="Invalid Format")]
        [Required( ErrorMessage ="Email is required")]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required( ErrorMessage ="Phone is required")]
        
        [Display(Name = "Phone :")]
        public string Phone { get; set; }

        [MinLength(6, ErrorMessage = "Password can not be less than 6 characters")]
        [DataType(DataType.Password)]
        [Required( ErrorMessage ="Password is required")]
        [Display(Name = "Password :")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Passwords did not match")]
        [Required( ErrorMessage ="Repeat Password is required")]

        [Display(Name = "Repeat Password :")]
        public string PasswordConfirm { get; set; }
    }
}
