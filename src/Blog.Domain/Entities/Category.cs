using Blog.Domain.Errors;

namespace Blog.Domain.Entities;

public class Category : BaseEntity
{
    private Category(string? id, string? name) : base(id, DateTime.UtcNow)
    {
        IsInvalidString(name);
        ValidateErrors();
        Id = id;
        Name = name;
    }

    public static DomainNotificationError NotFoundError
    {
        get
        {
            var errorDescription =
                DomainNotificationError.ErrorDescription.Create("DomainErrorKey", "category not found");
            var error = new DomainNotificationError();
            error.AddError(errorDescription);
            return error;
        }
    }

    public static DomainNotificationError AlreadyExistError
    {
        get
        {
            var errorDescription =
                DomainNotificationError.ErrorDescription.Create("DomainErrorKey", "category already exist");
            var error = new DomainNotificationError();
            error.AddError(errorDescription);
            return error;
        }
    }

    public string? Name { get; private set; }

    public List<BlogPost> BLogPosts { get; set; }

    public static Category Create(string? id, string? name)
    {
        return new Category(id, name);
    }

    public void Update(string newName)
    {
        IsInvalidString(newName);
        ValidateErrors();
        Name = newName;
        UpdateDateUtc = DateTime.UtcNow;
    }
}