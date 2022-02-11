namespace Blog.Domain.DTOs;

public record CreateResponse(string? Id);

public class Response
{
    public ErrorResponse Error { get; set; }
}

public class ErrorResponse
{
    public ErrorResponse()
    {
        Errors = new List<ErrorElement>();
    }

    public List<ErrorElement> Errors { get; set; }
}

public class ErrorElement
{
    public string Field { get; set; }
    public string Code { get; set; }
    public string Message { get; set; }
}