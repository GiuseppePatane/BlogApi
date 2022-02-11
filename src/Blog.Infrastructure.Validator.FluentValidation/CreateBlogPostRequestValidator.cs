using Blog.Domain.DTOs;
using Blog.Domain.Helpers;
using FluentValidation;

namespace Blog.Infrastructure.Validator.FluentValidation;

public class CreateBlogPostRequestValidator : AbstractValidator<CreateBlogPostRequest>
{
    public CreateBlogPostRequestValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("Title cannot be null or empty");
        RuleFor(x => x.Content).NotNull().NotEmpty().WithMessage("Content cannot be null or empty");
        RuleFor(x => x.Content).MaximumLength(1024).WithMessage("Invalid Content length, max 1024 characters");
        RuleFor(x => x.ImageUrl).NotNull().NotEmpty().WithMessage("ImageUrl cannot be null or empty");
        RuleFor(x => x.ImageUrl).Must(ValidationsHelper.IsFile).WithMessage("Invalid image url format");
        RuleFor(x => x.AuthorId).NotNull().NotEmpty().WithMessage("AuthorId cannot be null or empty");
        RuleFor(x => x.CategoryId).NotNull().NotEmpty().WithMessage("CategoryId cannot be null or empty");
        RuleFor(x => x.Tags).NotNull().NotEmpty().WithMessage("Tags cannot be null or empty");
    }
}

public class UpdateBlogPostRequestValidator : AbstractValidator<UpdateBlogPostRequest>
{
    public UpdateBlogPostRequestValidator()
    {

        RuleFor(x => x.Content).MaximumLength(1024).WithMessage("Invalid Content length, max 1024 characters");
        RuleFor(x => x.ImageUrl).Must(ValidationsHelper.IsFile).WithMessage("Invalid image url format");
      
    }
}