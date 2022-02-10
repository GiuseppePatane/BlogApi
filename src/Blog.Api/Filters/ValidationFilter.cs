using Blog.Domain.DTOs;
using Blog.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Api.Filters;

public class ValidationFilter : TypeFilterAttribute
{
    public ValidationFilter() : base(typeof(ValidationHandlerFilter))
    {
    }

    public class ValidationHandlerFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var response = new Response
                {
                    Error = new ErrorResponse()
                };
                foreach (var key in context.ModelState.Keys)
                {
                    var modelStateEntry = context.ModelState[key];
                    if (modelStateEntry != null)
                    {
                        foreach (var value in modelStateEntry.Errors)
                        {
                            var validationErrorResponse = new ErrorElement() { Field = key,Message = value.ErrorMessage,Code = "ValidationErrorKey"};
                            response.Error.Errors.Add(validationErrorResponse);
                        }
                    }
                }
                throw new ValidationException(response);
            }
        }
    }
}