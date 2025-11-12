using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.Models
{
    public enum TwoFactorialAuth
    {
        None = 0,
        [Display(Name ="Email Authentication")]
        Email = 1,
        [Display(Name ="Phone Authentication")]
        Phone = 2,
        [Display(Name ="Microsoft / Google Authentication")]
        MicrosoftGoogle = 3,
    }
}
