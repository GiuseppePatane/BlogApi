using Blog.Domain.Errors;
using Blog.Domain.Exceptions;

namespace Blog.Domain.Entities;

public abstract class BaseEntity
{
    protected BaseEntity(string? id, DateTime creationDateUtc)
    {
        IsInvalidString(id);
        IsValidDate(CreationDateUtc);
        Fail(creationDateUtc<DateTime.UtcNow.Date,DomainNotificationError.ErrorDescription.Create("CreationDate cannot be in the past"));
        ValidateErrors();
        Id = id;
        CreationDateUtc = creationDateUtc;
    }
   
    public string? Id { get; protected set; }
    public DateTime CreationDateUtc { get; protected set; }
    public DateTime? UpdateDateUtc { get; protected set; }
    
    protected DomainNotificationError Error { get; } = new DomainNotificationError();

    #region Validations

    protected  void ValidateErrors()
    {
        if (Error.HasErrors()) throw new DomainException(Error);
    }

    /// <summary>
    /// Check if the given key is a valid guid 
    /// </summary>
    /// <param name="key"> the guid to check </param>
    /// <param name="field"> the name of the field</param>
    protected void IsInvalidGuid(Guid key, string? field = null)
    {
        var errorMessage = $"Value {key} is an invalid guid.";
        if (!string.IsNullOrWhiteSpace(field))
        {
            errorMessage = $"{field} is an invalid guid.";
        }

        DomainNotificationError.ErrorDescription error =
             DomainNotificationError.ErrorDescription.Create("InvalidGuid", errorMessage);
        Fail(key == Guid.Empty, error);
    }

    protected  void IsValidDate(DateTime? key, string? field = null)
    {
        var errorMessage = $"Value {key} is an invalid DateTime.";
        DomainNotificationError.ErrorDescription error =
             DomainNotificationError.ErrorDescription.Create("InvalidDateTime", errorMessage);
        Fail(!key.HasValue, error);
        if (key.HasValue)
        {
            var value = key.GetValueOrDefault();
            Fail(value.Kind != DateTimeKind.Unspecified, error);
            if (value.Kind != DateTimeKind.Unspecified) ;
        }
    }
    protected void IsInvalidString(string? key, string? field = null)
    {
        var errorMessage = "Value cannot be null or whitespace.";
        if (!string.IsNullOrWhiteSpace(field))
        {
            errorMessage = $"{field} cannot be null or whitespace.";
        }

        DomainNotificationError.ErrorDescription error =
             DomainNotificationError.ErrorDescription.Create("InvalidString", errorMessage);
        Fail(string.IsNullOrWhiteSpace(key), error);
    }

    protected void IsInvalidInt(int key, string? field = null)
    {
        var errorMessage = "Value cannot be less or equal to 0";
        if (!string.IsNullOrWhiteSpace(field))
        {
            errorMessage = $" {field}  cannot be less or equal to 0";
        }

        DomainNotificationError.ErrorDescription error =
             DomainNotificationError.ErrorDescription.Create("InvalidInt", errorMessage);
        Fail(key <= 0, error);
    }

    protected void Fail(bool condition, DomainNotificationError.ErrorDescription description)
    {
        if (condition)
            Error.AddError(description);
    }

    #endregion
}