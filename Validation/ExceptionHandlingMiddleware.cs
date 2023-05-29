using System.Text.Json;

namespace Project.Validation
{


public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Project.Validation.Models.ValidationException e)
        {
            //Console.WriteLine(e.ValidationResultModel.StatusCode);
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Project.Validation.Models.ValidationException exception)
    {
    
        httpContext.Response.StatusCode =  (int)exception.ValidationResultModel.STATUSCODE ; //StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(exception.ValidationResultModel));

    }

}

}