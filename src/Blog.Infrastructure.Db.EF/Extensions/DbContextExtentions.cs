using Blog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Blog.Infrastructure.Db.EF.Extensions;

public static class DbContextExtensions
{

    public static void InitTestData(this BlogDbContext blogDbContext, ILogger logger)
    {
        var author = Author.Create("author1", "author 1");
        var category = Category.Create("category1", "fake category");
        var tag = Tag.Create("tag1", "fake tag");
        var blogPost = BlogPost.Create("1", "test","string","image",author,category,new List<Tag>(){tag});
        if (!blogDbContext.Authors.Any(x => x.Id == author.Id))
        {
            logger.LogWarning("Create fake author");
            blogDbContext.Authors.Add(author);
            blogDbContext.SaveChanges();
        }
        if (!blogDbContext.Categories.Any(x => x.Id == category.Id))
        {
            logger.LogWarning("Create fake category");
            blogDbContext.Categories.Add(category);
            blogDbContext.SaveChanges();
        }
        if (!blogDbContext.Tags.Any(x => x.Id == tag.Id))
        {
            logger.LogInformation("Create fake tag");
            blogDbContext.Tags.Add(tag);
            blogDbContext.SaveChanges();
        }
        if (!blogDbContext.BlogPosts.Any(x => x.Id == blogPost.Id))
        {
            logger.LogInformation("Create fake blogPost");
            blogDbContext.BlogPosts.Add(blogPost);
            blogDbContext.SaveChanges();
        }
    }
}