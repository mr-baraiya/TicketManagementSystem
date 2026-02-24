using FluentValidation;
using TicketManagementSystemAPI.Models;

namespace TicketManagementSystemAPI.Validators;

public class TicketCommentValidator : AbstractValidator<TicketComment>
{
    public TicketCommentValidator()
    {
        RuleFor(x => x.TicketId)
            .GreaterThan(0).WithMessage("Ticket ID must be a valid positive number.");
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be a valid positive number.");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment cannot be empty.")
            .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");

        RuleFor(x => x.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow).When(x => x.CreatedAt != null)
            .WithMessage("Comment creation date cannot be in the future.")
            .GreaterThan(DateTime.UtcNow.AddYears(-1)).When(x => x.CreatedAt != null)
            .WithMessage("Comment creation date cannot be more than 1 year in the past.");

    }
}
