using FluentValidation;
using TicketManagementSystemAPI.DTOs;

namespace TicketManagementSystemAPI.Validator
{
    public class CreateUserDtoValidator: AbstractValidator<CreateUserDTO>
    {
        public CreateUserDtoValidator() { 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(255).WithMessage("Password cannot exceed 255 characters.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => role == "MANAGER" || role == "SUPPORT" || role == "USER")
                .WithMessage("Role must be either MANAGER, SUPPORT, or USER.");

        }
    }
}
