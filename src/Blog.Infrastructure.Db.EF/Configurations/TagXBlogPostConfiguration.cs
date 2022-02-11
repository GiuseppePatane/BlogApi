using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Db.EF.Configurations;

public class TagXBlogPostConfiguration : IEntityTypeConfiguration<TagXBlogPost>
{
    public void Configure(EntityTypeBuilder<TagXBlogPost> builder)
    {
        builder.HasKey(post =>  new { post.TagId, post.BlogPostId});

        builder
            .HasOne(sc => sc.BLogPost)
            .WithMany(s => s.TagXBlogPosts)
            .HasForeignKey(sc => sc.BlogPostId);
        
        builder
            .HasOne(sc => sc.Tag)
            .WithMany(s => s.TagXBlogPosts)
            .HasForeignKey(sc => sc.TagId);

    }
}