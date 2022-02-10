using Blog.Domain.Errors;

namespace Blog.Domain.Entities;


/// <summary>
/// A new post 
/// </summary>
public  class BlogPost :BaseEntity
{
    public static DomainNotificationError AlreadyExistError
    {
        get
        {
            DomainNotificationError.ErrorDescription errorDescription = DomainNotificationError.ErrorDescription.Create("DomainErrorKey", " blog post already exist");
            var error = new DomainNotificationError();
            error.AddError(errorDescription);
            return error;
        }
    }
    private BlogPost(string? id, string? title, string? content,string? image, string? authorId,string? categoryId) : base(id, DateTime.UtcNow)
    {
        IsInvalidString(id);
        IsInvalidString(title);
        IsInvalidString(content);
        IsInvalidString(image);
        IsInvalidString(authorId);
        IsInvalidString(categoryId);
        ValidateErrors();
        Title = title;
        Content = content;
        AuthorId = authorId;
        CategoryId = categoryId;
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
    /// <param name="category"></param>
    /// <returns></returns>
    public static BlogPost Create(string? id, string? title, string? content, string? image, Author? author,Category? category)
    {
        return new BlogPost(id, title, content, image, author?.Id,category?.Id);
    }

    /// <summary>
    /// Associate a specific <see cref="Tag"/> to the current blog post
    /// </summary>
    /// <param name="tag"></param>
    public void AssociateWithTag(Tag? tag)
    {
        Fail(tag==null,
             DomainNotificationError.ErrorDescription.Create("TagNotFound", "Tag not found."));
        ValidateErrors();
        Fail(TagXBlogPosts.Any(x => x.TagId == tag?.Id && x.BlogPostId == Id),
             DomainNotificationError.ErrorDescription.Create("TagAlreadyAssociated", "Tag already associated."));
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
             DomainNotificationError.ErrorDescription.Create("Category not found", "category not  found"));
        ValidateErrors();
        CategoryId = category?.Id;
    }
    public string?  Title { get; private set; }
    public  string?  Content { get; private set; }
    public  string? AuthorId { get; private set; }
    public  string? Image { get; private set; }
    public  string? CategoryId { get; private set; }
    
    public ICollection<TagXBlogPost> TagXBlogPosts { get; private set; }
    public Category Category { get; private set; }
    public Author  Author { get; private set; }


    public void Update(string title, string content, string image)
    {
        bool endited = false;
        if (!string.IsNullOrWhiteSpace(title))  { Title = title; endited = true;}
        if (!string.IsNullOrWhiteSpace(content)) {Content = content; endited = true; }
        if (!string.IsNullOrWhiteSpace(image)){ Image = image; endited = true; }
        if(endited) UpdateDateUtc=DateTime.UtcNow;
    }
}