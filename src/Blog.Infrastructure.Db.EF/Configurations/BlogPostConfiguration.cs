using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Db.EF.Configurations;

public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(x => x.Content).HasMaxLength(1024);
        builder.HasOne(x => x.Category)
            .WithMany(x => x.BLogPosts)
            .HasForeignKey(x => x.CategoryId);
        builder.HasOne(x => x.Author)
            .WithMany(x => x.BLogPosts)
            .HasForeignKey(x => x.AuthorId);
        
    }
}