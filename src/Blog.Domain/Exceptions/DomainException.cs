using Blog.Domain.Errors;

namespace Blog.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainNotificationError DomainError;

    public DomainException(DomainNotificationError domainError) : base(domainError.ToString())
    {
        DomainError = domainError;
    }
}