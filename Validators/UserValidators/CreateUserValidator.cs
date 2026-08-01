using DEMO.DTOs.UsersDtos;
using FluentValidation;

namespace DEMO.Validators.UserValidators
{
    public class CreateUserValidator : AbstractValidator<CreatedUsersDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserName)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty()
                    .WithMessage("User name is required.").
                   MaximumLength(20)
                    .WithMessage("User name cannot exceed 20 characters.");

            RuleFor(x => x.EmailAddress)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Email address is required.")
                .EmailAddress()
                    .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.RoleId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                    .WithMessage("Please select a valid role.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Password is required.")
                .MinimumLength(6)
                    .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100)
                    .WithMessage("Password cannot exceed 100 characters long.");
        }
    }
}