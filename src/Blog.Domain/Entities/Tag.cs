using Blog.Domain.Errors;

namespace Blog.Domain.Entities;

public class Tag:BaseEntity
{
    public static DomainNotificationError NotFoundError
    {
        get
        {
            DomainNotificationError.ErrorDescription errorDescription =
                new("DomainErrorKey", " Tag not found");
            var error = new DomainNotificationError();
            error.AddError(errorDescription);
            return error;
        }
    }
    public static DomainNotificationError AlreadyExistError
    {
        get
        {
            DomainNotificationError.ErrorDescription errorDescription =
                new("DomainErrorKey", " Tag already Exist");
            var error = new DomainNotificationError();
            error.AddError(errorDescription);
            return error;
        }
    }
    public  string? Name { get; private set; }

    private Tag(string? id, string? name) : base(id, DateTime.UtcNow)
    {
        IsInvalidString(name);
        ValidateErrors();
        Name = name;
    }

    /// <summary>
    /// Create a new blog post tag
    /// </summary>
    /// <param name="id"> tag identifier </param>
    /// <param name="name"> tag name</param>
    /// <returns> new Tag entity</returns>
    public static Tag Create(string? id, string? name)
    {
        return new Tag(id, name);
    }
}