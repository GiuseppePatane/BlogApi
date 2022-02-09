using Blog.Domain.Entities;
using Blog.Infrastructure.Db.EF.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Db.EF;

public class BlogDbContext :DbContext 
{
     public BlogDbContext(DbContextOptions<BlogDbContext> options)
        : base(options)
    {
    }
     public  DbSet<Author> Authors { get; set; }
     public  DbSet<Category>Categories { get; set; }
     public  DbSet<Tag> Tags { get; set; }
     public  DbSet<BlogPost>BlogPosts { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new TagXBlogPostConfiguration());
    }
}