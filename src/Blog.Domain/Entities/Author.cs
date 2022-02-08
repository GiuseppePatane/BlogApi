using Blog.Domain.Errors;

namespace Blog.Domain.Entities;

/// <summary>
/// User that create blog post contents.
/// </summary>
public class Author : BaseEntity
{
    /// <summary>
    /// Generate a new author 
    /// </summary>
    /// <param name="id"> Identifier</param>
    /// <param name="name"> Unique name </param>
    /// <returns></returns>
    public static Author Create(string? id, string? name)
    {
        return new Author(id, name);
    }

    /// <summary>
    /// Create a new blog post  
    /// </summary>
    /// <param name="blogPostId"> blog post identifier </param>
    /// <param name="title"> the title of the post </param>
    /// <param name="content">  the content of the post </param>
    /// <param name="image"> image post </param>
    public void AddBlogPost( string? blogPostId,string? title, string? content, string image)
    {
        var blogPost = BLogPost.Create(blogPostId, title, content, image, this);
        Fail(BLogPosts.Any(x => x.Title.Equals(blogPost.Title, StringComparison.InvariantCultureIgnoreCase)),
            new DomainNotificationError.ErrorDescription("DuplicatePost", "Duplicate post"));
        ValidateErrors();
        BLogPosts.Add(blogPost);
    }
    public string? Name { get; private set; }
    public virtual ICollection<BLogPost> BLogPosts { get; }
    
    private Author(string? id, string? name) : base(id, DateTime.UtcNow)
    {
        IsInvalidString(name);
        ValidateErrors();
        Name = name;
        BLogPosts = new List<BLogPost>();
    }
}