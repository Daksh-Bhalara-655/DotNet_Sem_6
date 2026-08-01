using DEMO.DTOs.UsersDtos;
using FluentValidation;

namespace DEMO.Validators.UserValidators
{
    public class CreateUserValidator : AbstractValidator<CreatedUsersDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                    .WithMessage("User name is required.")
                .MaximumLength(20)
                    .WithMessage("User name cannot exceed 20 characters.");

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                    .WithMessage("Email address is required.")
                .EmailAddress()
                    .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Password is required.")
                .MinimumLength(6)
                    .WithMessage("Password must be at least 6 characters.")
                .MaximumLength(8)
                    .WithMessage("Password cannot exceed 8 characters.");

            RuleFor(x => x.RoleId)
                .GreaterThan(0)
                    .WithMessage("Please select a valid role.");
        }
    }
}