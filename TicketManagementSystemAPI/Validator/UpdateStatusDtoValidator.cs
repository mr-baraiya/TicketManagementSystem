using FluentValidation;
using TicketManagementSystemAPI.DTOs;

namespace TicketManagementSystemAPI.Validator
{
    public class UpdateStatusDtoValidator: AbstractValidator<UpdateStatusDto>
    {
        public UpdateStatusDtoValidator()
        { 
            RuleFor(x => x.status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => new[] { "OPEN", "IN_PROGRESS", "RESOLVED", "CLOSED" }.Contains(status.ToUpper()))
                .WithMessage("Status must be one of the following: OPEN, IN_PROGRESS, RESOLVED, CLOSED.");
        }
    }
}
