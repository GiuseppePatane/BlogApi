using System.Collections.ObjectModel;

namespace Blog.Domain.Errors;

public class DomainNotificationError
{
    private readonly List<ErrorDescription> _errors = new();
    public int NumbersOfErrors => _errors.Count;
    public ReadOnlyCollection<ErrorDescription> Errors => _errors.AsReadOnly();

    public void AddError(ErrorDescription description)
    {
        if (description == null) throw new ArgumentNullException(nameof(description));
        _errors.Add(description);
    }

    public string ErrorMessage()
    {
        return string.Join(',', _errors.Select(x => x));
    }

    public bool HasErrors()
    {
        return _errors.Any();
    }

    public class ErrorDescription
    {
        private ErrorDescription(string code, string message, string field) : this(code, message)
        {
            Field = field;
        }


        private ErrorDescription(string code, string message) : this(message)
        {
            Code = code;
        }


        private ErrorDescription(string message)
        {
            Message = message;
        }

        public string Code { get; }
        private string Message { get; }
        public string Field { get; }

        internal static ErrorDescription Create(string message)
        {
            return new ErrorDescription(message);
        }

        internal static ErrorDescription Create(string code, string message)
        {
            return new ErrorDescription(code, message);
        }

        internal static ErrorDescription Create(string code, string message, string field)
        {
            return new ErrorDescription(code, message, field);
        }

        public override string ToString()
        {
            return Message;
        }
    }
}