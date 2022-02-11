using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Api.Filters;


public class XUserAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _userType; 

    public XUserAuthorizationFilterAttribute(string usersType)
    {
        _userType = usersType;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        context.HttpContext.Request.Headers.TryGetValue("X-User", out var userType);
        if (string.IsNullOrWhiteSpace(userType))
        {
            context.Result = new UnauthorizedResult();
        }
        else if (!_userType.Equals(userType.ToString(), StringComparison.InvariantCultureIgnoreCase))
        {
            context.Result = new ForbidResult();
        }
    }
}