using FluentValidation;
using TicketManagementSystemAPI.DTOs;

namespace TicketManagementSystemAPI.Validator
{
    public class AssignDtoValidator: AbstractValidator<AssignDto>
    {
        public AssignDtoValidator() 
        {
            RuleFor(x => x.userId)
                .GreaterThan(0).WithMessage("User ID must be a valid positive number.");
        }
    }
}
