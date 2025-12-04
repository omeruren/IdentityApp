using AspNetCoreIdentityApp.Core.PermissionsRoot;
using AspNetCoreIdentityApp.Repository.Models;
using AspNetCoreIdentityApp.Service.Services;
using AspNetCoreIdentityApp.Web.ClaimProviders;
using AspNetCoreIdentityApp.Web.Helpers.Localization;
using AspNetCoreIdentityApp.Web.Requirements;
using AspNetCoreIdentityApp.Web.Validations;
using AspNetCoreIdentityApp.Web.Validations.FluentValidations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class ServiceExtensions
    {

        public static void ConfigureSqlConnectionExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("SqlConnection"), opt =>
                {
                    opt.MigrationsAssembly("AspNetCoreIdentityApp.Repository");
                });
            });

        }
        public static void AddIdentityServiceExt(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromMinutes(30);
            });

            services.AddAuthentication().AddFacebook(opts =>
            {
                opts.AppId = configuration["Authentication:Facebook:AppId"];
                opts.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            }).AddGoogle(opt =>
            {
                opt.ClientId = configuration["Authentication:Google:ClientID"];
                opt.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.User.AllowedUserNameCharacters = "abcçdeföÖgğhıijklmnoçpqrsştuüvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            }).AddPasswordValidator<PasswordValidator>()
               .AddUserValidator<UserValidatior>()
               .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
               .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<AppDbContext>();
        }

        public static void RegisterServicesExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
            services.AddScoped<TwoFactorialService>();
            services.AddIdentityServiceExt(configuration);
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IClaimsTransformation, UserClaimProvider>();
            services.AddScoped<IAuthorizationHandler, ExchangeExrpireRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();
            services.AddScoped<IMemberService, MemberService>();
        }
        public static void RegisterPoliciesAndPermissionsExt(this IServiceCollection services)
        {

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("GaziantepPolicy", policy =>
                {
                    policy.RequireClaim("city", "Gaziantep");
                });
                opt.AddPolicy("ExchangeExpireDate", policy =>
                {
                    policy.AddRequirements(new ExchangeExpireRequirment());
                });
                opt.AddPolicy("ViolencePolicy", policy =>
                {
                    policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 });
                });
                opt.AddPolicy("OrderPermissionReadOrDelete", policy =>
                {
                    policy.RequireClaim("permission", Permissions.Order.Read);
                    policy.RequireClaim("permission", Permissions.Order.Delete);
                    policy.RequireClaim("permission", Permissions.Stock.Delete);
                });
                opt.AddPolicy("Permissions.Order.Read", policy =>
                {
                    policy.RequireClaim("permission", Permissions.Order.Read);

                });
                opt.AddPolicy("Permissions.Order.Delete", policy =>
                {
                    policy.RequireClaim("permission", Permissions.Order.Delete);
                });
                opt.AddPolicy("Permissions.Stock.Delete", policy =>
                {
                    policy.RequireClaim("permission", Permissions.Stock.Delete);

                });


            });
        }

        public static void ConfigureApplicationCookieExt(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(opt =>
            {
                var cookieBuilder = new CookieBuilder();

                cookieBuilder.Name = "IdentityCookie";

                opt.LoginPath = new PathString("/Home/SignIn");

                opt.LogoutPath = new PathString("/Member/Logout");

                opt.AccessDeniedPath = new PathString("/Member/AccessDenied");

                opt.Cookie = cookieBuilder;
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                opt.SlidingExpiration = true;
            });
        }
    }
}
