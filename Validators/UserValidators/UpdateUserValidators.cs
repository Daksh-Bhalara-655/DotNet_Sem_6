using DEMO.DTOs.UsersDtos;
using FluentValidation;

namespace DEMO.Validators.UserValidators
{
    public class UpdateUserValidators : AbstractValidator<UpdatedUserDto>
    {
        public UpdateUserValidators()
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
                    .WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Password is required.")
                .MinimumLength(6)
                    .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100)
                    .WithMessage("Password cannot exceed 100 characters long.");

            RuleFor(x => x.RoleId)
                .NotEmpty()
                    .WithMessage("Role ID is required.");
        }
    }
}
