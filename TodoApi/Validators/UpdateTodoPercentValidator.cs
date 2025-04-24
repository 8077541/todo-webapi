using FluentValidation;
using TodoApi.Models.DTOs;


namespace TodoApi.Validators;

public class UpdateTodoPercentDtoValidator : AbstractValidator<UpdateTodoPercentDto>
{
    public UpdateTodoPercentDtoValidator()
    {
        RuleFor(x => x.PercentComplete)
            .InclusiveBetween(0, 100).WithMessage("Percent complete must be between 0 and 100");
    }
}