namespace Blog.Domain.DTOs;


public class PaginationResponse
{
    public long TotalHits { get; set; }
    public int From { get; set; }
    public int Size { get; set; }
}


public class BlogPostPaginationResponse : PaginationResponse
{
    public List<BlogPostResponse> Items { get; set; }
    public int TotalPages { get; set; }
}
public class BlogPostResponse
{
    public  string? BlogPostId { get; set; }
    public  string? Title { get; set; }
    public  string? Content { get; set; }
    public  string? Image { get; set; }
    public  string? AuthorName { get; set; }
    public  string CategoryName { get; set; }
    public List<string>Tags { get; set; }
}