using FluentValidation;
using System;

namespace TicketManagementSystemAPI.Models;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(255).WithMessage("Password cannot exceed 255 characters.");

        RuleFor(user => user.RoleId)
            .GreaterThan(0).WithMessage("RoleId must be a positive integer.");

        RuleFor(user => user.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow).When(user => user.CreatedAt.HasValue)
            .WithMessage("CreatedAt date cannot be in the future.");

    }
}
