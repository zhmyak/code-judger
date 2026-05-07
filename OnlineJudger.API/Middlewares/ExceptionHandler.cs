using Microsoft.AspNetCore.Mvc;
using OnlineJudger.Application.Exceptions;

namespace OnlineJudger.API.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;
        public ExceptionHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = ex switch
                {
                    AuthException => StatusCodes.Status401Unauthorized,
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Type = ex.GetType().FullName,
                    Title = "Ошибка",
                    Detail = ex.Message
                });
            }
        }
    }
}
