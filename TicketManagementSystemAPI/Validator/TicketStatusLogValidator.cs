using FluentValidation;
using TicketManagementSystemAPI.Models;

namespace TicketManagementSystemAPI.Validators;

public class TicketStatusLogValidator : AbstractValidator<TicketStatusLog>
{
    public TicketStatusLogValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty().WithMessage("Ticket ID is required.");

        RuleFor(x => x.OldStatus)
            .NotEmpty().WithMessage("Old status is required.")
            .MaximumLength(50).WithMessage("Old status cannot exceed 50 characters.")
            .NotEqual(x => x.NewStatus).WithMessage("Old status cannot be the same as new status.");


        RuleFor(x => x.NewStatus)
            .NotEmpty().WithMessage("New status is required.")
            .MaximumLength(50).WithMessage("New status cannot exceed 50 characters.")
            .NotEqual(x => x.OldStatus).WithMessage("New status cannot be the same as old status.");

        RuleFor(x => x.ChangedAt)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Changed date cannot be in the future.")
            .GreaterThan(DateTime.UtcNow.AddYears(-1)).WithMessage("Changed date cannot be more than 1 year in the past.");

    }
}
