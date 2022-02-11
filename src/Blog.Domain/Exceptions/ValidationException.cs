using Blog.Domain.DTOs;

namespace Blog.Domain.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(Response response)
    {
        Response = response ?? throw new ArgumentNullException(nameof(response));
    }

    public Response Response { get; }
}