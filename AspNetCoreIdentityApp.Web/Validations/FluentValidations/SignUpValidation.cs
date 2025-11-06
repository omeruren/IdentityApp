using AspNetCoreIdentityApp.Core.ViewModels;
using FluentValidation;

namespace AspNetCoreIdentityApp.Web.Validations.FluentValidations
{
    public class SignUpValidation : AbstractValidator<SignUpViewModel>
    {
        public SignUpValidation()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("User name is required");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required");
            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("Phone is required");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");
            RuleFor(x => x.PasswordConfirm)
                .NotEmpty()
                .WithMessage("Confirm Password is required");


        }
    }
}
