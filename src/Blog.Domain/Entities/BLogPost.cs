using Blog.Domain.Errors;

namespace Blog.Domain.Entities;


/// <summary>
/// A new post 
/// </summary>
public  class BlogPost :BaseEntity
{
    public static DomainNotificationError NotFoundError
    {
        get
        {
            DomainNotificationError.ErrorDescription errorDescription =
                DomainNotificationError.ErrorDescription.Create("DomainErrorKey", "blog post not found");
            var error = new DomainNotificationError();
            error.AddError(errorDescription);
            return error;
        }
    }
    
    public static DomainNotificationError AlreadyExistError
    {
        get
        {
            DomainNotificationError.ErrorDescription errorDescription = DomainNotificationError.ErrorDescription.Create("DomainErrorKey", "blog post already exist");
            var error = new DomainNotificationError();
            error.AddError(errorDescription);
            return error;
        }
    }
    private BlogPost()
    {
        
    }
    private BlogPost(string? id, string? title, string? content,string? image, string? authorId,string? categoryId,List<TagXBlogPost>? tagXBlogPosts) : base(id, DateTime.UtcNow)
    {
        IsInvalidString(id);
        IsInvalidString(title);
        IsInvalidString(content);
        IsInvalidString(image);
        IsInvalidString(authorId);
        IsInvalidString(categoryId);
        Fail(tagXBlogPosts == null || !tagXBlogPosts.Any(),DomainNotificationError.ErrorDescription.Create("tagXBlogPosts cannot be nul"));
        ValidateErrors();
        Title = title;
        Content = content;
        AuthorId = authorId;
        CategoryId = categoryId;
        Image = image;
        TagXBlogPosts = tagXBlogPosts;
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
    /// <param name="tags"></param>
    /// <returns></returns>
    public static BlogPost Create(string? id, string? title, string? content, string? image, Author? author,Category? category,List<Tag> tags)
    {
        var tagsXBlog= tags?.Select(x => TagXBlogPost.Create(id, x.Id)).ToList();
        return new BlogPost(id, title, content, image, author?.Id,category?.Id,tagsXBlog);
    }

    /// <summary>
    /// Associate a specific <see cref="Tag"/> to the current blog post
    /// </summary>
    /// <param name="tag"></param>
    public void AssociateTag(Tag? tag)
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
    /// Associate a specific <see cref="Category"/> to the current  blog post 
    /// </summary>
    /// <param name="category"></param>
    public void UpdateCategory(Category? category)
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
        bool edited = false;
        if (!string.IsNullOrWhiteSpace(title))  { Title = title; edited = true;}
        if (!string.IsNullOrWhiteSpace(content)) {Content = content; edited = true; }
        if (!string.IsNullOrWhiteSpace(image)){ Image = image; edited = true; }
        if(edited) UpdateDateUtc=DateTime.UtcNow;
    }
}