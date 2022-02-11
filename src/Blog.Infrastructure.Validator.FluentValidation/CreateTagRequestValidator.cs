using Blog.Domain.DTOs;
using FluentValidation;

namespace Blog.Infrastructure.Validator.FluentValidation;

public class CreateTagRequestValidator: AbstractValidator<CreateTagRequest>
{
    public CreateTagRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify a  name");
    }
}