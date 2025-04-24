using FluentValidation;
using TodoApi.Models.DTOs;

namespace TodoApi.Validators;


public class TodoDtoValidator : AbstractValidator<TodoDto>
{
    public TodoDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.ExpiryDateTime)
            .NotEmpty().WithMessage("Expiry date and time are required")
            .Must(BeInFuture).WithMessage("Expiry date and time must be in the future");

        RuleFor(x => x.PercentComplete)
            .InclusiveBetween(0, 100).WithMessage("Percent complete must be between 0 and 100");
    }

    private bool BeInFuture(DateTime dateTime)
    {
        return dateTime > DateTime.Now;
    }
}