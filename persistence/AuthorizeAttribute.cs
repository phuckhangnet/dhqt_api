using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public  void OnAuthorization(AuthorizationFilterContext context)
    {
        
        var isTokenExpired = context.HttpContext.Items["Token_Expired"];
        if (isTokenExpired != null ) {
            context.Result = new JsonResult(new { message = "Token has expired" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
        else {
            var user = context.HttpContext.Items["ID"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }

}