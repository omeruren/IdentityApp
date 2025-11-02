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

        [Display(Name = "User Name :")]
        public string UserName { get; set; }
        [Display(Name = "Email :")]
        public string Email { get; set; }
        [Display(Name = "Phone :")]
        public string Phone { get; set; }
        [Display(Name = "Password :")]
        public string Password { get; set; }
        [Display(Name = "Repeat Password :")]
        public string PasswordConfirm { get; set; }
    }
}
