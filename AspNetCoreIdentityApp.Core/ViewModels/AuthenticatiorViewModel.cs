using AspNetCoreIdentityApp.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class AuthenticatiorViewModel
    {
        public string SharedKey { get; set; }
        public string AuthenticationUri { get; set; }
        [Display(Name = "Verification Code")]
        [Required(ErrorMessage = "Verification code is required")]
        public string VerificationCode { get; set; }
        [Display(Name = "Two-Factor Authentication Type")]
        public TwoFactorialAuth TwoFactorType { get; set; }
    }
}
