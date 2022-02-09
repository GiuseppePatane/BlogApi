namespace Blog.Domain.Entities;

public class Category:BaseEntity
{
    private Category(string? id, string? name) : base(id,DateTime.UtcNow)
    {
        IsInvalidString(name);
        ValidateErrors();
        Id = id;
        Name = name;
    }

    public static Category Create(string? id, string? name)
    {
        return new Category(id, name);
    }
    public  string? Name { get; private set; }
    
    public  ICollection<BlogPost> BLogPosts { get; set; }

    public void Update(string newName)
    {
       IsInvalidString(newName);
       ValidateErrors();
       Name = newName;
       UpdateDateUtc = DateTime.UtcNow;
    }
}