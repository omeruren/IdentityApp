using AspNetCoreIdentityApp.Web.Helpers.Localization;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Validations;
using AspNetCoreIdentityApp.Web.Validations.FluentValidations;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredUniqueChars = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = true;

                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            }).AddPasswordValidator<PasswordValidator>()
            .AddUserValidator<UserValidatior>()
            .AddErrorDescriber<LocalizationIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
