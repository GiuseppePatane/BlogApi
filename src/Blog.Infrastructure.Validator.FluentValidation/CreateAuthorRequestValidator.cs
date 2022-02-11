using Blog.Domain.DTOs;
using FluentValidation;

namespace Blog.Infrastructure.Validator.FluentValidation;

public class CreateAuthorRequestValidator: AbstractValidator<CreateAuthorRequest>
{
    public CreateAuthorRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Please specify a  name");
    }
}