namespace Blog.Domain.Entities;

public class TagXBlogPost
{
    private TagXBlogPost(string blogPostId, string tagId)
    {
        BlogPostId = blogPostId;
        TagId = tagId;
    }

    internal static TagXBlogPost Create(string blogPostId, string tagId)
    {
        return new TagXBlogPost(blogPostId, tagId);
    }

    public  string BlogPostId { get; }
    public  string TagId { get; }
    public  virtual BlogPost BLogPost { get; }
    public virtual  Tag Tag { get; }
}