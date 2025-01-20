using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using VIS_API.Models;
using VIS_API.Repositories.Interface;
using VIS_API.Utilities;

namespace VIS_API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ErrorHandlingMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var exceptionHandler = scope.ServiceProvider.GetRequiredService<ExceptionHandler>();
                    await exceptionHandler.ExecuteWithExceptionHandlingAsync(
                        async () => { throw ex; },
                        e => Console.WriteLine($"Error in middleware: {e.Message}")
                    );
                }

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An unexpected error occurred.");
            }
        }
    }
}
