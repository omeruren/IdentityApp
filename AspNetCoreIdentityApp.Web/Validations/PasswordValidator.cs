using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.Validations
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {

                errors.Add(new() { Code = "PasswordContainsUserName", Description = "Password  can not contains user name" });

            }

            if (password.ToLower().StartsWith("1234"))
            {
                errors.Add(new() { Code = "PasswordContains1234", Description = "Password can not start with consecutive numbers" });
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
