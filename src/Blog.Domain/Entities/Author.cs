using Blog.Domain.Errors;

namespace Blog.Domain.Entities;

/// <summary>
///     User that create blog post contents.
/// </summary>
public class Author : BaseEntity
{
    private Author(string? id, string? name) : base(id, DateTime.UtcNow)
    {
        IsInvalidString(name);
        ValidateErrors();
        Name = name;
        BLogPosts = new List<BlogPost>();
    }

    public static DomainNotificationError NotFoundError
    {
        get
        {
            var errorDescription =
                DomainNotificationError.ErrorDescription.Create("DomainErrorKey", " author not found");
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
                DomainNotificationError.ErrorDescription.Create("DomainErrorKey", " artist already exist");
            var error = new DomainNotificationError();
            error.AddError(errorDescription);
            return error;
        }
    }

    public string? Name { get; private set; }
    public List<BlogPost> BLogPosts { get; set; }

    /// <summary>
    ///     Generate a new author
    /// </summary>
    /// <param name="id"> Identifier</param>
    /// <param name="name"> Unique name </param>
    /// <returns></returns>
    public static Author Create(string? id, string? name)
    {
        return new Author(id, name);
    }

    public void Update(string name)
    {
        IsInvalidString(name);
        ValidateErrors();
        Name = name;
    }
}