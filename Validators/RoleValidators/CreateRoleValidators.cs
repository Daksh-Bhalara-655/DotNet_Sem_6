using DEMO.DTOs.RolesDtos;
using FluentValidation;

namespace DEMO.Validators.RoleValidators
{
    public class CreateRoleValidators : AbstractValidator<CreatedRoleDto>
    {
        public CreateRoleValidators() {
            RuleFor(x => x.RoleName)
                .NotEmpty()
                    .WithMessage("Role name is required.")
                .MaximumLength(20)
                    .WithMessage("Role name cannot exceed 20 characters.")
                .Must(role => new[] { "Admin", "Student", "Faculty", "HOD", "Principal" }
                    .Contains(role, StringComparer.OrdinalIgnoreCase))
                    .WithMessage("Role name must be one of: Admin, Student, Faculty, HOD, Principal.");
        }
    }
}
