namespace Blog.Domain.DTOs;

public record CreateBlogPostRequest(string Title, string Content, string Image, string AuthorId);
public record CreateAuthorRequest(string Name);

