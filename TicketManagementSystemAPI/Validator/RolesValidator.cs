using TicketManagementSystemAPI.Models;
using FluentValidation;
using Microsoft.Identity.Client;

namespace TicketManagementSystemAPI.Validator
{
    public class RolesValidator : AbstractValidator<Role>
    {
        public RolesValidator()
        {
            RuleFor(role => role.Id)
                .NotNull().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(role => role.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");
        }
    }
}
