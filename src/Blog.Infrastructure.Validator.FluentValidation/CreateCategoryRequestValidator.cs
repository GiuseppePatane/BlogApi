using Blog.Domain.DTOs;
using FluentValidation;

namespace Blog.Infrastructure.Validator.FluentValidation;

public class CreateCategoryRequestValidator: AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify a  name");
    }
}

