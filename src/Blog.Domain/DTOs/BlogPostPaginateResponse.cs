namespace Blog.Domain.DTOs;


public class PaginationResponse
{
    public long TotalHits { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int Size { get; set; }
}


public class BlogPostPaginationResponse : PaginationResponse
{
    public List<BlogPostResponse> Items { get; set; }
   
}

public class TagPaginationResponse : PaginationResponse
{
    public List<TagResponse> Items { get; set; }
}
public class CategoryPaginationResponse : PaginationResponse
{
    public List<CategoryResponse> Items { get; set; }
}
public class AuthorPaginationResponse : PaginationResponse
{
    public List<AuthorResponse> Items { get; set; }
}
public class TagResponse
{
    public  string Id { get; set; }
    public  string Name { get; set; }
}

public class CategoryResponse
{
    public  string Id { get; set; }
    public  string Name { get; set; }
}
public class AuthorResponse
{
    public  string Id { get; set; }
    public  string Name { get; set; }
}
public class BlogPostResponse
{
    public  string? BlogPostId { get; set; }
    public  string? Title { get; set; }
    public  string? Content { get; set; }
    public  string? ImageUrl { get; set; }
    public  string? AuthorName { get; set; }
    public  string CategoryName { get; set; }
    public List<string?>? Tags { get; set; }
}