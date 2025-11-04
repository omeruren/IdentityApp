using AspNetCoreIdentityApp.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User Name :")]
        public string UserName { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Invalid Format")]
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone is required")]
        [Display(Name = "Phone :")]
        public string Phone { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date :")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "City :")]
        public string? City { get; set; }

        [Display(Name = "Profile Picture :")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "Gender :")]
        public Gender? Gender { get; set; }

    }
}
