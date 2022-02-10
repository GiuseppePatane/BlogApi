namespace Blog.Domain.DTOs;

public record CreateBlogPostRequest(string Title, string Content, string Image, string AuthorId,string CategoryId,List<string>Tags);
public record CreateAuthorRequest(string Name);
public record CreateCategoryRequest(string Name);
public record CreateTagRequest(string Name);

