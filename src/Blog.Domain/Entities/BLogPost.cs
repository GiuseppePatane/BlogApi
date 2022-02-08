using System.Diagnostics;
using Blog.Domain.Errors;

namespace Blog.Domain.Entities;


/// <summary>
/// A new post 
/// </summary>
public class BLogPost :BaseEntity
{
    private BLogPost(string? id, string? title, string? content,string? image, string? authorId) : base(id, DateTime.UtcNow)
    {
        IsInvalidString(id);
        IsInvalidString(title);
        IsInvalidString(content);
        IsInvalidString(image);
        IsInvalidString(authorId);
        ValidateErrors();
        Title = title;
        Content = content;
        AuthorId = authorId;
        Image = image;
        TagXBlogPosts = new List<TagXBlogPost>();
    }

    /// <summary>
    /// Create a new blog post with the provided parameters
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="image"></param>
    /// <param name="author"></param>
    /// <returns></returns>
    public static BLogPost Create(string? id, string? title, string? content, string? image, Author? author)
    {
        return new BLogPost(id, title, content, image, author?.Id);
    }

    /// <summary>
    /// Associate a specific <see cref="Tag"/> to the current blog post
    /// </summary>
    /// <param name="tag"></param>
    public void AssociateWithTag(Tag? tag)
    {
        Fail(tag==null,
            new DomainNotificationError.ErrorDescription("TagNotFound", "Tag not found."));
        ValidateErrors();
        Fail(TagXBlogPosts.Any(x => x.TagId == tag?.Id && x.BlogPostId == Id),
            new DomainNotificationError.ErrorDescription("TagAlreadyAssociated", "Tag already associated."));
        ValidateErrors();
        TagXBlogPosts.Add(TagXBlogPost.Create(Id,tag.Id));
    }

    /// <summary>
    /// Associate a specific <see cref="Category"/> tto the current  blog post 
    /// </summary>
    /// <param name="category"></param>
    public void AssociateWithCategory(Category? category)
    {
        Fail(category==null,
            new DomainNotificationError.ErrorDescription("Category not found", "category not  found"));
        ValidateErrors();
        CategoryId = category?.Id;
    }
    public string?  Title { get; private set; }
    public  string?  Content { get; private set; }
    public  string? AuthorId { get; private set; }
    public  string? Image { get; private set; }
    public  string? CategoryId { get; private set; }
    
    public  virtual  ICollection<TagXBlogPost> TagXBlogPosts { get; private set; }
    public  virtual Category Category { get; private set; }
    public  virtual Author  Author { get; private set; }

  
}