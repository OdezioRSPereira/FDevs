using FDevsQuiz.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FDevsQuiz.Application.Middleware
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static string SerializeObject<TValue>(TValue value)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            return JsonSerializer.Serialize(value, options);
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ExceptionMiddleware> _logger)
        {
            context.Response.ContentType = "application/json";

            if (exception is ValidateException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(SerializeObject(new IdentityError { Code = "ValidateError", Description = exception.Message }));
            }
            else
            {
                _logger.LogError(exception.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync(SerializeObject(new IdentityError { Code = "InternalError", Description = exception.Message }));
            }
        }
    }
}
