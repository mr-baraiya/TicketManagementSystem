using FluentValidation;
using TicketManagementSystemAPI.Models;

namespace TicketManagementSystemAPI.Validators
{
    public class TicketValidator : AbstractValidator<Ticket>
    {
        public TicketValidator()
        {
            RuleFor(ticket => ticket.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(ticket => ticket.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(ticket => ticket.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => new[] { "Open", "InProgress", "Resolved", "Closed" }.Contains(status))
                .WithMessage("Status must be Open, InProgress, Resolved or Closed.");

            RuleFor(ticket => ticket.Priority)
                .NotEmpty().WithMessage("Priority is required.")
                .Must(priority => new[] { "Low", "Medium", "High" }.Contains(priority))
                .WithMessage("Priority must be Low, Medium, or High.");

            RuleFor(ticket => ticket.CreatedBy)
                .GreaterThan(0).WithMessage("Valid creator is required.");

            RuleFor(ticket => ticket.AssignedTo)
                .GreaterThan(0).When(ticket => ticket.AssignedTo.HasValue)
                .WithMessage("Valid assigned user is required.");
        }
    }
}
